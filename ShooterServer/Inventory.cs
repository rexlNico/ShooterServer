using System;
using System.Collections.Generic;
using System.Text;

namespace ShooterServer
{
    public class Inventory
    {
        private int limit = -1;
        private Dictionary<int, Item> inventroy = new Dictionary<int, Item>();

        public bool HasSlotItem(int slot)
        {
            if (slot == -1) return false;
            return inventroy.ContainsKey(slot);
        }

        public bool HasItem(Item item)
        {
            return inventroy.ContainsValue(item);
        }

        public Item GetItem(int slot)
        {
            return inventroy[slot];
        }

        public int GetSlotOfItem(Item item)
        {
            if (inventroy.ContainsValue(item))
            {
                int slot = 0;
                bool search = true;
                while (search)
                {
                    if (HasSlotItem(slot) && inventroy[slot] == item)
                    {
                        search = false;
                        return slot;
                    }
                }
            }
            return -1;
        }

        public bool SwitchItemsFromSlots(int slot1, int slot2)
        {
            if(HasSlotItem(slot1) && HasSlotItem(slot2))
            {
                Item i1 = GetItem(slot1);
                Item i2 = GetItem(slot2);
                inventroy.Remove(slot1);
                inventroy.Remove(slot2);
                inventroy.Add(slot1, i2);
                inventroy.Add(slot2, i1);
                return true;
            }
            return false;
        }

        public bool SwitchItemsFromItems(Item item1, Item item2)
        {
            int slot1 = GetSlotOfItem(item1);
            int slot2 = GetSlotOfItem(item2);
            return SwitchItemsFromSlots(slot1, slot2);
        }

        public int GetNextEmptySlot()
        {
            int slot = 0;
            bool search = true;
            while (search)
            {
                if (!HasSlotItem(slot))
                {
                    search = false;
                    return slot;
                }
                slot++;
                if(limit != -1)
                {
                    if(slot > limit)
                    {
                            search = false;
                            return -1;
                    }
                }
            }
            return slot;
        }

        public bool IsInventoryFull()
        {
            return GetNextEmptySlot() == -1;
        }

        public bool RemoveItem(Item item)
        {
            if (HasItem(item))
            {
                int slot = GetSlotOfItem(item);
                inventroy.Remove(slot);
                return true;
            }
            return false;
        }
        public bool AddItem(Item item)
        {
            int slot = GetNextEmptySlot();
            if (slot == -1) return false;
            inventroy.Add(slot, item);
            return true;
        }

    }



}
