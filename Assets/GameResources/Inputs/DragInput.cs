using System;
using System.Linq;
using Lean.Touch;
using UnityEngine;

namespace GameResources.Inputs
{
    public class DragInput : MonoBehaviour
    {
        public event Action OnUpdateInput;

        public Vector2 DeltaInput { get; private set; }
        
        [SerializeField]
        private LeanFingerFilter use = new LeanFingerFilter(true);

        [SerializeField]
        private float deltaThreshold;

        [SerializeField]
        private float maxSqrMagnitude = 25;

        private bool _isLocked;

        private void OnEnable()
        {
            LeanTouch.OnFingerUp += OnFingerUp;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerUp -= OnFingerUp;
        }
        
        private void Update()
        {
            if (_isLocked)
            {
                return;
            }

            if (InputStates.IsDragActive == false)
            {
                return;
            }
            
            var fingers = use.UpdateAndGetFingers();
            
            var screenScaledDelta = LeanGesture.GetScaledDelta(fingers);

            var delta = screenScaledDelta / deltaThreshold;

            if (delta.sqrMagnitude > maxSqrMagnitude)
            {
                DeltaInput = Vector2.zero;
                
                _isLocked = true;
                
                OnUpdateInput?.Invoke();
                
                return;
            }
            
            DeltaInput = delta.normalized;
            
            OnUpdateInput?.Invoke();
        }

        private void OnFingerUp(LeanFinger finger)
        {
            if (LeanTouch.Fingers.Count(x => x.Down) == 0)
            {
                _isLocked = false;
            }
        }
    }
}
