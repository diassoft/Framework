using Diassoft.Commanding.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diassoft.Commanding
{
    /// <summary>
    /// Represents a network communication session
    /// </summary>
    internal class NetworkCommunicationSession
    {
        /// <summary>
        /// The client to which the communication is happening
        /// </summary>
        public TcpClient Client { get; }
        /// <summary>
        /// The ID of the Session
        /// </summary>
        /// <remarks>Retrieve the ID using the <see cref="CommandProcessor.BeginSession"/> method</remarks>
        public string ID { get; }
        /// <summary>
        /// The Status of the session
        /// </summary>
        public SessionStatus Status { get; protected set; }
        /// <summary>
        /// Defines whether to show a timer when running commands
        /// </summary>
        /// <remarks>It's good to display the timer as it will provide some feedback for the user calling and waiting for a command response</remarks>
        protected bool ShowTimer { get; private set; } = false;
        /// <summary>
        /// Defines whether to show a command prompt symbol so the user knows the session is waiting for a command input
        /// </summary>
        protected bool ShowPrompt { get; private set; } = false;
        /// <summary>
        /// The Network Stream communication
        /// </summary>
        protected NetworkStream NetworkStream { get; }
        /// <summary>
        /// Event triggered when a command is received
        /// </summary>
        public event EventHandler<RawCommandInputReceivedEventArgs> RawCommandReceived;

        /// <summary>
        /// Initializes the Network Communication Session
        /// </summary>
        /// <param name="client">The client to which the communication is happening</param>
        /// <param name="sessionID">The Session ID. Retrieve it from the <see cref="CommandProcessor.BeginSession"/></param>
        /// <param name="showTimer">Defines whether to show a timer when running commands</param>
        /// <param name="showPrompt">Defines whether to show a command prompt symbol so the user knows the session is waiting for a command input</param>
        public NetworkCommunicationSession(TcpClient client, string sessionID, bool showTimer, bool showPrompt)
        {
            this.Client = client;
            this.NetworkStream = client.GetStream();
            this.ID = sessionID;
            this.Status = SessionStatus.None;
            this.ShowTimer = showTimer;
            this.ShowPrompt = showPrompt;
        }

        /// <summary>
        /// Runs the Communication Session
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        public void Run(CancellationToken cancellationToken)
        {
            // Session has just been created, output data
            RunAsync(cancellationToken).ContinueWith(t =>
            {

            }, cancellationToken);
        }

        /// <summary>
        /// Stops the session and disposes all the elements
        /// </summary>
        public void Stop()
        {
            // If session is running, then close it
            if (this.Status == SessionStatus.Running)
            {
                // Ensures to close the Network Stream
                if (this.NetworkStream != null)
                {
                    this.NetworkStream.Close();
                    this.NetworkStream.Dispose();
                }

                // Closes the Tcp Client
                if (this.Client != null)
                    this.Client.Close();
            }
        }

        /// <summary>
        /// Runs the Communication Session
        /// </summary>
        /// <param name="cancellationToken">The Cancellation Token</param>
        /// <returns>The executing task</returns>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            // Make sure the session is not a closed session
            if (this.Status == SessionStatus.Stopped)
                throw new InvalidOperationException("Session is already stopped and cannot be reused");

            // Set the Status to Running
            this.Status = SessionStatus.Running;

            // Output Welcome Message
            await ReplyAsync(cancellationToken, "Welcome!");
            if (ShowPrompt) await PrintPromptAsync(cancellationToken);

            // Listen to following session requests
            while (cancellationToken.IsCancellationRequested == false)
            {
                // Retrieve the command input
                var command = await ReadCommandAsync(cancellationToken).ConfigureAwait(false);

                if (command == null)
                    break;

                // Process Internal Commands (since there are very few commands, they are hard coded)
                var commandUpper = command.ToUpper();

                if (commandUpper == "SET TIMER ON")
                    ShowTimer = true;
                else if (commandUpper == "SET TIMER OFF")
                    ShowTimer = false;
                else if (commandUpper == "SET PROMPT ON")
                    ShowPrompt = true;
                else if (commandUpper == "SET PROMPT OFF")
                    ShowPrompt = false;
                else if (commandUpper == "QUIT")
                    break;
                else
                {
                    // Other commands, process properly

                    // Raise Command Received Event
                    var commandEventArgs = new RawCommandInputReceivedEventArgs(command);

                    // Create the Processing Signal
                    CancellationTokenSource tokenSource = new CancellationTokenSource();
                    CancellationToken token = tokenSource.Token;

                    var taskProcessing = new Task(async () =>
                    {
                        var capturedToken = token;
                        var characters = new char[] { '|', '/', '-', '\\' };
                        int currentChar = 0;

                        while (token.IsCancellationRequested == false)
                        {
                            await ReplyAsync(cancellationToken, "\r" + characters[currentChar], true, false);
                            currentChar++;
                            if (currentChar == 4) currentChar = 0;

                            await Task.Delay(250);
                        }
                    }, token);

                    if (ShowTimer)
                        taskProcessing.Start();

                    RawCommandReceived?.Invoke(this, commandEventArgs);

                    // Cancel the processing thing
                    if (ShowTimer)
                        tokenSource.Cancel();

                    // Reply to command
                    if (String.IsNullOrEmpty(commandEventArgs.Response))
                        await ReplyAsync(cancellationToken, "\rNo Response");
                    else
                        await ReplyAsync(cancellationToken, "\r" + commandEventArgs.Response);
                }

                // Display Prompt for Next Command
                if (ShowPrompt) await PrintPromptAsync(cancellationToken);
            }

            // Stop Session
            this.Stop();
        }

        /// <summary>
        /// Read the communication stream until reaching the EOF character (CrLf)
        /// </summary>
        /// <param name="cancellationToken">The Cancellation Token</param>
        /// <returns>A string containing the command</returns>
        private async Task<string> ReadCommandAsync(CancellationToken cancellationToken)
        {
            // Assume the delimiter is CrLf
            return await ReadCommandAsync(cancellationToken, new byte[] { 13, 10 });
        }

        /// <summary>
        /// Read the communication stream until reaching the EOF character configured by the <paramref name="delimiter"/>
        /// </summary>
        /// <param name="cancellationToken">The Cancellation Token</param>
        /// <param name="delimiter">A byte array to define what is the character set to end a command</param>
        /// <returns>A string containing the command</returns>
        private async Task<string> ReadCommandAsync(CancellationToken cancellationToken, byte[] delimiter)
        {
            // Get referece to the NetworkStream
            bool foundDelimiter = false;

            string command = "";

            // While the delimiter has not been found, keep reading data
            while (!foundDelimiter)
            {
                var buffer = new byte[1024];
                int bytesRead = await NetworkStream.ReadAsync(buffer, 0, buffer.Length).WithCancellation(cancellationToken);

                if (bytesRead == 0)
                    return null;

                if (delimiter.Length >= bytesRead)
                {
                    foundDelimiter = true;
                    for (int iByte = 0; iByte < bytesRead; iByte++)
                    {
                        if (delimiter[iByte] != buffer[iByte])
                        {
                            foundDelimiter = false;
                            break;
                        }
                    }

                    if (!foundDelimiter)
                    {
                        var data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        command += data;
                    }
                }
            }

            return command;
        }

        /// <summary>
        /// Writes a message to the Network Stream
        /// </summary>
        /// <param name="cancellationToken">The Cancellation Token</param>
        /// <param name="message">The message to be written</param>
        /// <param name="autoFlush">A flag to define whether the message is automatically flushed to the stream. By default, it is set to true</param>
        /// <param name="breakLine">A flag to define whether the line will break or not</param>
        /// <returns>The executing task</returns>
        private async Task ReplyAsync(CancellationToken cancellationToken, string message, bool autoFlush = true, bool breakLine = true)
        {
            var newLine = new string(new[] { (char)13, (char)10 });
            var byteData = Encoding.ASCII.GetBytes(message + (breakLine ? newLine : ""));

            await NetworkStream.WriteAsync(byteData, 0, byteData.Length).ConfigureAwait(false);

            if (autoFlush)
                await FlushAsync(cancellationToken);
        }

        /// <summary>
        /// Print the Prompt Character before waiting for a new command input
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The original task</returns>
        private async Task PrintPromptAsync(CancellationToken cancellationToken)
        {
            await ReplyAsync(cancellationToken, "\r> ", true, false);
        }

        /// <summary>
        /// Flushes the message to the Network Stream
        /// </summary>
        /// <param name="cancellationToken">The Cancellation Token</param>
        /// <returns>The executing task</returns>
        private async Task FlushAsync(CancellationToken cancellationToken)
        {
            await NetworkStream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Define the valid status for a Network Communication Session
    /// </summary>
    public enum SessionStatus: byte
    {
        /// <summary>
        /// Session is created but nothing happened
        /// </summary>
        None = 0,
        /// <summary>
        /// Session is running
        /// </summary>
        Running = 1,
        /// <summary>
        /// Session is stopped
        /// </summary>
        Stopped = 2
    }
}
