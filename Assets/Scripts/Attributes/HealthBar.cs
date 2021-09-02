using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] Image foregroundImage = null;
        [SerializeField] Canvas canvas = null;

            float amount = 0;
        void Update()
        {
            amount = health.GetFraction();
            if (Mathf.Approximately(amount, 0)|| Mathf.Approximately(amount, 1))
            {
                canvas.enabled = false;
                return;
            }
            canvas.enabled = true;
            foregroundImage.transform.localScale = new Vector3(amount, 1f, 1f);
        }
    }
}
