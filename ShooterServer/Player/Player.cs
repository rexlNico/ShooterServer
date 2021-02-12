using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace ShooterServer
{
    public class Player
    {

        public long lastPing;

        public int connectionID;
        public string username;
        public string emai;

        //Locations
        public Vector3 location;
        public Quaternion looking;

    }

}
