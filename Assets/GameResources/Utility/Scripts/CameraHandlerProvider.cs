using UnityEngine;

namespace GameResources.Utility.Scripts
{
    public sealed class CameraHandlerProvider : MonoBehaviour
    {
        [SerializeField]
        private Camera handledCamera;
        
        [SerializeField]
        private CameraHandler cameraHandler;

        private void Awake()
        {
            cameraHandler.Construct(handledCamera);
        }
    }
}
