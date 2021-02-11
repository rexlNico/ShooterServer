using System.Numerics;

namespace ShooterServer
{
    class MovementManager
    {

        public static void PlayerMovement(Player player, Vector3 lastPos, Vector3 pos)
        {
            //MOVEMENT CHECK

            player.location = pos;
            NetworkSend.SendPlayerPosition(player.connectionID, pos);
        }

        public static void PlayerLooking(Player player, Quaternion lastLooking, Quaternion looking)
        {
            //LOOKING CHECK

            player.looking = looking;
            NetworkSend.SendPlayerLook(player.connectionID, looking);
        }

    }
}
