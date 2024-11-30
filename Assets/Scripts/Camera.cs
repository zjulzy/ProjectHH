using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectHH
{
    public class Camera : MonoBehaviour
    {
        public float CameraDistance = 3f;
        public float LookAtOffset = 1.5f;
        [ShowInInspector, PropertyRange(0,1)]
        public float CameraAttachProperty = 0.8f;

        private TestCharacter _testCharacter;

        void Start()
        {
            _testCharacter = GameObject.Find("Hime").GetComponent<TestCharacter>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 lookAtPoint = _testCharacter.transform.position + new Vector3(0, LookAtOffset, 0);
            var targetPosition = lookAtPoint + CameraDistance * Vector3.right;
            if (Vector3.Distance(targetPosition, transform.position) < 0.001f)
            {
                transform.position = targetPosition;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, CameraAttachProperty);
            }

            transform.forward = (_testCharacter.transform.position - transform.position).normalized;
        }
    }
}