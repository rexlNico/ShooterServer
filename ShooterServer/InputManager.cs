using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Threading;

namespace ShooterServer
{
    public class InputManager
    {

        private int connectionID;
        private float deltaTime;

        //Rotation and look
        private float xRotation;
        private float sensitivity = 50f;
        private float sensMultiplier = 1f;

        //Movement
        public float moveSpeed = 4500;
        public float maxSpeed = 20;
        private bool grounded;
        public Vector3 position;
        private Vector3 forward;
        private Vector3 right;

        public float counterMovement = 0.175f;
        private float threshold = 0.01f;
        public float maxSlopeAngle = 35f;

        //Crouch & Slide
        private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
        private Vector3 playerScale;
        public float slideForce = 400;
        public float slideCounterMovement = 0.2f;

        //Jumping
        private bool readyToJump = true;
        private float jumpCooldown = 0.25f;
        public float jumpForce = 550f;

        //Input
        float x, y;
        bool jumping, sprinting, crouching;

        //Sliding
        private Vector3 normalVector = new Vector3(0, 1, 0);
        private Vector3 wallNormalVector;

        public void MyInput(int connectionID, float x, float y, bool jumping, bool crouching, bool sprinting, bool contDown, bool contUp, Vector3 position, float magnitude)
        {
            this.connectionID = connectionID;
            this.x = x;
            this.y = y;
            this.jumping = jumping;
            this.crouching = crouching;
            this.sprinting = sprinting;
            this.position = position;

            //Crouching
            if (contDown)
                StartCrouch(magnitude);
            if (contUp)
                StopCrouch();
        }

        private void StartCrouch(float magnitude)
        {
            NetworkSend.SendPlayerScale(connectionID, crouchScale);
            NetworkSend.SendPlayerPosition(connectionID, position);
            this.position = new Vector3(position.X, position.Y - 0.5f, position.Z);
            if (magnitude > 0.5f)
            {
                if (grounded)
                {
                    Vector3 force = forward * slideForce;
                    NetworkSend.SendPlayerForce(connectionID, force);
                }
            }
        }

        private void StopCrouch()
        {
            NetworkSend.SendPlayerScale(connectionID, playerScale);
            position = new Vector3(position.X, position.Y + 0.5f, position.Z);
            NetworkSend.SendPlayerPosition(connectionID, position);
        }

        public void Movement(Vector3 velocity, Vector2 velRelativeToLook, Vector3 velocityNormalized, float deltaTime)
        {
            this.deltaTime = deltaTime;
            //Extra gravity
            Vector3 force = new Vector3(0, -1, 0) * deltaTime * 10;
            NetworkSend.SendPlayerForce(connectionID, force);

            //Find actual velocity relative to where player is looking
            Vector2 mag = velRelativeToLook;
            float xMag = mag.X, yMag = mag.Y;

            //Counteract sliding and sloppy movement
            CounterMovement(x, y, mag, velocityNormalized, velocity);

            //If holding jump && ready to jump, then jump
            if (readyToJump && jumping) Jump(velocity);

            //Set max speed
            float maxSpeed = this.maxSpeed;

            //If sliding down a ramp, add force down so player stays grounded and also builds speed
            if (crouching && grounded && readyToJump)
            {
                Vector3 force2 = new Vector3(0, -1, 0) * deltaTime * 3000;
                NetworkSend.SendPlayerForce(connectionID, force2);
                return;
            }

            //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
            if (x > 0 && xMag > maxSpeed) x = 0;
            if (x < 0 && xMag < -maxSpeed) x = 0;
            if (y > 0 && yMag > maxSpeed) y = 0;
            if (y < 0 && yMag < -maxSpeed) y = 0;

            //Some multipliers
            float multiplier = 1f, multiplierV = 1f;

            // Movement in air

            if (sprinting)
            {
                multiplier = 5f;
                multiplierV = 5f;
            }

            if (!grounded)
            {
                multiplier = 0.3f;
                multiplierV = 0.3f;
            }

            // Movement while sliding
            if (grounded && crouching) multiplierV = 0f;

            //Apply forces to move player
            NetworkSend.SendPlayerForce(connectionID, forward * y * moveSpeed * deltaTime * multiplier * multiplierV);
            NetworkSend.SendPlayerForce(connectionID, right * x * moveSpeed * deltaTime * multiplier);
        }

