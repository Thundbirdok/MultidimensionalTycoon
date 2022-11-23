using System;
using Lean.Touch;
using UnityEngine;

namespace GameResources.Inputs
{
    public class PinchInput : MonoBehaviour
    {
        public event Action OnUpdateInput;

        public float DeltaInput { get; private set; } 
        
        [SerializeField]
        private LeanFingerFilter use = new LeanFingerFilter(true);

        private void Update()
        {
            var fingers = use.UpdateAndGetFingers();
            
            var pinchScale = LeanGesture.GetPinchScale(fingers);

            DeltaInput = Mathf.Clamp(pinchScale - 1, -1, 1);;

            OnUpdateInput?.Invoke();
        }
    }
}
