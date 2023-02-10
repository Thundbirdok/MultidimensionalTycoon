using System;
using GameResources.Control.Economy.Resources.Scripts;
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
        public Resource Value { get; private set; }

        public BuildingsInteractionValue(ResourceObjectData data, Resource value)
        {
            Data = data;
            Value = value;
        }
    }
}
