using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHH
{
    public class Camera : MonoBehaviour
    {
        public float CameraDistance = 3f;
        public float LookAtOffset = 1.5f;
        [ShowInInspector, PropertyRange(0, 1)] public float SmoothTime = 0.8f;

        [ShowInInspector, MinMaxSlider(-90, 90, true)]
        public Vector2 YawLimit = new Vector2(45, 0);

        [ShowInInspector, MinMaxSlider(-90, 90, true)]
        public Vector2 RowLimit = new Vector2(45, 0);

        private Vector3 _smoothSpeed;

        private Transform _testCharacter;

        void Start()
        {
            _testCharacter = GameInstance.Get().GetCurrentPlayer();
            transform.position = _testCharacter.position + new Vector3(0, LookAtOffset, 0) +
                                 Vector3.right * CameraDistance;
            transform.forward = Vector3.left;
        }

        // 相机的tick放到LateUpdate中
        void LateUpdate()
        {
            Vector3 lookAtPoint = _testCharacter.transform.position + new Vector3(0, LookAtOffset, 0);
            var currentLookAtDirection = (transform.position - lookAtPoint).normalized;
            var currentLookAtDirectionXZ = new Vector3(currentLookAtDirection.x, 0, currentLookAtDirection.z).normalized;
            var currentLookAtDirectionXY = new Vector3(currentLookAtDirection.x, currentLookAtDirection.y, 0).normalized;
            var yaw = Vector3.SignedAngle(Vector3.right, currentLookAtDirectionXZ, Vector3.up);
            var row = Vector3.SignedAngle(Vector3.right, currentLookAtDirectionXY, Vector3.forward);
            if (yaw > YawLimit.y || yaw < YawLimit.x || row > RowLimit.y || row < RowLimit.x)
            {
                var targetYaw = Mathf.Clamp(yaw, YawLimit.x, YawLimit.y);
                var targetRow = Mathf.Clamp(row, RowLimit.x, RowLimit.y);
                var targetLookAtDirection = Quaternion.Euler(0, targetYaw, targetRow) * Vector3.right;
                var targetDistance = CameraDistance / Mathf.Cos(Vector3.Angle(targetLookAtDirection, Vector3.right) * Mathf.Deg2Rad);
                var targetPosition = lookAtPoint + targetLookAtDirection * targetDistance;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _smoothSpeed, SmoothTime);
            }
        }
    }
}