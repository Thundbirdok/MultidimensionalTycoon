using GameResources.Location.Scripts;
using Lean.Touch;
using UnityEngine;

namespace GameResources.Location.Island.Scripts
{
    public sealed class IslandRotator : MonoBehaviour
    {
        [SerializeField]
        private LeanFingerSwipe north;
        
        [SerializeField]
        private LeanFingerSwipe east;
        
        [SerializeField]
        private LeanFingerSwipe south;
        
        [SerializeField]
        private LeanFingerSwipe west;

        [SerializeField]
        private Rotator _rotator;

        private void OnEnable()
        {
            north.onFinger.AddListener(RotateNorth);
            east.onFinger.AddListener(RotateEast);
            south.onFinger.AddListener(RotateSouth);
            west.onFinger.AddListener(RotateWest);
        }

        private void OnDisable()
        {
            north.onFinger.RemoveListener(RotateNorth);
            east.onFinger.RemoveListener(RotateEast);
            south.onFinger.RemoveListener(RotateSouth);
            west.onFinger.RemoveListener(RotateWest);
        }

        private void RotateNorth(LeanFinger finger)
        {
            _rotator.Rotate(new Vector2Int(0, 1));
        }
        
        private void RotateEast(LeanFinger finger)
        {
            _rotator.Rotate(new Vector2Int(1, 0));
        }
        
        private void RotateSouth(LeanFinger finger)
        {
            _rotator.Rotate(new Vector2Int(0, -1));
        }
        
        private void RotateWest(LeanFinger finger)
        {
            _rotator.Rotate(new Vector2Int(-1, 0));
        }
    }
}
