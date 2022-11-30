using DG.Tweening;
using GameResources.Control.Scripts;
using GameResources.Location.Builder.Scripts;
using Lean.Gui;
using UnityEngine;

namespace GameResources.UI.LocationUI.Scripts
{
    public class BuildingMenu : MonoBehaviour
    {
        [SerializeField]
        private BuilderEventHandler builderEventHandler;

        [SerializeField]
        private RectTransform container;
        
        [SerializeField]
        private LeanButton build;

        [SerializeField]
        private LeanButton cancel;

        [SerializeField]
        private RectTransform rectTransformBuild;
        
        [SerializeField]
        private Transform showBuildPosition;
        
        [SerializeField]
        private Transform hideBuildPosition;
        
        [SerializeField]
        private Transform openPosition;
        
        [SerializeField]
        private Transform closePosition;
        
        [SerializeField]
        private float tweenTime = 0.25f;
        
        private bool _isOpen;
        private bool _isShowBuild;
        
        private void OnEnable()
        {
            build.OnClick.AddListener(Build);
            cancel.OnClick.AddListener(Cancel);
            
            builderEventHandler.OnValidPosition += OnValidPosition;

            builderEventHandler.OnStartBuilding += StartBuilding;
            builderEventHandler.OnStopBuilding += StopBuilding;

            CloseNow();
        }
        
        private void OnDisable()
        {
            build.OnClick.RemoveListener(Build);
            cancel.OnClick.RemoveListener(Cancel);
            
            builderEventHandler.OnValidPosition -= OnValidPosition;
            
            builderEventHandler.OnStartBuilding -= StartBuilding;
            builderEventHandler.OnStopBuilding -= StopBuilding;
        }
        
        private void Build() => builderEventHandler.InvokeAccept();

        private void Cancel() => builderEventHandler.InvokeCancel();

        private void StartBuilding(BuildingData building) => Open();

        private void StopBuilding() => Close();

        private void OnValidPosition(bool isValid)
        {
            if (isValid)
            {
                ShowBuild();
                
                return;
            }
            
            HideBuild();
        }
        
        private void Open()
        {
            if (_isOpen)
            {
                return;
            }

            _isOpen = true;

            container
                .DOLocalMove(openPosition.localPosition, tweenTime, true);
        }

        private void Close()
        {
            if (_isOpen == false)
            {
                return;
            }

            _isOpen = false;

            container.
                DOLocalMove(closePosition.localPosition, tweenTime, true);

            HideBuild();
        }

        private void CloseNow()
        {
            _isOpen = false;

            container.position = closePosition.position;
            
            HideBuildNow();
        }

        private void ShowBuild()
        {
            if (_isOpen == false)
            {
                return;
            }

            if (_isShowBuild)
            {
                return;
            }
            
            _isShowBuild = true;
            
            rectTransformBuild
                .DOLocalMove(showBuildPosition.localPosition, tweenTime, true);
        }

        private void HideBuild()
        {
            if (_isShowBuild == false)
            {
                return;
            }
            
            _isShowBuild = false;
            
            rectTransformBuild
                .DOLocalMove(hideBuildPosition.localPosition, tweenTime, true);
        }
        
        private void HideBuildNow()
        {
            _isShowBuild = false;

            rectTransformBuild.localPosition = hideBuildPosition.localPosition;
        }
    }
}
