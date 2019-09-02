using System;
using System.Collections.Generic;
using System.Text;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents an object containing the response of a command execution
    /// </summary>
    public class CommandExecutionResponse
    {
        /// <summary>
        /// The basic command response
        /// </summary>
        /// <remarks>Return <see cref="CommandExecutionResponseStatus.Success"/> if the command has executed succesfully. Set to <see cref="CommandExecutionResponseStatus.Error"/> if any exception happened during the command execution.</remarks>
        public CommandExecutionResponseStatus Status { get; }

        /// <summary>
        /// The Custom Status Code specific to the application responding to commands
        /// </summary>
        public int CustomStatusCode { get; }

        /// <summary>
        /// A <see cref="Stack{T}"/> of <see cref="Exception"/> containing all errors that happened during the execution
        /// </summary>
        public Stack<Exception> Exceptions { get;  }

        /// <summary>
        /// The Data Returned from the Command
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionResponse"/>
        /// </summary>
        /// <param name="status">The status of the command execution</param>
        /// <param name="customStatusCode">A custom status for the command execution</param>
        /// <param name="exceptions">A <see cref="Stack{T}"/> of <see cref="Exception"/></param>
        /// <param name="commandData">An object containing command specific data returned</param>
        private CommandExecutionResponse(CommandExecutionResponseStatus status, int customStatusCode, object commandData): this(status, customStatusCode, null, commandData) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionResponse"/>
        /// </summary>
        /// <param name="status">The status of the command execution</param>
        /// <param name="customStatusCode">A custom status for the command execution</param>
        /// <param name="exceptions">A <see cref="Stack{T}"/> of <see cref="Exception"/></param>
        /// <param name="commandData">An object containing command specific data returned</param>
        private CommandExecutionResponse(CommandExecutionResponseStatus status, int customStatusCode, Stack<Exception> exceptions, object commandData)
        {
            Status = status;
            CustomStatusCode = customStatusCode;
            Exceptions = exceptions;
            Data = commandData;
        }

        #region Command Response Static Methods

        /// <summary>
        /// Returns a successfull command execution response
        /// </summary>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Success()
        {
            return Success(0);
        }

        /// <summary>
        /// Returns a successfull command execution response
        /// </summary>
        /// <param name="customStatusCode">A custom status code</param>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Success(int customStatusCode)
        {
            return Success(customStatusCode, null);
        }

        /// <summary>
        /// Returns a successfull command execution response
        /// </summary>
        /// <param name="customStatusCode">A custom status code</param>
        /// <param name="commandData">An object containing command additional data</param>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Success(int customStatusCode, object commandData)
        {
            return new CommandExecutionResponse(CommandExecutionResponseStatus.Success, customStatusCode, commandData);
        }

        /// <summary>
        /// Returns a help command response with no errors
        /// </summary>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Help(string commandHelp)
        {
            return new CommandExecutionResponse(CommandExecutionResponseStatus.Help, 0, commandHelp);
        }

        /// <summary>
        /// Returns a help command response with a single exception
        /// </summary>
        /// <param name="e">The exception that caused the error</param>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Help(string commandHelp, int customStatusCode, Exception e)
        {
            var exceptionStack = new Stack<Exception>();
            exceptionStack.Push(e);

            return Help(commandHelp, customStatusCode, exceptionStack);
        }

        /// <summary>
        /// Returns a help command with errors
        /// </summary>
        /// <param name="exceptions">A <see cref="Stack{T}"/> of <see cref="Exception"/> that happened during the command execution</param>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Help(string commandHelp, int customStatusCode, Stack<Exception> exceptions)
        {
            return new CommandExecutionResponse(CommandExecutionResponseStatus.HelpWithErrors, customStatusCode, exceptions, commandHelp);
        }

        /// <summary>
        /// Returns an erroneous command execution response
        /// </summary>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Error()
        {
            return Error(0);
        }

        /// <summary>
        /// Returns an erroneous command execution response
        /// </summary>
        /// <param name="customStatusCode">A custom status code</param>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Error(int customStatusCode)
        {
            return Error(customStatusCode, (Exception)null);
        }

        /// <summary>
        /// Returns an erroneous command execution response
        /// </summary>
        /// <param name="customStatusCode">A custom status code</param>
        /// <param name="e">The exception that caused the error</param>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Error(int customStatusCode, Exception e)
        {
            var exceptionStack = new Stack<Exception>();
            exceptionStack.Push(e);

            return Error(customStatusCode, exceptionStack);
        }

        /// <summary>
        /// Returns an erroneous command execution response
        /// </summary>
        /// <param name="customStatusCode">A custom status code</param>
        /// <param name="exceptions">A <see cref="Stack{T}"/> of <see cref="Exception"/> that happened during the command execution</param>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse Error(int customStatusCode, Stack<Exception> exceptions)
        {
            return new CommandExecutionResponse(CommandExecutionResponseStatus.Error, customStatusCode, exceptions, null);
        }

        /// <summary>
        /// Returns a Command Not Found execution response
        /// </summary>
        /// <param name="customStatusCode">A custom status code</param>
        /// <returns>An object containing the command execution results</returns>
        public static CommandExecutionResponse NotFound(int customStatusCode)
        {
            return new CommandExecutionResponse(CommandExecutionResponseStatus.CommandNotFound, customStatusCode, null);
        }

        #endregion Command Response Static Methods

    }


}
