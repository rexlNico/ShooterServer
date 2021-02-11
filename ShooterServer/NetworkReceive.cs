using KaymakNetwork;
using MySql.Data.MySqlClient;
using System;
using System.Numerics;

namespace ShooterServer
{
    enum ClientPackets
    {
        CPlayerLogin = 1,
        CPlayerQuit,
        CPlayerMove,
        CPlayerLook
    }

    internal static class NetworkReceive
    {

        internal static void PacketRouter()
        {
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerLogin] = Packet_PlayerLogin;
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerQuit] = Packet_PlayerQuit;
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerLook] = Packet_PlayerLook;
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerMove] = Packet_PlayerMove;
        }

        private static void Packet_PlayerQuit(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            Console.WriteLine("Disconnecting index[" + connectionID + "]");
            if (GameManager.playerList.ContainsKey(connectionID))
            {
                GameManager.SavePlayer(GameManager.playerList[connectionID]);
                GameManager.playerList.Remove(connectionID);
            }
            buffer.Dispose();
        }

        private static void Packet_PlayerLook(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            Player player = GameManager.playerList[connectionID];
            Quaternion lastLook = new Quaternion(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
            Quaternion look = new Quaternion(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
            MovementManager.PlayerLooking(player, lastLook, look);
            buffer.Dispose();
        }

        private static void Packet_PlayerMove(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            Player player = GameManager.playerList[connectionID];
            Vector3 lastPosition = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
            Vector3 position = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
            Console.WriteLine("move: " + connectionID + " POS: " + position + " LAST: " + lastPosition);
            MovementManager.PlayerMovement(player, lastPosition, position);
            buffer.Dispose();
        }

        private static void Packet_PlayerLogin(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            string email = buffer.ReadString();
            bool canLogin = GameManager.CanPlayerLogin(email, buffer.ReadString());
            NetworkSend.SendPlayerLoginResult(connectionID, canLogin);
            buffer.Dispose();
            if (canLogin)
            {
                string username = "";
                MySqlDataReader reader = Program.database.GetData("SELECT username FROM Users WHERE EMAIL='" + email + "'");
                if (reader.HasRows)
                {
                    reader.Read();
                    username = reader.GetString("username");
                }
                Player player = GameManager.TryToLoadPlayer(connectionID, username, email);
                GameManager.playerList.Add(connectionID, player);
                NetworkSend.InstantiateNetworkPlayer(connectionID, player);
                reader.Close();
            }
            else
            {
                MySqlDataReader reader = Program.database.GetData("SELECT * FROM Users WHERE EMAIL='" + email + "'");
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader.GetBoolean("baned"))
                    {
                        string reason = reader.GetString("banreason");
                        long bantime = reader.GetInt64("bantime");
                        NetworkSend.SendPlayerBanData(connectionID, reason, bantime);
                    }
                }
                reader.Close();
            }
        }

    }
}
