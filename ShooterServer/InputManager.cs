using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ShooterServer
{
    public class InputManager
    {
        public static void TryToMove(int connectionID, Vector3 movement, float deltaTime)
        {
            NetworkSend.SendPlayerMove(connectionID, movement * deltaTime);
        }

        public static void TryToRotate(int connectionID, Vector2 rotations, float deltaTime)
        {
            Player player = GameManager.playerList[connectionID];
            float mouseX = rotations.X * 300f;
            float mouseY = rotations.Y * 300f;

            player.xRotation -= mouseY;
            player.xRotation = Math.Clamp(player.xRotation, -90f, 90f);

            NetworkSend.SendPlayerRotation(connectionID, new Vector2(mouseX, player.xRotation) * deltaTime);
        }

    }
}
