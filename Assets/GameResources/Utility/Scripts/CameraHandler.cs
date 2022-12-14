using UnityEngine;

namespace GameResources.Utility.Scripts
{
    [CreateAssetMenu(fileName = "CameraHandler", menuName = "CrossSceneInteraction/CamearaHandler")]
    public sealed class CameraHandler : ScriptableObject
    {
        public Camera HandledCamera { get; private set; }

        public void Construct(Camera handledCamera)
        {
            HandledCamera = handledCamera;
        }
    }
}
