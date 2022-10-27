using System;
using UnityEngine;

namespace GameResources.Economy
{
    [Serializable]
    public class ResourceValue
    {
        [SerializeReference]
        private IResource type;
        public IResource Type => type;

        [SerializeField]
        private uint value;

        public uint Value => value;

        public ResourceValue(IResource type, uint value)
        {
            this.type = type;
            this.value = value;
        }
    }
}
