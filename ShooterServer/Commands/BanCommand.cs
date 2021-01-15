using System;
using MySql.Data.MySqlClient;

namespace ShooterServer.Commands
{
    class BanCommand : Command
    {
        public BanCommand() : base("ban <username> <reason> -> bans a user") { }

        public override void ExecuteCommand(string command, string args)
        {
            if (args.Split(" ").Length == 1)
            {
                string username = args.Trim();
                MySqlDataReader reader = Program.database.GetData("SELECT baned FROM Users WHERE username='" + username + "'");
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.GetBoolean("baned"))
                    {
                        reader.Close();
                        Program.database.Query("UPDATE Users SET baned=1, bantime=-1 WHERE username='" + username + "'");
                        Console.WriteLine("Baned Player " + username);
                    }
                    else
                    {
                        reader.Close();
                        Console.WriteLine("Player " + username + " is already baned");
                    }
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("Player " + username + " does not exist");
                }
            }
            else
            {
                string[] list = args.Split(" ");
                string username = list[0];
                string reason = args.Replace(username + " ", "");
                MySqlDataReader reader = Program.database.GetData("SELECT baned FROM Users WHERE username='" + username + "'");
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.GetBoolean("baned"))
                    {
                        reader.Close();
                        Program.database.Query("UPDATE Users SET baned=1, banreason='"+reason+"', bantime=-1 WHERE username='" + username + "'");
                        Console.WriteLine("Baned Player " + username +" with reason > "+reason);
                    }
                    else
                    {
                        reader.Close();
                        Console.WriteLine("Player " + username + " is already baned");
                    }
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("Player " + username + " does not exist");
                }
            }
        }
    }
}
