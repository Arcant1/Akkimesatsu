using TMPro;
using UnityEngine;
namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text = null;

        public void SetDamage(float damage)
        {
            text.SetText(string.Format("{0:0}", damage));
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}
