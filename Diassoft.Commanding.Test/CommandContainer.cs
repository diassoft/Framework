using Diassoft.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diassoft.Communication.Test.Network
{
    [CommandClass()]
    public class CommandContainer
    {

        public CommandContainer()
        {

        }

        [Command("COMMAND1", "Command 1")]
        public string Command1()
        {
            return $"Line 1\nLine 2\rLine 3";
        }

        [Command("COMMAND2", "Command 2")]
        public string Command2()
        {
            System.Threading.Thread.Sleep(5000);
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        [Command("COMMAND3", "Command 3")]
        [CommandHelp("COMMAND 3 HELP\n\n" +
            "Usage:\n" +
            "  GETTIME <format>\n\n" +
            "Examples:\n" +
            "  GETTIME \"yyyy-MM-dd\" --> 2019-01-01\n" +
            "  GETTIME \"MM-dd-yyyy HH:mm:ss\" --> 2019-01-01 10:00:00")]
        public void Command3(string parameter1, string parameter2)
        {
            Console.WriteLine("Parameter 1: " + parameter1);
            Console.WriteLine("Parameter 2: " + parameter2);
        }

        [Command("GETTIME", "Gets the current time")]
        [CommandHelp("" +
            "Usage:\n" +
            "  GETTIME <format>\n\n" +
            "Examples:\n" +
            "  GETTIME \"yyyy-MM-dd\" --> 2019-01-01\n" +
            "  GETTIME \"MM-dd-yyyy HH:mm:ss\" --> 2019-01-01 10:00:00")]
        public string GetTime(string format)
        {
            return DateTime.Now.ToString(format);
        }

    }
}
