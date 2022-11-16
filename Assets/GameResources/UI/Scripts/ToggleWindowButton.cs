using Lean.Gui;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameResources.UI.Scripts
{
    public class ToggleWindowButton : MonoBehaviour, 
        IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField]
        private DragSnapWindowPosition windowPositionController;
        
        [SerializeField]
        private LeanDrag drag;

        private bool _isDraging;

        public void OnDrag(PointerEventData eventData) => drag.OnDrag(eventData);

        public void OnBeginDrag(PointerEventData eventData)
        {
            drag.OnBeginDrag(eventData);
            
            _isDraging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            drag.OnEndDrag(eventData);

            _isDraging = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isDraging)
            {
                return;
            }

            windowPositionController.SnapPosition();
        }
    }
}
