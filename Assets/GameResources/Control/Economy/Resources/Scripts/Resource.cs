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
    }
}
