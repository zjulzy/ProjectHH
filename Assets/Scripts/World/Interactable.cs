using UnityEngine;

namespace ProjectHH.World
{
    public enum InteractableType
    {
        Climb
    }
    [RequireComponent(typeof(Collider))]
    public class Interactable:MonoBehaviour
    {
        public InteractableType Type;
    }
}