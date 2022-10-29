using System;

namespace GameResources.Economy
{
    public class ResourceValue
    {
        public int Value { get; }

        public IResource Type { get; }

        public ResourceValue(IResource type, int value)
        {
            Type = type;
            Value = value;
        }
    }
}
