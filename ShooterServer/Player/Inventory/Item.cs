using System;
using System.Collections.Generic;
using System.Text;

namespace ShooterServer
{

    public enum ItemType
    {
        TESTITEM = 1,
    }

    public class Item
    {
        public ItemType type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public long amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }
        public long damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }


    }
}
