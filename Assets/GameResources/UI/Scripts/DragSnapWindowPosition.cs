using Lean.Gui;
using UnityEngine;

namespace GameResources.UI.Scripts
{
    public class DragSnapWindowPosition : MonoBehaviour
    {
        [SerializeField]
        private RectTransform window;

        [SerializeField]
        private LeanSnap snap;

        [SerializeField]
        private float left = -400;
        
        [SerializeField]
        private float right = 0;
        
        private bool _isGoRightOnClick = true;
        
        private void OnEnable()
        {
            snap.OnPositionChanged.AddListener(SetPositionToGo);
        }

        private void OnDisable()
        {
            snap.OnPositionChanged.RemoveListener(SetPositionToGo);
        }
        
        private void SetPositionToGo(Vector2Int snapPosition)
        {
            _isGoRightOnClick = snapPosition.x < 0;
        }

        public void SnapPosition()
        {
            var position = new Vector3(0, window.position.y, 0);

            position.x = (left + right) / 2;
            
            position.x += _isGoRightOnClick ? 1 : -1;

            window.anchoredPosition = position;
        }
    }
}
