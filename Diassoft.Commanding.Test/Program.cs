using Diassoft.Commanding.Parsers;
using Diassoft.Communication.Test.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diassoft.Commanding.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupServer();

            //var parser = new Diassoft.Commanding.Parsers.DefaultCommandParser();

            //ParsedCommand result;

            //while (true)
            //{
            //    Console.Write("Enter the command: ");
            //    var input = Console.ReadLine();

            //    if (input == "QUIT")
            //        break;

            //    try
            //    {
            //        result = parser.Parse(input);
            //        Console.WriteLine("Result ----> " + result.ToString());
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine($"Error: {e.Message}, {e.StackTrace}");
            //    }
            //}

            Console.ReadLine();
        }

        private async static void SetupServer()
        {
            // Register Command Processor
            var cmd = new NetworkCommandListener(42000);
            cmd.RegisterCommandClass<CommandContainer>();

            cmd.ListeningStarted += Cmd_ListeningStarted;
            cmd.SessionStarted += Cmd_SessionStarted;
            cmd.SessionEnded += Cmd_SessionEnded;
            cmd.CommandReceived += Cmd_CommandReceived;

            await cmd.StartAsync(CancellationToken.None);
        }

        private static void Cmd_CommandReceived(object sender, CommandReceivedEventArgs e)
        {
            Console.WriteLine($"Command Received on Session {e.SessionID}\n> {e.Command}");
        }

        private static void Cmd_ListeningStarted(object sender, ListeningStartedEventArgs e)
        {
            Console.WriteLine($"Listening Started on Port {e.Port}");
        }

        private static void Cmd_SessionStarted(object sender, SessionStartedEventArgs e)
        {
            Console.WriteLine("Session Started: " + e.SessionID);
        }

        private static void Cmd_SessionEnded(object sender, SessionEndedEventArgs e)
        {
            Console.WriteLine("Session Ended: " + e.SessionID);
        }


        private static void Cmd_ClientDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Client Disconnected...");
        }

    }
}
