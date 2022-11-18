using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameResources.Location.Scripts
{
    [Serializable]
    public class Rotator
    {
        [SerializeField]
        private UnityEngine.Camera view;
        
        [SerializeField]
        private Transform target;
        
        [SerializeField]
        private float rotateDuration = 0.25f;
        
        private const float ROTATE_ANGLE = 90;
        
        private Quaternion _newRotation;
        
        private bool _isRotating;
        
        public void Rotate(Vector2Int direction)
        {
            if (_isRotating)
            {
                return;
            }

            var list = new List<Vector3>
            {
                target.forward,
                -target.forward,
                target.up,
                -target.up,
                target.right,
                -target.right
            };

            GetCodirectionalAxises(list, out var xAxis, out var yAxis, out var zAxis);
            
            SetNewRotation(direction, zAxis, yAxis, xAxis);

            _ = RotateSmooth();
        }

        private void SetNewRotation(Vector2Int direction, Vector3 zAxis, Vector3 yAxis, Vector3 xAxis)
        {
            var localZAxis = target.InverseTransformVector(zAxis);

            if (direction.y != 0)
            {
                var newLocalZAxis = target.InverseTransformVector(yAxis * direction.y);
                var addRotation = Quaternion.FromToRotation(localZAxis, newLocalZAxis);

                _newRotation = target.localRotation * addRotation;
            }
            else
            {
                var newLocalZAxis = target.InverseTransformVector(xAxis * direction.x);
                var addRotation = Quaternion.FromToRotation(localZAxis, newLocalZAxis);

                _newRotation = target.localRotation * addRotation;
            }
        }

        private void GetCodirectionalAxises
        (
            List<Vector3> list, 
            out Vector3 xAxis, 
            out Vector3 yAxis, 
            out Vector3 zAxis
        )
        {
            var zClosestAngle = 0f;
            zAxis = target.forward;

            foreach (var vector in list)
            {
                GetAxis
                (
                    -1, 
                    view.transform.forward, 
                    vector, 
                    ref zClosestAngle, 
                    ref zAxis
                );
            }

            var xClosestAngle = 0f;
            xAxis = target.forward;

            foreach (var vector in list)
            {
                GetAxis
                (
                    1,
                    view.transform.right,
                    vector, 
                    ref xClosestAngle, 
                    ref xAxis
                );
            }

            var yClosestAngle = 0f;
            yAxis = target.forward;

            foreach (var vector in list)
            {
                GetAxis
                (
                    1, 
                    view.transform.up, 
                    vector, 
                    ref yClosestAngle, 
                    ref yAxis
                );
            }
        }

        private static void GetAxis
        (
            float desireAngle, 
            Vector3 a, 
            Vector3 b, 
            ref float closestAngle, 
            ref Vector3 closestVector
        )
        {
            var angle = Vector3.Dot(b, a);

            if (Mathf.Abs(desireAngle - closestAngle) <= Mathf.Abs(desireAngle - angle))
            {
                return;
            }

            closestAngle = angle;
            closestVector = b;
        }

        private async Task RotateSmooth()
        {
            _isRotating = true;

            while (target.localRotation != _newRotation)
            {
                var t = ROTATE_ANGLE / rotateDuration * Time.deltaTime;

                target.localRotation = Quaternion.RotateTowards
                (
                    target.localRotation, 
                    _newRotation, 
                    t
                );

                await Task.Yield();
            }

            RoundRotation();

            _isRotating = false;
        }

        private void RoundRotation()
        {
            target.eulerAngles = new Vector3
            (
                Mathf.Round(target.eulerAngles.x),
                Mathf.Round(target.eulerAngles.y),
                Mathf.Round(target.eulerAngles.z)
            );
        }
    }
}
