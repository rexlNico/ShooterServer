using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterServer.Commands
{
    class StopCommand : Command
    {
        public StopCommand() : base("stop -> Stops the server") { }
        public override void ExecuteCommand(string command, string args)
        {
            if (args.Length == 0)
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine(help);
            }
        }
    }
}
