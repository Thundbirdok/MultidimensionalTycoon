using System;
using System.Threading.Tasks;
using Cinemachine;
using GameResources.Inputs;
using GameResources.Location.Island.Scripts;
using Lean.Touch;
using UnityEngine;

namespace GameResources.Location.Camera
{
    public class CameraInputProvider : MonoBehaviour, AxisState.IInputAxisProvider
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;
        
        [SerializeField]
        private float zoomSpeed = 5f;
        
        [SerializeField]
        private float minZoom = 10f;
        
        [SerializeField]
        private float maxZoom = 25f;

        [SerializeField]
        private DragInput drag;

        [SerializeField]
        private PinchInput pinch;
        
        [SerializeField]
        private float returnToOldPositionDuration = 0.25f;
        
        [SerializeField]
        private IslandRotator rotator;
        
        private CinemachineFramingTransposer _transposer;
        private CinemachinePOV _composer;
        
        private Vector2 _moveInput;

        private float _zoomInput;
        
        private float _oldPositionX;
        private float _oldPositionY;
        
        private void Awake()
        {
            _transposer = virtualCamera.GetComponentInChildren<CinemachineFramingTransposer>();
            _composer = virtualCamera.GetComponentInChildren<CinemachinePOV>();
            
            if (_transposer == null)
            {
                Debug.LogError("Don't get transposer on " + gameObject.name);

                enabled = false;
            }

            if (_composer == null)
            {
                Debug.LogError("Don't get composer on " + gameObject.name);

                enabled = false;
            }
        }

        private void OnEnable()
        {
            drag.OnUpdateInput += SetMoveInput;
            pinch.OnUpdateInput += SetZoomInput;
            
            LeanTouch.OnFingerDown += SetOldPosition;
            
            rotator.OnRotate += ReturnOnOldPosition;
        }

        private void OnDisable()
        {
            drag.OnUpdateInput -= SetMoveInput;
            pinch.OnUpdateInput -= SetZoomInput;
            
            LeanTouch.OnFingerDown -= SetOldPosition;
            
            rotator.OnRotate -= ReturnOnOldPosition;
        }

        private void Update()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                _zoomInput = scroll;
            }
        }
        
        private void LateUpdate() => SetZoom();

        public float GetAxisValue(int axis)
        {
            if (enabled == false)
            {
                return 0;
            }

            return axis switch
            {
                0 => _moveInput.x,
                1 => _moveInput.y,
                _ => 0
            };
        }

        private void SetMoveInput() => _moveInput = drag.DeltaInput;

        private void SetZoomInput() => _zoomInput = pinch.DeltaInput;


        private void SetZoom()
        {
            var distance = _transposer.m_CameraDistance;

            distance = Mathf.Clamp
                (
                    distance - _zoomInput * zoomSpeed * Time.deltaTime,
                    minZoom,
                    maxZoom
                );

            _transposer.m_CameraDistance = distance;
        }

        private void SetOldPosition(LeanFinger finger)
        {
            _oldPositionX = _composer.m_HorizontalAxis.Value;
            _oldPositionY = _composer.m_VerticalAxis.Value;
        }
        
        private async void ReturnOnOldPosition()
        {
            var distanceX = Math.Abs(_composer.m_HorizontalAxis.Value - _oldPositionX);
            var distanceY = Math.Abs(_composer.m_VerticalAxis.Value - _oldPositionY);
            
            var speedX = distanceX / returnToOldPositionDuration;
            var speedY = distanceY / returnToOldPositionDuration;
            
            while (Math.Abs(_composer.m_HorizontalAxis.Value - _oldPositionX) > 0.01f &&
                    Math.Abs(_composer.m_VerticalAxis.Value - _oldPositionY) > 0.01f)
            {
                _composer.m_HorizontalAxis.Value 
                    = Mathf.MoveTowardsAngle
                    (
                    _composer.m_HorizontalAxis.Value, 
                    _oldPositionX, 
                    speedX * Time.deltaTime
                    );
                
                _composer.m_VerticalAxis.Value 
                    = Mathf.MoveTowardsAngle
                    (
                    _composer.m_VerticalAxis.Value, 
                    _oldPositionY, 
                    speedY * Time.deltaTime
                    );

                await Task.Yield();
            }
        }
    }
}
