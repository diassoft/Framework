using Diassoft.Commanding.Extensions;
using Diassoft.Commanding.Formatters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents a Network Traffic Listener waiting for commands to process
    /// </summary>
    public sealed class NetworkCommandListener: CommandProcessor
    {
        /// <summary>
        /// Represents the Default Buffer Size for Socket Communication
        /// </summary>
        public static uint DEFAULT_BUFFERSIZE = 1024;

        /// <summary>
        /// Lock object for the <see cref="NetworkCommunicationSessions"/> collection
        /// </summary>
        private readonly object NetworkCommunicationSessionsLock = new object();
        /// <summary>
        /// Represent the sessions
        /// </summary>
        private Dictionary<string, NetworkCommunicationSession> NetworkCommunicationSessions { get; set; } = new Dictionary<string, NetworkCommunicationSession>();

        /// <summary>
        /// The Tcp Listener to be listening to incoming requests
        /// </summary>
        private TcpListener Server { get; set; }

        /// <summary>
        /// The port to be listening
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The size of the communication buffer
        /// </summary>
        public uint BufferSize { get; set; } = DEFAULT_BUFFERSIZE;

        /// <summary>
        /// A formatter which will convert the <see cref="CommandExecutionResponse"/> into a string.
        /// </summary>
        /// <remarks>By default, it will use the <see cref="DefaultResponseFormatter"/>. You can create custom formatters by implementing the <see cref="IResponseFormatter"/> in a custom class.</remarks>
        public IResponseFormatter ResponseFormatter { get; set; } = new DefaultResponseFormatter();

        /// <summary>
        /// Defines whether to show the timer when processing a long running command
        /// </summary>
        /// <remarks>The timer is just "dots" that get added to the session until it is running</remarks>
        public bool ShowTimer { get; set; } = true;

        /// <summary>
        /// Defines whether to show a command prompt on the communication session
        /// </summary>
        /// <remarks>When this option is activated, the system will show a &gt; before the command input, which could be an easy indicator during telnet sessions</remarks>
        public bool ShowPrompt { get; set; } = true;

        #region Events

        /// <summary>
        /// Event triggered when the listening started
        /// </summary>
        public event EventHandler<ListeningStartedEventArgs> ListeningStarted; 
        /// <summary>
        /// Event triggered when a session started
        /// </summary>
        public event EventHandler<SessionStartedEventArgs> SessionStarted;
        /// <summary>
        /// Event triggered when a session ended
        /// </summary>
        public event EventHandler<SessionEndedEventArgs> SessionEnded;
        
        #endregion Events

        /// <summary>
        /// Initializes a new instance of the Network Command Processor
        /// </summary>
        public NetworkCommandListener(): this(0) { }

        /// <summary>
        /// Initializes a new instance of the Network Command Processor
        /// </summary>
        /// <param name="port">Port to be listening</param>
        public NetworkCommandListener(int port): this(port, 0) { }

        /// <summary>
        /// Initializes a new instance of the Network Command Processor
        /// </summary>
        /// <param name="port">Port to be listening</param>
        /// <param name="bufferSize">The size of the buffer</param>
        public NetworkCommandListener(int port, uint bufferSize)
        {
            Port = port;
            BufferSize = (bufferSize == 0 ? DEFAULT_BUFFERSIZE : bufferSize);
        }

        /// <summary>
        /// Starts Processing Commands
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A Task which performs the operation</returns>
        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            // Validate Port before starting
            if (Port == 0)
                throw new Exception("Invalid port number");

            // Initializes Server
            try
            {
                Server = new TcpListener(IPAddress.Any, Port);
                Server.Start(10);

                // Notify the listening started
                ListeningStarted?.Invoke(this, new ListeningStartedEventArgs(Port));

                while (cancellationToken.IsCancellationRequested == false)
                {
                    // Keep listening for incoming connections
                    var client = await Server.AcceptTcpClientAsync().WithCancellation(cancellationToken).ConfigureAwait(false);
                    cancellationToken.ThrowIfCancellationRequested();

                    // Creates the Communication Session
                    var sessionID = this.BeginSession();

                    // Notify that a session is started
                    var sessionStartedEventArgs = new SessionStartedEventArgs(sessionID);
                    SessionStarted?.Invoke(this, sessionStartedEventArgs);

                    // Make sure session is not denied
                    if (sessionStartedEventArgs.Deny)
                    {
                        // Session is denied, unregister it
                        this.EndSession(sessionID);
                    }
                    else
                    {
                        // Session is allowed, continue with it
                        var networkCommunicationSession = new NetworkCommunicationSession(client, sessionID, ShowTimer, ShowPrompt);
                        networkCommunicationSession.RawCommandReceived += (sender, commandEventArgs) =>
                        {
                            // Executes the command and waits for the answer
                            var taskResponse = Task.Run<CommandExecutionResponse>(async () => await ExecuteCommandAsync(networkCommunicationSession.ID, commandEventArgs.RawCommand));
                            var response = taskResponse.Result;

                            // Outputs the response
                            commandEventArgs.Response = this.ResponseFormatter.Format(response);

                            // Replace "\n" by the environment NewLine
                            commandEventArgs.Response = commandEventArgs.Response.Replace("\n", System.Environment.NewLine);
                        };

                        // Add session to the collection
                        lock (NetworkCommunicationSessionsLock)
                        {
                            NetworkCommunicationSessions.Add(networkCommunicationSession.ID, networkCommunicationSession);
                        }

#pragma warning disable 4014

                        // Run Communication Session
                        networkCommunicationSession.RunAsync(cancellationToken).ContinueWith(t =>
                        {
                            // Stop Session
                            networkCommunicationSession.Stop();

                            // Remove Session from Internal Controls
                            lock (NetworkCommunicationSessionsLock)
                            {
                                NetworkCommunicationSessions.Remove(networkCommunicationSession.ID);
                            }

                            // Unregister session with the Command Processor
                            this.EndSession(networkCommunicationSession.ID);

                            // Call SessionEnded event
                            SessionEnded?.Invoke(this, new SessionEndedEventArgs(networkCommunicationSession.ID));

                        }, cancellationToken);

#pragma warning restore 4014
                    }


                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Server != null)
                {
                    Server.Stop();
                }
            }


        }
    }

}
