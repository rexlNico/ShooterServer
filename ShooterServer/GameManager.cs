using System;
using System.Collections.Generic;
using System.Text;

namespace ShooterServer
{
    static class GameManager
    {
        public static Dictionary<int, Player> playerList = new Dictionary<int, Player>();
        public static float gravity = -19.62f * 1.5f;

        public static void JoinGame(int connectionID, Player player)
        {
            NetworkSend.InstantiateNetworkPlayer(connectionID, player);
        }

        public static void CreatePlayer(int connectionID)
        {
            String name = "";
            Player player = TryToLoadPlayer(connectionID, name);
            playerList.Add(connectionID, player);
            Console.WriteLine("Player {1} has been added to the game with id {0}", connectionID, name);
            JoinGame(connectionID, player);
        }

        public static void SavePlayer(Player player)
        {

        }

        public static Player TryToLoadPlayer(int connectionID, string username)
        {
            return new Player()
            {
                connectionID = connectionID,
                inGame = true
            }; ;
        }

    }
}
