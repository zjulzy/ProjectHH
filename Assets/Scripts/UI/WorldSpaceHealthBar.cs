using UnityEngine;
using UnityEngine.UI;

namespace ProjectHH.UI
{
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        // 世界空间下头上的血条
        public Image Front;
        public Image Back;


        public void UpdateHealth(float percent)
        {
            float baseLength = Back.GetComponent<RectTransform>().rect.width;
            Front.GetComponent<RectTransform>().sizeDelta = new Vector2(baseLength * percent, Front.GetComponent<RectTransform>().rect.height);
        }
    }
}