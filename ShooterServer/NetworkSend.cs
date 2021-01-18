using System;
using System.Collections.Generic;
using KaymakNetwork;
using System.Numerics;

namespace ShooterServer
{

    enum ServerPackets
    {
        SPlayerLogin = 1,
        SPlayerBaned,
        SPlayerInstantiate,
        SPlayerPositon,
        SPlayerForce3D,
        SPlayerForce2D,
        SPlayerLook,
        SPlayerVelocity,
        SPlayerScale
    }

    internal static class NetworkSend
    {
        public static void SendPlayerLoginResult(int connectionID, bool canLogin)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerLogin);
            buffer.WriteInt32(connectionID);
            buffer.WriteBoolean(canLogin);
            NetworkConfig.socket.SendDataTo(connectionID, buffer.Data, buffer.Head);
            buffer.Dispose();

        }

        public static void SendPlayerForce(int connectionID, Vector3 force)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerForce3D);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(force.X);
            buffer.WriteSingle(force.Y);
            buffer.WriteSingle(force.Z);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
        }

        public static void SendPlayerLook(int connectionID, float xRotation, float desiredX)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerLook);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(xRotation);
            buffer.WriteSingle(desiredX);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
        }

        public static void SendVelocity(int connectionID, Vector3 velocity)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerVelocity);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(velocity.X);
            buffer.WriteSingle(velocity.Y);
            buffer.WriteSingle(velocity.Z);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
        }

        public static void SendPlayerForce(int connectionID, Vector2 force)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerForce2D);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(force.X);
            buffer.WriteSingle(force.Y);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
        }

        public static void SendPlayerPosition(int connectionID, Vector3 position)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerPositon);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(position.X);
            buffer.WriteSingle(position.Y);
            buffer.WriteSingle(position.Z);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
        }

        public static void SendPlayerScale(int connectionID, Vector3 scale)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerScale);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(scale.X);
            buffer.WriteSingle(scale.Y);
            buffer.WriteSingle(scale.Z);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
        }

        public static void SendPlayerBanData(int connectionID, string banreason, string bantime)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerLogin);
            //buffer.WriteBoolean(canLogin);
            NetworkConfig.socket.SendDataTo(connectionID, buffer.Data, buffer.Head);
            buffer.Dispose();

        }

        private static ByteBuffer PlayerData(int connectionID, Player player)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerInstantiate);
            buffer.WriteInt32(connectionID);
            buffer.WriteString(player.username);
            buffer.WriteSingle(player.inputManager.position.X);
            buffer.WriteSingle(player.inputManager.position.Y);
            buffer.WriteSingle(player.inputManager.position.Z);
            buffer.WriteSingle(0);
            buffer.WriteSingle(0);
            buffer.WriteSingle(0);
            buffer.WriteSingle(0);
            return buffer;
        }


        public static void InstantiateNetworkPlayer(int connectionID, Player player)
        {
            foreach (var item in GameManager.playerList)
            {
                if (item.Key == connectionID)
                {
                    NetworkConfig.socket.SendDataToAll(PlayerData(connectionID, player).Data, PlayerData(connectionID, player).Head);
                }
                else
                {
                    NetworkConfig.socket.SendDataTo(connectionID, PlayerData(item.Key, player).Data, PlayerData(item.Key, player).Head);
                }
            }
        }
    }
}
