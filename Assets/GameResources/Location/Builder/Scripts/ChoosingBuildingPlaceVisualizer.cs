using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Builder.Scripts
{
    public class ChoosingBuildingPlaceVisualizer : MonoBehaviour
    {
        [SerializeField]
        private CellPointer cellPointer;

        [SerializeField]
        private AssetReference buildingReference;

        private GameObject _building;

        private Vector3 _targetPosition;

        private Coroutine _coroutine;
        
        private float _duration = 0.5f;

        private bool _isTweening;
        
        private async void OnEnable()
        {
            _building = await buildingReference.InstantiateAsync().Task;
            _building.SetActive(false);

            _coroutine = StartCoroutine(TweenPosition());
            
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);

                _coroutine = null;
            }
            
            if (_building == null)
            {
                return;
            }

            _building.SetActive(false);
            buildingReference.ReleaseInstance(_building);
            _building = null;
        }

        private void Subscribe()
        {
            cellPointer.OnCellPointed += OnCellPointed;
            cellPointer.OnNoCellPointed += OnNoCellPointed;
        }

        private void Unsubscribe()
        {
            cellPointer.OnCellPointed -= OnCellPointed;
            cellPointer.OnNoCellPointed -= OnNoCellPointed;
        }

        private void OnCellPointed()
        {
            var cell = cellPointer.PointedCell;
            var gridTransform = cellPointer.PointedGrid.transform;

            var localPosition = cell.GetPosition();
            var position = gridTransform.transform.TransformPoint(localPosition);

            if (_building.transform.parent != gridTransform || _building.activeSelf == false)
            {
                _isTweening = false;
                
                _building.transform.parent = gridTransform;
                _building.transform.position = position;
                _building.transform.rotation = gridTransform.rotation;
            }
            else
            {
                _targetPosition = position;
                
                _isTweening = true;
            }

            _building.SetActive(true);
        }

        private void OnNoCellPointed()
        {
            _isTweening = false;
            
            _building.SetActive(false);
        }

        private IEnumerator TweenPosition()
        {
            var waitTween = new WaitUntil(() => _isTweening);
            
            while (true)
            {
                yield return waitTween;
                
                var t = _duration;

                while (t > 0)
                {
                    if (_isTweening == false)
                    {
                        _building.transform.position = _targetPosition;

                        break;
                    }
                    
                    t -= Time.deltaTime;

                    _building.transform.position = Vector3.Lerp
                        (_targetPosition, _building.transform.position, t / _duration);

                    yield return null;
                }
            }
        }
    }
}
