using System;
using UnityEngine;

namespace GameResources.Location.Scripts
{
    public sealed class Island : MonoBehaviour
    {
        [SerializeField] 
        private string key;

        public string Key => key;

        [SerializeField]
        private GridProvider[] grids;
        
        
    }
}
