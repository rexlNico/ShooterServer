using System;
using System.Collections.Generic;
using System.Text;
using KaymakNetwork;
using System.Numerics;

namespace ShooterServer
{
    enum ClientPackets
    {
        CPlayerLocation = 1,
        CPlayerMoveUpdate,
    }

    internal static class NetworkReceive
    {

        internal static void PacketRouter()
        {
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerLocation] = Packet_PlayerLocation;
            NetworkConfig.socket.PacketId[(int)ClientPackets.CPlayerMoveUpdate] = Packet_PlayerMoveUpdate;
        }

        private static void Packet_PlayerMoveUpdate(int connectionID, ref byte[] data)
        {
            Player player = GameManager.playerList[connectionID];
            ByteBuffer buffer = new ByteBuffer(data);
            float deltaTime = buffer.ReadSingle();
            bool isGrounded = buffer.ReadBoolean();
            bool hasJumped = buffer.ReadBoolean();
            Vector2 movement = new Vector2(buffer.ReadSingle(), buffer.ReadSingle());
            Vector3 forward = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
            Vector3 right = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
            Vector2 rotation = new Vector2(buffer.ReadSingle(), buffer.ReadSingle());

            Vector3 move = right * movement.X + forward * movement.Y;

            InputManager.TryToMove(connectionID, move * player.movementSpeed, deltaTime);
            InputManager.TryToRotate(connectionID, rotation, deltaTime);
            player.Update(deltaTime, isGrounded, hasJumped);

            buffer.Dispose();
        }

        private static void Packet_PlayerLocation(int connectionID, ref byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(data);
            Vector3 position = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
            Vector3 rotationPlayer = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
            Vector3 rotationCamera = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());

            Player player = GameManager.playerList[connectionID];
            player.position = position;
            player.rotationCamera = rotationCamera;
            player.rotationPlayer = rotationPlayer;

            buffer.Dispose();
        }

    }
}
