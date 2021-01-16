using ShooterServer.Commands;
using System;
using System.Collections.Generic;

namespace ShooterServer
{
    public class CommandManager
    {

        public static Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public static void InitCommandManager()
        {
            commands.Add("help", new HelpCommand());
            commands.Add("ban", new BanCommand());
            commands.Add("unban", new UnbanCommand());
            commands.Add("stop", new StopCommand());
        }

        public static void ExecuteCommand(string input)
        {
            if(input.Length >= 1)
            {
                string[] list = input.Split(" ");
                if (commands.ContainsKey(list[0].ToLower()))
                {
                    Command command = commands[list[0].ToLower()];
                    if (list.Length > 1)
                    {
                        string args = "";
                        for (int i = 1; i < list.Length; i++)
                        {
                            args = args + " " + list[i];
                        }
                        command.ExecuteCommand(list[0], args.Trim());
                        return;
                    }
                    else command.ExecuteCommand(list[0], "");
                    return;
                }
                Console.WriteLine("Command "+ list[0].ToLower() + " does not exist!");
            }
        }
    }
}
