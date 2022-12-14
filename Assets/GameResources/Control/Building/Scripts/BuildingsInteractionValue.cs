using System;
using GameResources.Control.ResourceObjects.Scripts;
using UnityEngine;

namespace GameResources.Control.Building.Scripts
{
    [Serializable]
    public sealed class BuildingsInteractionValue
    {
        [field: SerializeField]
        public ResourceObjectData Data { get; private set; }

        [field: SerializeField]
        public int Value { get; private set; }
    }
}
