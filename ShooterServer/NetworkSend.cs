﻿using System;
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
        SPlayerMove,
        SPlayerLook
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


        public static void SendPlayerPosition(int connectionID, Vector3 position)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerMove);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(position.X);
            buffer.WriteSingle(position.Y);
            buffer.WriteSingle(position.Z);
            NetworkConfig.socket.SendDataToAll(buffer.Data, buffer.Head);
            buffer.Dispose();
        }

        public static void SendPlayerLook(int connectionID, Quaternion looking)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteInt32((int)ServerPackets.SPlayerLook);
            buffer.WriteInt32(connectionID);
            buffer.WriteSingle(looking.X);
            buffer.WriteSingle(looking.Y);
            buffer.WriteSingle(looking.Z);
            buffer.WriteSingle(looking.W);
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
            buffer.WriteSingle(player.location.X);
            buffer.WriteSingle(player.location.Y);
            buffer.WriteSingle(player.location.Z);
            buffer.WriteSingle(player.looking.X);
            buffer.WriteSingle(player.looking.Y);
            buffer.WriteSingle(player.looking.Z);
            buffer.WriteSingle(player.looking.W);
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
