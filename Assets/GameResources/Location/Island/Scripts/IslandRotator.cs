using System;
using GameResources.Location.Scripts;
using Lean.Touch;
using System.Linq;
using UnityEngine;

namespace GameResources.Location.Island.Scripts
{
    public sealed class IslandRotator : MonoBehaviour
    {
        public event Action OnRotate; 
        
        [SerializeField]
        private LeanFingerSwipe north;
        
        [SerializeField]
        private LeanFingerSwipe east;
        
        [SerializeField]
        private LeanFingerSwipe south;
        
        [SerializeField]
        private LeanFingerSwipe west;

        [SerializeField]
        private Rotator rotator;

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
            Rotate(new Vector2Int(0, 1));
        }
        
        private void RotateEast(LeanFinger finger)
        {
            Rotate(new Vector2Int(1, 0));
        }
        
        private void RotateSouth(LeanFinger finger)
        {
            Rotate(new Vector2Int(0, -1));
        }
        
        private void RotateWest(LeanFinger finger)
        {
            Rotate(new Vector2Int(-1, 0));
        }

        private void Rotate(Vector2Int direction)
        {
            if (LeanTouch.Fingers.Count(x => x.Index != -42) > 1)
            {
                return;
            }
            
            OnRotate?.Invoke();
            
            rotator.Rotate(direction);
        }
    }
}
