using UnityEngine;

namespace GameResources.Location.Island.Scripts
{
    public sealed class Island : MonoBehaviour
    {
        [SerializeField] 
        private string key;

        public string Key => key;

        [SerializeField]
        private LocationGridProvider[] grids;
        
        
    }
}
