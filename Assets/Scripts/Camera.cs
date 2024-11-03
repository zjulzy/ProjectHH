using UnityEngine;

namespace ProjectHH
{
    public class Camera : MonoBehaviour
    {
        private const float c_CameraDistance = 3f;
        private const float c_LookAtOffset = 1.5f;

        private TestCharacter _testCharacter;

        void Start()
        {
            _testCharacter = GameObject.Find("Hime").GetComponent<TestCharacter>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 lookAtPoint = _testCharacter.transform.position + new Vector3(0, c_LookAtOffset, 0);
            transform.position = lookAtPoint + c_CameraDistance * Vector3.right;
            transform.forward = (_testCharacter.transform.position - transform.position).normalized;
        }
    }
}