        private void Jump(Vector3 velocity)
        {
            if (grounded && readyToJump)
            {
                readyToJump = false;

                //Add jump forces
                NetworkSend.SendPlayerForce(connectionID, new Vector2(0, 1) * jumpForce * 1.5f);
                NetworkSend.SendPlayerForce(connectionID, normalVector * jumpForce * 0.5f);

                //If jumping while falling, reset y velocity.
                Vector3 vel = velocity;
                if (velocity.Y < 0.5f)
                    velocity = new Vector3(vel.X, 0, vel.Z);
                else if (velocity.Y > 0)
                    velocity = new Vector3(vel.X, vel.Y / 2, vel.Z);
                //Invoke(nameof(ResetJump), jumpCooldown);
                Thread thread = new Thread(new ThreadStart(ThreadTask));
                thread.Start();
            }
        }

        private void ThreadTask()
        {
            Thread.Sleep(250);
            ResetJump();
            Console.WriteLine("test");
            //Thread.CurrentThread.Abort();
        }

        private void ResetJump()
        {
            readyToJump = true;
        }

        private float desiredX;
        public void Look(float mouseXIn, float mouseYIn, Vector3 roation, float fixedDeltaTime)
        {
            float mouseX = mouseXIn * sensitivity * fixedDeltaTime * sensMultiplier;
            float mouseY = mouseYIn * sensitivity * fixedDeltaTime * sensMultiplier;

            //Find current look rotation
            Vector3 rot = roation;
            desiredX = rot.Y + mouseX;

            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Math.Clamp(xRotation, -90f, 90f);

            //Perform the rotations

            //playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            //orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
            NetworkSend.SendPlayerLook(connectionID, xRotation, desiredX);

        }

        private void CounterMovement(float x, float y, Vector2 mag, Vector3 velocityNormalized, Vector3 velocity)
        {
            if (!grounded || jumping) return;

            //Slow down sliding
            if (crouching)
            {
                NetworkSend.SendPlayerForce(connectionID, moveSpeed * deltaTime * -velocityNormalized * slideCounterMovement);
                return;
            }

            //Counter movement
            if (Math.Abs(mag.X) > threshold && Math.Abs(x) < 0.05f || (mag.X < -threshold && x > 0) || (mag.X > threshold && x < 0))
            {
                NetworkSend.SendPlayerForce(connectionID, moveSpeed * right * deltaTime * -mag.X * counterMovement);
            }
            if (Math.Abs(mag.Y) > threshold && Math.Abs(y) < 0.05f || (mag.Y < -threshold && y > 0) || (mag.Y > threshold && y < 0))
            {
                NetworkSend.SendPlayerForce(connectionID, moveSpeed * forward * deltaTime * -mag.Y * counterMovement);
            }

            //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
            if (Math.Sqrt((Math.Pow(velocity.X, 2) + Math.Pow(velocity.Z, 2))) > maxSpeed)
            {
                float fallspeed = velocity.Y;
                Vector3 n = velocityNormalized * maxSpeed;
                //rb.velocity = new Vector3(n.X, fallspeed, n.Z);
                NetworkSend.SendVelocity(connectionID, new Vector3(n.X, fallspeed, n.Z));
            }
        }

        private bool IsFloor(float angle)
        {
            return angle < maxSlopeAngle;
        }

        private bool cancellingGrounded;
        private void OnCollisionStay(Vector3 normalVector)
        {
            Thread thread = new Thread(new ThreadStart(JumpThreadTask));
            thread.Start();
            grounded = true;
            cancellingGrounded = false;
            this.normalVector = normalVector;
            thread.Abort();
            //Invoke ground/wall cancel, since we can't check normals with CollisionExit
            float delay = 3f;
            if (!cancellingGrounded)
            {
                cancellingGrounded = true;
                thread.Start();
            }
        }

        private void JumpThreadTask()
        {
            Thread.Sleep((int)(deltaTime * 3f * 1000f));
            StopGrounded();
            Thread.CurrentThread.Abort();
        }

        private void StopGrounded()
        {
            grounded = false;
        }

    }
}
