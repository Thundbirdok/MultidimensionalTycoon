using System.Linq;
using UnityEngine;

namespace GameResources.UI.Scripts
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;
        
        private Transform _target;
        
        private void Awake()
        {
            var currentCamera = Camera.allCameras.FirstOrDefault(x => x.name == "UI");

            _target = currentCamera.transform;
            canvas.worldCamera = currentCamera;
        }

        private void Update() => transform.rotation = Quaternion.LookRotation(_target.forward, Vector3.up);
    }
}
