using Cinemachine;
using UnityEngine;

namespace GameResources.Location.Camera
{
    public class CameraZoomController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;
        
        [SerializeField]
        private float scrollSpeed = 5f;
        
        [SerializeField]
        private float minZoom = 10f;
        
        [SerializeField]
        private float maxZoom = 25f;

        private CinemachineFramingTransposer _transposer;
        private CinemachinePOV _composer;
        
        private Vector2 _input;

        private float _scrollInput;

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

        private void Update()
        {
            _scrollInput = Input.GetAxis("Mouse ScrollWheel");
        }

        private void LateUpdate()
        {
            SetZoom();

            _composer.m_HorizontalAxis.Value = 0;
            _composer.m_VerticalAxis.Value = 0;
        }

        private void SetZoom()
        {
            var distance = _transposer.m_CameraDistance;

            distance = Mathf.Clamp(distance - _scrollInput * scrollSpeed, minZoom, maxZoom);

            _transposer.m_CameraDistance = distance;
        }
    }
}
