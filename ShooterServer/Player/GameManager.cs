using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using System.Numerics;

namespace ShooterServer
{
    static class GameManager
    {
        public static Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        public static float gravity = -19.62f * 1.5f;

        public static void SavePlayer(Player player)
        {
            TextWriter writer = null;
            try
            {
                string path = "playerdata/" + player.emai + ".pdata";
                var serializer = new XmlSerializer(typeof(Player));
                writer = new StreamWriter(path, false);
                serializer.Serialize(writer, player);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }

        public static Player TryToLoadPlayer(int connectionID, string username, string email)
        {
            string path = "playerdata/" + email + ".pdata";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                return new Player()
                {
                    connectionID = connectionID,
                    username = username,
                    emai = email,
                    location = new Vector3(720, 79, 463),
                    looking = new Quaternion(0, 0, 0, 0)
                };
            }
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Player));
                reader = new StreamReader(path);
                Player player = (Player)serializer.Deserialize(reader);
                player.connectionID = connectionID;
                return player;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static bool CanPlayerLogin(string email, string password)
        {
            MySqlDataReader reader = Program.database.GetData("SELECT baned FROM Users WHERE email='" + email + "' AND password='" + password + "'");
            if (reader.HasRows)
            {
                reader.Read();
                if (reader.GetBoolean("baned"))
                {
                    reader.Close();
                    return false;
                }
                else
                {
                    reader.Close();
                    return true;
                }
            }
            reader.Close();
            return false;
        }

    }
}
