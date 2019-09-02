using Diassoft.Commanding.Parsers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents the base class of a command processor
    /// </summary>
    public abstract class CommandProcessor
    {
        #region Default Status Codes

        /// <summary>
        /// The Default Sucess Status Code to return
        /// </summary>
        public int DefaultSuccessStatus { get; set; } = 200;
        /// <summary>
        /// The Default Error Status Code to return
        /// </summary>
        public int DefaultErrorStatus { get; set; } = 500;
        /// <summary>
        /// The Default Not Found Status Code to return
        /// </summary>
        public int DefaultNotFoundStatus { get; set; } = 404;

        #endregion Default Status Codes

        /// <summary>
        /// Internal object for locking the <see cref="SingletonObjectsCollectionInternal"/>
        /// </summary>
        /// <remarks>This class is necessary to avoid a reassignment of the <see cref="SingletonObjectsCollectionInternal"/> memory address to cause the data to go out of sync</remarks>
        private readonly object SingletonObjectsCollectionInternalLock = new object();
        /// <summary>
        /// A Dictionary containing the command classes registered, by type
        /// </summary>
        internal Dictionary<Type, object> SingletonObjectsCollectionInternal { get; private set; } = new Dictionary<Type, object>();

        /// <summary>
        /// Internal object for locking the <see cref="CommandClassCollectionInternal"/>
        /// </summary>
        private readonly object CommandClassCollectionInternalLock = new object();
        /// <summary>
        /// A dictionary containing the commad classes 
        /// </summary>
        internal Dictionary<Type, CommandClassInfo> CommandClassCollectionInternal { get; private set; } = new Dictionary<Type, CommandClassInfo>();

        /// <summary>
        /// Internal object for locing the <see cref="CommandCollectionInternal"/>
        /// </summary>
        private readonly object CommandCollectionInternalLock = new object();
        /// <summary>
        /// A dictionary containing the commands and its relevant information for calling it
        /// </summary>
        internal Dictionary<string, CommandInfo> CommandCollectionInternal { get; private set; } = new Dictionary<string, CommandInfo>();

        /// <summary>
        /// Internal object for locking the <see cref="SessionCollectionInternal"/>
        /// </summary>
        private readonly object SessionCollectionInternalLock = new object();
        /// <summary>
        /// A Concurrent Dictionary containing session information by a unique ID
        /// </summary>
        /// <remarks>Sessions are created thru the <see cref="BeginSession"/> method. Once the session is finished, call the <see cref="EndSession(string)"/> method</remarks>
        internal Dictionary<string, SessionInfo> SessionCollectionInternal { get; private set; } = new Dictionary<string, SessionInfo>();

        /// <summary>
        /// An event triggered before a command runs
        /// </summary>
        public event EventHandler<CommandReceivedEventArgs> CommandReceived;

        /// <summary>
        /// An event triggered when a command is being registered
        /// </summary>
        public event EventHandler<RegisteringCommandEventArgs> RegisteringCommand;

        /// <summary>
        /// An event triggered when a command is being registered by it is found to be duplicated
        /// </summary>
        /// <remarks>Use the <see cref="DuplicatedCommandEventArgs.Action"/> to define how to handle duplcated commands. If nothing is informed, the system will use the default action of <see cref="DuplicatedCommandActions.Ignore"/></remarks>
        public event EventHandler<DuplicatedCommandEventArgs> RegisteringDuplicatedCommand;

        /// <summary>
        /// The Parser for the Command Input
        /// </summary>
        /// <remarks>Use different parsers to accept different types of command input</remarks>
        public ICommandParser CommandParser { get; set; } = new DefaultCommandParser();

        /// <summary>
        /// Register a type as a Command Class
        /// </summary>
        /// <remarks>The type must implement the <see cref="CommandClassAttribute"/></remarks>
        /// <typeparam name="TCommandClassType">Type to be registered</typeparam>
        public void RegisterCommandClass<TCommandClassType>()
        {
            RegisterCommandClass<TCommandClassType>(null);
        }

        /// <summary>
        /// Register a type as a Command Class
        /// </summary>
        /// <param name="creationFunction">A <see cref="Func{T, TResult}"/> to define how the object is instantiated</param>
        /// <remarks>The type must implement the <see cref="CommandClassAttribute"/></remarks>
        /// <typeparam name="TCommandClassType">Type to be registered</typeparam>
        public void RegisterCommandClass<TCommandClassType>(Func<object> creationFunction)
        {
            // Command Class Type
            Type commandClassType = typeof(TCommandClassType);

            // Check if object implement the "CommandClassAttribute"
            if (!(commandClassType.GetCustomAttributes(true).Where(t => t is CommandClassAttribute)
                                                            .Select(t1 => t1)
                                                            .First() is CommandClassAttribute commandClassAttribute))
                throw new Exception($"Class must implement the {nameof(CommandClassAttribute)}");

            // Add registration information to the proper collection (make sure object is locked for thread-safety
            lock (CommandClassCollectionInternalLock)
            {
                if (CommandClassCollectionInternal.ContainsKey(commandClassType))
                {
                    // Command Class already registered
                    throw new Exception($"Command Class {commandClassType.Name} is already registered");
                }

                // Register the command class info
                var commandClassInfo = new CommandClassInfo()
                {
                    CommandClassAttribute = commandClassAttribute,
                    CommandClassType = commandClassType,
                    CreationFunction = creationFunction
                };

                CommandClassCollectionInternal.Add(commandClassType, commandClassInfo);
            }

            // Search Methods to find Commands
            foreach (var method in commandClassType.GetMethods())
            {
                // Loop thru all CommandAttribute's tied to the method (multiple names for the same command are acceptable)
                foreach (var commandAttribute in from methodAttribute
                                                 in method.GetCustomAttributes(true)
                                                 where methodAttribute is CommandAttribute
                                                 select methodAttribute as CommandAttribute)
                {
                    // Trigger the registering command event
                    var registeringCommandEventArgs = new RegisteringCommandEventArgs(commandAttribute.Command);
                    RegisteringCommand?.Invoke(this, registeringCommandEventArgs);

                    // Make sure command is allowed to be registered
                    if (!registeringCommandEventArgs.SuppressRegistration)
                    {
                        lock (CommandCollectionInternalLock)
                        {
                            // Process Duplicated Commands
                            if (CommandCollectionInternal.ContainsKey(commandAttribute.Command))
                            {
                                // Call duplicated event to decide what to do
                                var duplicatedCommandEventArgs = new DuplicatedCommandEventArgs(commandAttribute.Command, DuplicatedCommandActions.Ignore);
                                RegisteringDuplicatedCommand?.Invoke(this, duplicatedCommandEventArgs);

                                if (duplicatedCommandEventArgs.Action == DuplicatedCommandActions.Error)
                                    // ERROR: Command is duplicated
                                    throw new Exception($"Command '{commandAttribute.Command}' is duplicated and cannot be registered");
                                else if (duplicatedCommandEventArgs.Action == DuplicatedCommandActions.Replace)
                                    // Remove current command and let registration continue
                                    CommandCollectionInternal.Remove(commandAttribute.Command);
                                else
                                    // Skip to the next entry on the foreach loop
                                    continue;
                            }

                            // Setup the necessary information to run a command later
                            var commandInfo = new CommandInfo()
                            {
                                CommandAttribute = commandAttribute,
                                CommandClassType = commandClassType,
                                MethodInfo = method,
                                ParametersInfo = method.GetParameters(),
                            };

                            // Initialize the ParameterInfo array into an empty array if needed
                            if (commandInfo.ParametersInfo == null)
                                commandInfo.ParametersInfo = new ParameterInfo[] { };

                            // Include Command Help Attributes on the CommandInfo
                            foreach (var commandHelpAttribute in from methodAttribute
                                                                 in method.GetCustomAttributes(typeof(CommandHelpAttribute), true)
                                                                 select methodAttribute as CommandHelpAttribute)
                            {
                                if (String.IsNullOrEmpty(commandHelpAttribute.CultureName))
                                    commandInfo.CommandHelpAttributes.Add("DEFAULT", commandHelpAttribute);
                                else
                                    commandInfo.CommandHelpAttributes.Add(commandHelpAttribute.CultureName, commandHelpAttribute);
                            }

                            // Add to the collection
                            CommandCollectionInternal.Add(commandAttribute.Command, commandInfo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new session
        /// </summary>
        /// <returns>The ID of the new session</returns>
        protected string BeginSession()
        {
            lock (SessionCollectionInternalLock)
            {
                // Initializes a new session
                var newSession = new SessionInfo();

                while (SessionCollectionInternal.ContainsKey(newSession.ID))
                {
                    // Keep pulling new sessions until it does not exists on the collection
                    newSession = new SessionInfo();
                }

                // Add the new session
                SessionCollectionInternal.Add(newSession.ID, newSession);

                // Returns the new session ID
                return newSession.ID;
            }
        }

        /// <summary>
        /// Finishes a given session by destroying all objects inside it
        /// </summary>
        /// <param name="sessionID">The ID Session</param>
        protected void EndSession(string sessionID)
        {
            lock (SessionCollectionInternalLock)
            {
                // Fetch Session
                if (SessionCollectionInternal.TryGetValue(sessionID, out var sessionInfo))
                {
                    // Loop thru objects and call dispose, if applicable
                    foreach (var kvp in sessionInfo.ObjectCollection)
                    {
                        if (kvp.Value is IDisposable disposable)
                            disposable.Dispose();
                    }

                    // Remove session from collection
                    SessionCollectionInternal.Remove(sessionID);
                }
            }
        }

        /// <summary>
        /// Executes a command asynchronously
        /// </summary>
        /// <param name="sessionID">The ID of the session running the command</param>
        /// <param name="inputCommand">The full command input</param>
        /// <returns>A <see cref="CommandExecutionResponse"/> containing the results of the execution</returns>
        protected async Task<CommandExecutionResponse> ExecuteCommandAsync(string sessionID, string inputCommand)
        {
            try
            {
                // Notify CommandReceived event
                CommandReceived?.Invoke(this, new CommandReceivedEventArgs(inputCommand, sessionID));

                // Parse Full Command, by separating what is the command from what are the arguments
                var ResultingCommand = this.CommandParser.Parse(inputCommand);

                // Try to assign each parameter
                var methodArgumentsList = new List<object>();

                // If arguments are not specified, set an empty array to it
                if (ResultingCommand.Arguments == null)
                    ResultingCommand.Arguments = new ParsedCommandArgument[] { };

                // Look for command
                if (CommandCollectionInternal.TryGetValue(ResultingCommand.Command, out var commandInfo))
                {
                    // Check for Help Call
                    if (ResultingCommand.IsHelpRequest)
                    {
                        // Display the command help
                        var commandHelpAttribute = commandInfo.GetCommandHelpAttribute();
                        if (commandHelpAttribute == null)
                            return CommandExecutionResponse.Help("");

                        return CommandExecutionResponse.Help(commandHelpAttribute.Contents);
                    }
                    else
                    {
                        // Check if required parameters have been informed, if they were not, display help (if available)

                        // Assign Parameters
                        if (ResultingCommand.IsArgumentParameterSpecific)
                        {
                            // The arguments have a parameter name or a identifier, set them in sequence to be used later
                            var currentArgumentsDictionary = new Dictionary<string, ParsedCommandArgument>();
                            foreach (var r in ResultingCommand.Arguments)
                            {
                                if (currentArgumentsDictionary.ContainsKey(r.Parameter.ToLower()))
                                    throw new Exception($"Parameter '{r.Parameter.ToLower()}' is repeated on the syntax");

                                currentArgumentsDictionary.Add(r.Parameter.ToLower(), r);
                            }

                            var newArgumentsList = new List<ParsedCommandArgument>();

                            foreach (var p in commandInfo.ParametersInfo)
                            {
                                if (currentArgumentsDictionary.TryGetValue(p.Name.ToLower(), out var parsedCommandArgument))
                                    throw new Exception($"Parameter '{p.Name.ToLower()}' is not informed");

                                newArgumentsList.Add(parsedCommandArgument);
                            }

                            ResultingCommand.Arguments = newArgumentsList.ToArray();
                        }

                        // Make sure the number of arguments match the method argument number
                        if (ResultingCommand.Arguments.Length == commandInfo.ParametersInfo.Length)
                        {

                            for (int iArgument = 0; iArgument < ResultingCommand.Arguments.Length; iArgument++)
                            {
                                // Get Variables
                                var param = commandInfo.ParametersInfo[iArgument];
                                var argument = ResultingCommand.Arguments[iArgument];

                                // Try to apply parameter
                                try
                                {
                                    if (param.ParameterType == typeof(string))
                                    {
                                        // A direct conversion is available
                                        methodArgumentsList.Add(argument.Value);
                                    }
                                    else
                                    {
                                        // There is no direct conversion, try to apply the parameter using the Convert object
                                        methodArgumentsList.Add(Convert.ChangeType(argument, param.ParameterType));
                                    }

                                }
                                catch (Exception eAssignment)
                                {
                                    // Throw an error with the invalid argument
                                    throw new InvalidCommandArgumentsException(ResultingCommand.Command, param.Name, param.ParameterType, argument.Value, eAssignment);
                                }
                            }
                        }
                        else
                        {
                            // Number of arguments do not match, return help
                            var argumentException = new ArgumentException("The number of arguments needed for the command does not match the number of arguments provided");

                            var commandHelpAttribute = commandInfo.GetCommandHelpAttribute();
                            if (commandHelpAttribute == null)
                                return CommandExecutionResponse.Help("", DefaultErrorStatus, argumentException);

                            return CommandExecutionResponse.Help(commandHelpAttribute.Contents, DefaultErrorStatus, argumentException);
                        }

                        // Arguments are set

                        // Define the CommandClass where the command will be executed against
                        var commandObject = (object)null;

                        // Look for Session Information to start fetching / creating objects
                        if (SessionCollectionInternal.TryGetValue(sessionID, out SessionInfo sessionInfo))
                        {
                            // Session information found

                            // Retrieve command class info
                            if (CommandClassCollectionInternal.TryGetValue(commandInfo.CommandClassType, out var commandClassInfo))
                            {
                                // Check the CommandClass lifetime before trying to find the object where to run the command against
                                if (commandClassInfo.CommandClassAttribute.Lifetime == InstanceLifetimes.Singleton)
                                {
                                    // Singleton lifetime, only one instance of the object should exist

                                    // Make sure to lock the dictionary to force other threads to hold
                                    lock (SingletonObjectsCollectionInternalLock)
                                    {
                                        // Singleton - Object should be on the SingletonObjectCollectionInternal
                                        if (!(SingletonObjectsCollectionInternal.TryGetValue(commandInfo.CommandClassType, out commandObject)))
                                        {
                                            // Singleton object could not be found, then create it and add back to the collection

                                            // Create the Command Object
                                            commandObject = commandClassInfo.CreateInstance();

                                            // Adds the singleton object to the collection
                                            SingletonObjectsCollectionInternal.Add(commandInfo.CommandClassType, commandObject);
                                        }
                                    }
                                }
                                else if (commandClassInfo.CommandClassAttribute.Lifetime == InstanceLifetimes.Scoped)
                                {
                                    // Scoped lifetime, only one instance per session should exist

                                    // Check object on the Session Info
                                    if (!sessionInfo.ObjectCollection.TryGetValue(commandInfo.CommandClassType, out commandObject))
                                    {
                                        // Create and add object to the session
                                        lock (SessionCollectionInternalLock)
                                        {
                                            // Create the Command Object and add to collection
                                            commandObject = commandClassInfo.CreateInstance();
                                            sessionInfo.ObjectCollection.Add(commandInfo.CommandClassType, commandObject);
                                        }
                                    }
                                }
                                else
                                {
                                    // Transient lifetime, initializes it everytime
                                    commandObject = commandClassInfo.CreateInstance();
                                }
                            }
                            else
                            {
                                // Cannot find registration details for command class
                                throw new Exception($"Unable to find registration details for type '{commandInfo.CommandClassType.Name}'");
                            }
                        }
                        else
                        {
                            // Error, unable to find the session
                            throw new Exception($"Session {sessionID} cannot be found");
                        }

                        try
                        {
                            // Calls the Command Asynchronously
                            var result = await Task.Factory.StartNew<object>(() =>
                            {
                                return commandInfo.MethodInfo.Invoke(commandObject, methodArgumentsList.ToArray());
                            }
                            ).ConfigureAwait(false);

                            // Depending on the type of result, throw response
                            if (result is CommandExecutionResponse cr)
                                return cr;
                            else
                            {
                                // Command executed without exceptions, create a Success Response with no data
                                return CommandExecutionResponse.Success(DefaultSuccessStatus, result);
                            }

                        }
                        catch (Exception eExecution)
                        {
                            // Throws the execution exception ahead
                            throw new CommandInExecutionException(ResultingCommand.Command, methodArgumentsList.ToArray(), eExecution);
                        }
                    }
                }
                else
                {
                    // Command not found, throw exception
                    throw new CommandNotFoundException(ResultingCommand.Command);
                }

            }
            catch (InvalidCommandArgumentsException e)
            {
                return CommandExecutionResponse.Error(DefaultErrorStatus, e);
            }
            catch (CommandNotFoundException)
            {
                return CommandExecutionResponse.NotFound(DefaultNotFoundStatus);
            }
            catch (Exception e)
            {
                return CommandExecutionResponse.Error(DefaultErrorStatus, e);
            }

        }

        /// <summary>
        /// Method to be overwritten 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task StartAsync(CancellationToken cancellationToken);

    }

    internal class CommandClassInfo
    {
        public CommandClassAttribute CommandClassAttribute { get; internal set; }
        public Func<object> CreationFunction { get; internal set; }
        public Type CommandClassType { get; internal set; }

        public object CreateInstance()
        {
            // Singleton object could not be found, then create it and add back to the collection
            if (CreationFunction == null)
            {
                // Initializes the object using the Activator (it expects the class to have a parameterless constructor then)
                return Activator.CreateInstance(CommandClassType);
            }
            else
            {
                // Initialize using the given function
                return CreationFunction();
            }
        }
    }

    /// <summary>
    /// Represents the command information necessary to run it
    /// </summary>
    internal class CommandInfo
    {
        /// <summary>
        /// The type of the Command Container Class
        /// </summary>
        public Type CommandClassType { get; set; }
        /// <summary>
        /// The command attribute extracted from the 
        /// </summary>
        public CommandAttribute CommandAttribute { get; set; }
        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the  help information by language
        /// </summary>
        public Dictionary<string, CommandHelpAttribute> CommandHelpAttributes { get; set; } = new Dictionary<string, CommandHelpAttribute>();
        /// <summary>
        /// Method Information for calling the Command
        /// </summary>
        public MethodInfo MethodInfo { get; set; }
        /// <summary>
        /// Parameters of the command
        /// </summary>
        public ParameterInfo[] ParametersInfo { get; set; }

        /// <summary>
        /// Retrieves the Command Help attribute based on a given culture
        /// </summary>
        /// <returns>A <see cref="CommandHelpAttribute"/> containing the help information, or null if no <see cref="CommandHelpAttribute"/> is found</returns>
        public CommandHelpAttribute GetCommandHelpAttribute()
        {
            return GetCommandHelpAttribute(CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Retrieves the Command Help attribute based on a given culture
        /// </summary>
        /// <param name="cultureInfo">The culture to fetch data for</param>
        /// <returns>A <see cref="CommandHelpAttribute"/> containing the help information, or null if no <see cref="CommandHelpAttribute"/> is found</returns>
        public CommandHelpAttribute GetCommandHelpAttribute(CultureInfo cultureInfo)
        {
            // Check by Language/Country
            if (!(CommandHelpAttributes.TryGetValue(cultureInfo.Name, out CommandHelpAttribute commandHelpAttribute)))
            {
                // Check by Language/Country
                if (!(CommandHelpAttributes.TryGetValue(cultureInfo.TwoLetterISOLanguageName, out commandHelpAttribute)))
                {
                    // Check Default
                    if (!(CommandHelpAttributes.TryGetValue("DEFAULT", out commandHelpAttribute)))
                    {
                        // Return an empty response since there is no help for the command
                        return null;
                    }
                }
            }

            return commandHelpAttribute;
        }
    }

    /// <summary>
    /// Represents the Session Information
    /// </summary>
    internal class SessionInfo
    {
        /// <summary>
        /// Represents the Session Unique ID
        /// </summary>
        public string ID { get; private set; }
        /// <summary>
        /// The Date/Time when the Session Started
        /// </summary>
        public DateTime StartDateTime { get; private set; } = DateTime.MinValue;
        /// <summary>
        /// The Objects that belong to the given session
        /// </summary>
        public Dictionary<Type, object> ObjectCollection { get; private set; } = new Dictionary<Type, object>();

        /// <summary>
        /// Initializes a new Session Info
        /// </summary>
        public SessionInfo()
        {
            StartDateTime = DateTime.Now;
            ID = Guid.NewGuid().ToString();
        }



    }

}
