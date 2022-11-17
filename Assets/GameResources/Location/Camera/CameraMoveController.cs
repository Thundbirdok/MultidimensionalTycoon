using Cinemachine;
using UnityEngine;

namespace GameResources.Location.Camera
{
    public class CameraMoveController : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField]
        private float moveSpeed = 0.5f;
        
        [SerializeField]
        private float scrollSpeed = 5f;
        
        [SerializeField]
        private float minZoom = 10f;
        
        [SerializeField]
        private float maxZoom = 25f;
        
        [SerializeField]
        private Transform target;

        private CinemachineFramingTransposer _transposer;
        
        private Vector2 _input;

        private float _scrollInput;

        private void Awake()
        {
            _transposer = virtualCamera.GetComponentInChildren<CinemachineFramingTransposer>();

            if (_transposer != null)
            {
                return;
            }

            Debug.LogError("Don't get transposer on " + gameObject.name);

            enabled = false;
        }

        private void Update()
        {
            _input.x = Input.GetAxis("Horizontal");
            _input.y = Input.GetAxis("Vertical");

            _scrollInput = Input.GetAxis("Mouse ScrollWheel");
        }

        private void LateUpdate()
        {
            var position = target.position;
            
            transform.RotateAround(position, Vector3.up, _input.x * moveSpeed);
            
            transform.RotateAround(position, Vector3.right, _input.y * moveSpeed);

            var distance = _transposer.m_CameraDistance;

            distance = Mathf.Clamp(distance - _scrollInput * scrollSpeed, minZoom, maxZoom);
            
            _transposer.m_CameraDistance = distance;
        }
    }
}
