using System;
using System.Collections.Generic;
using System.Text;

namespace ShooterServer
{

    public class BulletManager
    {
        public void AddDamage(Player player, int bulletID)
        {
            int damage = GetBulleDamage(bulletID);
            //player.DamagePlayer(damage);
            //NetworkSend.SendPlayerHealthChange(player.connectionID, player.health, player.maxHealth);
        }

        public int GetBulleDamage(int bulletID)
        {
            switch (bulletID)
            {

                case 1:
                    return 20;

            }
            return 0;
        }

    }
}
