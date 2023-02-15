using System;
using UnityEngine;

namespace GameResources.Control.Economy.Resources.Scripts
{
    [Serializable]
    public class Resource
    {
        [field: SerializeField]
        public int Value { get; set; }

        [field: SerializeField]
        public IResourceType Type { get; private set; }

        public Resource(IResourceType type, int value)
        {
            Type = type;
            Value = value;
        }

        public static Resource operator +(Resource a, Resource b)
        {
            if (a.Type.Equals(b.Type) == false)
            {
                throw new Exception("Different types can not be summed");
            }

            return new Resource(a.Type, a.Value + b.Value);
        } 
    }
}
