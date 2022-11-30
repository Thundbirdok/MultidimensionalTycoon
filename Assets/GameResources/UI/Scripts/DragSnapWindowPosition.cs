using GameResources.Control.Scripts;
using GameResources.Location.Builder.Scripts;
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

        [SerializeField]
        private BuilderEventHandler builderEventHandler;
        
        private bool _openOnClick = true;
        
        private void OnEnable()
        {
            snap.OnPositionChanged.AddListener(SetPositionToGo);

            builderEventHandler.OnChooseBuilding += Close;
        }

        private void OnDisable()
        {
            snap.OnPositionChanged.RemoveListener(SetPositionToGo);
            
            builderEventHandler.OnChooseBuilding -= Close;
        }
        
        private void SetPositionToGo(Vector2Int snapPosition)
        {
            _openOnClick = snapPosition.x < 0;
        }

        public void SnapPosition()
        {
            if (_openOnClick)
            {
                Open();
                
                return;
            }

            Close();
        }

        public void Open()
        {
            var position = new Vector3((left + right) / 2 + 1, window.position.y, 0);

            window.anchoredPosition = position;
        }        
        
        public void Close()
        {
            var position = new Vector3((left + right) / 2 - 1, window.position.y, 0);

            window.anchoredPosition = position;
        }

        private void Close(BuildingData data) => Close();
    }
}
