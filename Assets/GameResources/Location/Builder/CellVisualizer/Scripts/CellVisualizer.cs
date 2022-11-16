using UnityEngine;

namespace GameResources.Location.Builder.CellVisualizer.Scripts
{
    public class CellVisualizer : MonoBehaviour
    {
        public Vector2Int Index { get; private set; }

        public VisualizerState State { get; private set; }

        [SerializeField]
        private GameObject cell;

        [SerializeField]
        private float fadeHeight = 0.05f;

        [SerializeField]
        private Renderer cellRenderer;
        
        [SerializeField]
        private Color free;
        
        [SerializeField]
        private Color occupied;

        [SerializeField]
        private Color fadeOut;
        
        private Vector3 _localPosition = Vector3.zero;

        private Color _cellColor;
        
        public void Init(Vector2Int index, bool isOccupied)
        {
            _localPosition.y = -fadeHeight;
            cell.transform.localPosition = _localPosition;

            State = VisualizerState.FadeIn;
            
            Index = index;
            
            _cellColor = isOccupied ? occupied : free;
            cellRenderer.material.color = _cellColor;
        }

        public void Move(float delta)
        {
            if (State == VisualizerState.Stay)
            {
                return;
            }

            if (State == VisualizerState.FadeOut)
            {
                delta = -delta;
            }
            
            _localPosition.y += delta;

            if (_localPosition.y <= -fadeHeight || _localPosition.y >= fadeHeight)
            {
                _localPosition.y = Mathf.Clamp(_localPosition.y,-fadeHeight, fadeHeight);

                State = VisualizerState.Stay;
            }

            var t = (_localPosition.y + fadeHeight) / (fadeHeight * 2);
            cellRenderer.material.color = Color.Lerp(fadeOut,  _cellColor, t);
            
            cell.transform.localPosition = _localPosition;
        }

        public void SetState(VisualizerState state)
        {
            State = state;
        }
        
        public enum VisualizerState
        {
            FadeIn,
            FadeOut,
            Stay
        }
    }
}
