using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ShooterServer
{
    public class Player
    {
        public int connectionID;
        public string username;
        public bool inGame;

        public float movementSpeed = 12f;
        public float jumpHight = 6f;
        public float xRotation = 0;
        public Vector3 velocity;
        public bool isGrounded;

        public Vector3 position;
        public float rotation;
        public Vector3 rotationPlayer;
        public Vector3 rotationCamera;

        public void Update(float deltatime, bool isGrounded, bool hasJumped)
        {
            this.isGrounded = isGrounded;
            if (isGrounded && velocity.Y < 0f)
            {
                velocity.Y = -2f;
            }
            if (hasJumped)
            {
                velocity.Y = (float)Math.Sqrt(jumpHight * -2f * GameManager.gravity);
            }
            velocity.Y += GameManager.gravity * deltatime;
            NetworkSend.SendPlayerMove(connectionID, velocity * deltatime);
        }

    }

}
