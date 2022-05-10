using System;

namespace Project
{
    public class Reward
    {
        public static event Action<Reward> Received = delegate { };

        public ItemType Type
        {
            get;
            private set;
        }

        public int Count
        {
            get;
            private set;
        }

        public Reward(ItemType type, int count)
        {
            Type = type;
            Count = count;

            Received(this);
        }
    }
}