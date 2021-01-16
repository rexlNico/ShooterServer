using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterServer.Commands
{
    class HelpCommand : Command
    {
        public HelpCommand() : base("help <command> -> Shows this help page") { }
        public override void ExecuteCommand(string command, string args)
        {
            if (args.Length == 0)
            {
                foreach (string cmd in CommandManager.commands.Keys)
                {
                    Console.WriteLine(CommandManager.commands[cmd].help);
                }
            }
            else
            {
                if (CommandManager.commands.ContainsKey(args.ToLower()))
                {
                    Console.WriteLine(CommandManager.commands[args.ToLower()].help);
                }
                else
                {
                    Console.WriteLine("Command " + args.ToLower() + " does not exist!");
                }
            }
        }
    }
}
