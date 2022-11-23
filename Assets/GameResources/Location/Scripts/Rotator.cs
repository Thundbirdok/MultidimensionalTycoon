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

        private List<Vector3> _targetAxes;

        public void Rotate(Vector2Int direction)
        {
            if (_isRotating)
            {
                return;
            }

            GetCoDirectionalAxes(out var axes);
            
            _newRotation = GetNewRotation(direction, axes);

            _ = RotateSmooth();
        }

        private Quaternion GetNewRotation(Vector2Int direction, in Vector3[] axes)
        {
            var localZAxis = target.InverseTransformVector(axes[2]);

            var newLocalZAxis = direction.y != 0 ? axes[1] * direction.y : axes[0] * direction.x;

            newLocalZAxis = target.InverseTransformVector(newLocalZAxis);
            
            var addRotation = Quaternion.FromToRotation(localZAxis, newLocalZAxis);

            return target.localRotation * addRotation;
        }

        private void GetCoDirectionalAxes(out Vector3[] axes)
        {
            axes = new[]
            {
                Vector3.zero,
                Vector3.zero,
                Vector3.zero
            };

            if (_targetAxes == null)
            {
                SetTargetAxes();
            }

            GetCoDirectionalAxis(ref axes, 0, view.transform.right, 1);
            GetCoDirectionalAxis(ref axes, 1, view.transform.up, 1);
            GetCoDirectionalAxis(ref axes, 2, view.transform.forward, -1);
        }

        private void GetCoDirectionalAxis
        (
            ref Vector3[] axes, 
            in int index, 
            in Vector3 viewAxis, 
            in float desireAngle
        )
        {
            var closestAngle = 0f;
            var closestAxis = viewAxis;

            foreach (var vector in _targetAxes)
            {
                CheckAxis
                (
                    desireAngle, 
                    viewAxis, 
                    vector,
                    axes,
                    index,
                    ref closestAngle, 
                    ref closestAxis
                );
            }
            
            axes[index] = closestAxis;
        }

        private static void CheckAxis
        (
            in float desireAngle,
            in Vector3 a,
            in Vector3 b,
            in Vector3[] axes,
            in int index,
            ref float closestAngle,
            ref Vector3 closestAxis
        )
        {
            var angle = Vector3.Dot(b, a);

            if (Mathf.Abs(desireAngle - closestAngle) <= Mathf.Abs(desireAngle - angle))
            {
                return;
            }

            for (var i = 0; i < axes.Length; i++)
            {
                if (i >= index)
                {
                    continue;
                }

                if (Mathf.Abs(Vector3.Dot(axes[i], b)) - 0.01 > 0)
                {
                    return;
                }
            }

            closestAngle = angle;
            closestAxis = b;
        }

        private async Task RotateSmooth()
        {
            _isRotating = true;

            var speed = ROTATE_ANGLE / rotateDuration;
            
            while (target.localRotation != _newRotation)
            {
                target.localRotation = Quaternion.RotateTowards
                (
                    target.localRotation,
                    _newRotation,
                    speed * Time.deltaTime
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

        private void SetTargetAxes()
        {
            var forward = target.forward;
            var up = target.up;
            var right = target.right;

            _targetAxes = new List<Vector3>
            {
                forward,
                -forward,
                up,
                -up,
                right,
                -right
            };
        }
    }
}
