using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameResources.Control.Builder.Scripts;
using GameResources.Utility.Scripts;
using UnityEngine;
using UnityEngine.Pool;

namespace GameResources.UI.GameUI.Location.Scripts
{
    public sealed class BuildingsInteractionValuesView : MonoBehaviour
    {
        [SerializeField]
        private CameraHandler locationCamera;
        
        [SerializeField]
        private InteractionValueView viewPrefab;

        [SerializeField]
        private RectTransform localRect;
        
        [SerializeField]
        private Transform container;

        [SerializeField]
        private BuildingsInteractedValuesHandler handler;

        private List<InteractionValueView> CurrentViews => _viewsArray[_viewsActiveIndex];
        private List<InteractionValueView> NewViews => _viewsArray[_viewsActiveIndex == 0 ? 1 : 0];

        private readonly List<InteractionValueView>[] _viewsArray = new List<InteractionValueView>[2];

        private int _viewsActiveIndex;

        private ObjectPool<InteractionValueView> _pool;
        
        private Coroutine _coroutine;
        
        private void OnEnable()
        {
            InitPool();
            
            Subscribe();

            InitViewsContainers();

            SetViews();

            StartMoveCoroutine();
        }

        private void OnDisable()
        {
            DestroyPool();
            
            Unsubscribe();

            DestroyViewContainer();
            
            StopMoveCoroutine();
        }

        private void InitPool()
        {
            _pool = new ObjectPool<InteractionValueView>
            (
                CreateView,
                GetView,
                ReleaseView,
                DestroyView,
                false,
                10,
                600
            );
        }

        private void DestroyPool()
        {
            _pool.Clear();
            _pool = null;
        }

        private void Subscribe() => handler.OnUpdateObjects += SetViews;

        private void Unsubscribe() => handler.OnUpdateObjects -= SetViews;


        private void StartMoveCoroutine()
        {
            _coroutine = StartCoroutine(MoveViewsToWorldPosition());
        }

        private void StopMoveCoroutine()
        {
            if (_coroutine == null)
            {
                return;
            }

            StopCoroutine(_coroutine);

            _coroutine = null;
        }

        private void InitViewsContainers()
        {
            for (var index = 0; index < _viewsArray.Length; index++)
            {
                _viewsArray[index] = new List<InteractionValueView>();
            }
        }

        private void DestroyViewContainer()
        {
            for (var index = 0; index < _viewsArray.Length; index++)
            {
                _viewsArray[index] = null;
            }
        }

        private IEnumerator MoveViewsToWorldPosition()
        {
            while (enabled)
            {
                MoveViews();

                yield return null;
            }
        }

        private void MoveViews()
        {
            foreach (var view in CurrentViews)
            {
                var worldToScreenPoint = RectTransformUtility.WorldToScreenPoint
                (
                    locationCamera.HandledCamera,
                    view.BuildingsInteractionEventData.Position
                );

                RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    localRect,
                    worldToScreenPoint,
                    null,
                    out var anchoredPosition
                );

                var viewRectTransform = view.transform as RectTransform;

                if (viewRectTransform != null)
                {
                    viewRectTransform.anchoredPosition = anchoredPosition;
                }
            }
        }

        private void SetViews()
        {
            SetNewViews();

            ReleaseUnusedViews();

            SetNewViewsAsCurrent();
        }

        private void SetNewViewsAsCurrent() => _viewsActiveIndex = _viewsActiveIndex == 0 ? 1 : 0;

        private void ReleaseUnusedViews()
        {
            foreach (var oldView in CurrentViews.Where(oldView => !NewViews.Contains(oldView)))
            {
                _pool.Release(oldView);
            }
        }

        private void SetNewViews()
        {
            NewViews.Clear();

            foreach (var interaction in handler.EventsData)
            {
                var oldView = CurrentViews.FirstOrDefault
                    (x => x.BuildingsInteractionEventData.Equals(interaction));

                if (oldView != null)
                {
                    NewViews.Add(oldView);

                    continue;
                }

                var view = _pool.Get();

                view.SetValue(interaction);

                NewViews.Add(view);
            }
        }

        private InteractionValueView CreateView() => Instantiate(viewPrefab, container);

        private static void GetView(InteractionValueView view) => view.gameObject.SetActive(true);

        private static void ReleaseView(InteractionValueView view) => view.gameObject.SetActive(false);

        private static void DestroyView(InteractionValueView view) => Destroy(view.gameObject);
    }
}
