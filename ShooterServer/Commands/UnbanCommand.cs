using System;
using MySql.Data.MySqlClient;

namespace ShooterServer.Commands
{
    class UnbanCommand : Command
    {
        public UnbanCommand() : base("unban <username> -> unbans a user"){}

        public override void ExecuteCommand(string command, string args)
        {
            if(args.Split(" ").Length == 1)
            {
                string username = args.Trim();
                MySqlDataReader reader = Program.database.GetData("SELECT baned FROM Users WHERE username='"+username+"'");
                reader.Read();
                if (reader.HasRows)
                {
                    if (reader.GetBoolean("baned"))
                    {
                        reader.Close();
                        Program.database.Query("UPDATE Users SET baned=0 WHERE username='" + username + "'");
                        Console.WriteLine("Unbaned Player " + username);
                    }
                    else
                    {
                        reader.Close();
                        Console.WriteLine("Player "+username+" is not baned");
                    }
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("Player "+username+" does not exist");
                }
            }
            else
            {
                Console.WriteLine(help);
            }
        }
    }
}
