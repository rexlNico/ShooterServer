using System;
using KaymakNetwork.Network.Server;
using KaymakNetwork.Network;

namespace ShooterServer
{
    internal static class NetworkConfig
    {
        private static Server _socket;

        internal static Server socket
        {
            get
            {
                return _socket;
            }
            set
            {
                if(_socket != null)
                {
                    _socket.ConnectionReceived -= Socke_ConnectionReceived;
                    _socket.ConnectionLost -= Socket_ConnectionLost;
                }

                _socket = value;
                if(_socket != null)
                {
                    _socket.ConnectionReceived += Socke_ConnectionReceived;
                    _socket.ConnectionLost += Socket_ConnectionLost;
                }

            }
        }

        internal static void InitNetwork()
        {
            if (!(socket == null)) return;
            socket = new Server(1000)
            {
                BufferLimit = 2048000,
                PacketAcceptLimit = 100000,
                PacketDisconnectCount = 200000
            };

            NetworkReceive.PacketRouter();

        }

        internal static void Socke_ConnectionReceived(int connectionID)
        {
            Console.WriteLine("Connection received on index[" + connectionID + "]");
            //NetworkSend.WelomeMessage(connectionID, "Welcome to the Server!");
            //GameManager.CreatePlayer(connectionID);
        }

        internal static void Socket_ConnectionLost(int connectionID)
        {
        Console.WriteLine("Connectrion removed on index[" + connectionID + "]");
            if (GameManager.playerList.ContainsKey(connectionID))
            {
                GameManager.SavePlayer(GameManager.playerList[connectionID]);
                GameManager.playerList.Remove(connectionID);
            }
            //Player player = GameManager.playerList[connectionID];
            //player.SavePlayer();
            //NetworkSend.DeletePlayer(connectionID);
            //GameManager.playerList.Remove(connectionID);
        }

    }
}
