using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterServer
{
    public abstract class Command
    {
        public string help;
        public Command(string help)
        {
            this.help = help;
        }

        public abstract void ExecuteCommand(string command, string args);

    }
}
