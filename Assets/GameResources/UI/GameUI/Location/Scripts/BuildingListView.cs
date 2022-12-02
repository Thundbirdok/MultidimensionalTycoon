using System.Collections.Generic;
using GameResources.Control.Builder.Scripts;
using GameResources.Location.Builder.Scripts;
using GameResources.UI.LocationUI.Scripts;
using UnityEngine;
using Zenject;

namespace GameResources.UI.GameUI.Location.Scripts
{
    public class BuildingListView : MonoBehaviour
    {
        [SerializeField]
        private PopulateBuildingList populateBuildingList;

        private BuilderEventHandler _builderEventHandler;
        
        private IReadOnlyList<BuildingView> BuildingsViews => populateBuildingList.BuildingsViews;
        
        [Inject]
        private void Construct
        (
            BuilderEventHandler builderEventHandler, 
            AvailableBuildings availableBuildings, 
            BuildingsViewDataCollector buildingsViewDataCollector
        )
        {
            _builderEventHandler = builderEventHandler;
            
            populateBuildingList.OnBeforeClear += UnsubscribeFromButtons;
            populateBuildingList.OnPopulate += SubscribesToButtons;
            
            populateBuildingList.Construct
            (
                availableBuildings,
                builderEventHandler,
                buildingsViewDataCollector
            );
        }

        private void OnDisable()
        {
            populateBuildingList.OnPopulate -= SubscribesToButtons;
            
            UnsubscribeFromButtons();
        }

        private void SubscribesToButtons()
        {
            foreach (var view in BuildingsViews)
            {
                view.OnButtonClick += ChooseBuilding;
            }
        }

        private void UnsubscribeFromButtons()
        {
            foreach (var view in BuildingsViews)
            {
                view.OnButtonClick -= ChooseBuilding;
            }
        }
        
        private void ChooseBuilding(BuildingViewData building)
        {
            _builderEventHandler.InvokeChooseBuilding(building.BuildingData);
        }
    }
}
