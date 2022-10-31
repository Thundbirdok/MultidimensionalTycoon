using System.Collections.Generic;
using GameResources.Location.Building.Scripts;
using UnityEngine;
using Zenject;

namespace GameResources.Location.Builder.Scripts
{
    public class BuildingListView : MonoBehaviour
    {
        [SerializeField]
        private BuilderEventHandler builderEventHandler;

        [SerializeField]
        private PopulateBuildingList populateBuildingList;
        
        private BuildingsViewDataCollector _buildingsViewDataCollector;

        private IReadOnlyList<BuildingView> BuildingsViews => populateBuildingList.BuildingsViews;
        
        [Inject]
        private void Construct(BuildingsViewDataCollector buildingsViewDataCollector)
        {
            _buildingsViewDataCollector = buildingsViewDataCollector;

            populateBuildingList.OnPopulate += SubscribesToButtons;
            
            populateBuildingList.Construct(_buildingsViewDataCollector);
        }

        private void SubscribesToButtons()
        {
            populateBuildingList.OnPopulate += SubscribesToButtons;
            
            foreach (var view in BuildingsViews)
            {
                view.OnButtonClick += ChooseBuilding;
            }
        }

        private void ChooseBuilding(BuildingViewData building)
        {
            builderEventHandler.InvokeChooseBuilding(building.BuildingData);
        }
    }
}
