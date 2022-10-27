using System;
using GameResources.Location.Island.Scripts;
using GameResources.Location.Scripts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameResources.Location.Builder.Scripts
{
    public sealed class Builder : MonoBehaviour
    {
        [SerializeField]
        private CellPointer cellPointer;

        [SerializeField]
        private AssetReference buildingReference;
        
        private async void Update()
        {
            if (!cellPointer.IsCellPointedNow || !Input.GetMouseButtonDown(0))
            {
                return;
            }

            var cell = cellPointer.PointedCell;
            var gridTransform = cellPointer.PointedGrid.transform;
            
            var building = await buildingReference.InstantiateAsync(gridTransform).Task;

            var localPosition = cell.GetPosition();
            var position = gridTransform.transform.TransformPoint(localPosition);

            building.transform.position = position;
            building.transform.rotation = gridTransform.rotation;
        }
    }
}