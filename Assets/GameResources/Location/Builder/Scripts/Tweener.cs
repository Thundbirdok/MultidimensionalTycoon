using System.Collections;
using UnityEngine;

namespace GameResources.Location.Builder.Scripts
{
    public class Tweener
    {
        public bool IsTweening;
        
        private MonoBehaviour _behaviour;
        
        private GameObject _building;

        private float _tweenDuration;
        
        private float _currentTweenTime;
        
        private Vector3 _targetPosition;

        private Coroutine _coroutine;

        public Tweener(MonoBehaviour behaviour, GameObject building, float tweenDuration)
        {
            _behaviour = behaviour;
            _building = building;
            _tweenDuration = tweenDuration;
            
            _coroutine = _behaviour.StartCoroutine(TweenPosition());
        }

        ~Tweener()
        {
            if (_coroutine == null)
            {
                return;
            }

            _behaviour.StopCoroutine(_coroutine);

            _coroutine = null;
        }
        
        public void SetPosition(Transform gridTransform, Vector3 position)
        {
            if (_building.transform.parent == gridTransform && _building.activeSelf)
            {
                if (_targetPosition != position)
                {
                    _currentTweenTime = _tweenDuration;
                }
                
                _targetPosition = position;
                
                IsTweening = true;

                return;
            }
            
            IsTweening = false;

            _building.transform.parent = gridTransform;
            _building.transform.position = position;
            _building.transform.rotation = gridTransform.rotation;
        }
        
        private IEnumerator TweenPosition()
        {
            var waitTween = new WaitUntil(() => IsTweening);
            
            while (true)
            {
                yield return waitTween;

                _currentTweenTime = _tweenDuration;

                while (_currentTweenTime > 0)
                {
                    if (_building == null)
                    {
                        break;
                    }
                    
                    if (IsTweening == false)
                    {
                        _building.transform.position = _targetPosition;

                        break;
                    }
                    
                    _currentTweenTime -= Time.deltaTime;

                    _building.transform.position = Vector3.Lerp
                        (_targetPosition, _building.transform.position, _currentTweenTime / _tweenDuration);

                    yield return null;
                }
            }
        }
    }
}
