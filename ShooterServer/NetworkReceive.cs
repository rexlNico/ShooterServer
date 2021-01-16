using KaymakNetwork;
using MySql.Data.MySqlClient;

namespace ShooterServer
{
    enum ClientPackets
    {
        CPlayerLogin = 1,
        CPlayerMove,
        CPlayerLook
    }

    internal static class NetworkReceive
    {

        internal static void PacketRouter()
        {
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerLogin] = Packet_PlayerLogin;
        }

        private static void Packet_PlayerLogin(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            string email = buffer.ReadString();
            System.Console.WriteLine("Login try from " + connectionID + " " + email);
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
                    reader.Close();
                }
                Player player = GameManager.TryToLoadPlayer(connectionID, username, email);
                GameManager.playerList.Add(connectionID, player);
                NetworkSend.InstantiateNetworkPlayer(connectionID, player);
            }
        }

    }
}
