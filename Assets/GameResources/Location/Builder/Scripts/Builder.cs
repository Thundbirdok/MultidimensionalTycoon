using GameResources.Location.Scripts;
using UnityEngine;

namespace GameResources.Location.Builder.Scripts
{
    public sealed class Builder : MonoBehaviour
    {
        [SerializeField] private Camera raycastCamera;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) == false)
            {
                return;
            }

            if (TryGetPointedCell(out var cell))
            {
                Debug.Log(cell.Position);
            }
        }

        private bool TryGetPointedCell(out Cell cell)
        {
            var ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, 50) == false)
            {
                cell = null;
                
                return false;
            }

            if (hit.collider.gameObject.TryGetComponent(out GridProvider grid) == false)
            {
                cell = null;
                
                return false;
            }

            var localPoint = grid.transform.InverseTransformPoint(hit.point);

            return grid.Grid.TryGetPointedCell(localPoint, out cell);
        }
    }
}