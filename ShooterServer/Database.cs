using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace ShooterServer
{
    public class Database
    {

        private string server = "89.163.221.3";
        private string database = "s1_Server";
        private string uid = "u1_7lp4U9mzdc";
        private string password = "n+PT1JiuV9qdfsCwp6sfKy+g";

        public MySqlConnection connection;

        public Database()
        {
            string conn = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(conn);
            try
            {
                connection.Open();
                Console.WriteLine("Connected to Database");
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                    default:
                        Console.WriteLine("Error with Database");
                        break;
                }
            }
            MySqlCommand create1 = new MySqlCommand("CREATE TABLE IF NOT EXISTS Users(ID INT NOT NULL AUTO_INCREMENT, email VARCHAR(255) NOT NULL, username VARCHAR(255) NOT NULL, password VARCHAR(255) NOT NULL, baned BOOLEAN NOT NULL DEFAULT FALSE, banreason VARCHAR(255) NOT NULL, bantime bigint(20) NOT NULL DEFAULT -1, PRIMARY KEY (ID))", connection);
            create1.ExecuteNonQuery();
        }

        public int Query(string command)
        {
            return new MySqlCommand(command, connection).ExecuteNonQuery();
        }

        public int Query(MySqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        public MySqlDataReader GetData(string command)
        {
            MySqlCommand mcommand = new MySqlCommand(command, connection);
            Console.WriteLine(command);
            MySqlDataReader ret = mcommand.ExecuteReader();
            ret.Close();
            return ret;
        }

        public MySqlDataReader GetData(MySqlCommand command)
        {
            return command.ExecuteReader();
        }

    }
}
