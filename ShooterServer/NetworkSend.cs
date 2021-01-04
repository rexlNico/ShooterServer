using System;
using System.Collections.Generic;
using KaymakNetwork;
using System.Numerics;

namespace ShooterServer
{

    enum ServerPackets
    {
        SWelcomeMessage = 1,
        SInstantiatePlayer,
        SPlayerMove,
        SPlayerRotate,
        SPlayerRemove,
    }

    internal static class NetworkSend
    {
        public static void WelomeMessage(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SWelcomeMessage);
            buffer.WriteInt32(connectionID);
            buffer.WriteString(msg);
            NetworkConfig.socket.SendDataTo(connectionID, buffer.Data, buffer.Head);
            buffer.Dispose();

        }

        public static void DeletePlayer(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerRemove);
            buffer.WriteInt32(connectionID);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();

        }

        private static ByteBuffer PlayerData(int connectionID, Player player)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SInstantiatePlayer);
            buffer.WriteInt32(connectionID);
            return buffer;
        }

        public static void SendPlayerRotation(int connectionID, Vector2 rotations)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerRotate);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(rotations.X);
            buffer.WriteSingle(rotations.Y);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);

            buffer.Dispose();
        }

        public static void SendPlayerMove(int connectionID, Vector3 movement)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerMove);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(movement.X);
            buffer.WriteSingle(movement.Y);
            buffer.WriteSingle(movement.Z);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);

            buffer.Dispose();
        }

        public static void InstantiateNetworkPlayer(int connectionID, Player player)
        {
            for (int i = 1; i <= GameManager.playerList.Count; i++)
            {
                if(GameManager.playerList[i] != null)
                {
                    if (GameManager.playerList[i].inGame)
                    {
                        if(i != connectionID)
                        {
                            NetworkConfig.socket.SendDataTo(connectionID, PlayerData(i, player).Data, PlayerData(i, player).Head);
                        }
                    }
                }
            }
            NetworkConfig.socket.SendDataToAll(PlayerData(connectionID, player).Data, PlayerData(connectionID, player).Head);
        }

    }
}
