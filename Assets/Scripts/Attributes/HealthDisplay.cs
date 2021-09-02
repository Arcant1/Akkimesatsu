using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        TextMeshProUGUI text;

        private void Awake()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += UpdateHealth;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= UpdateHealth;
        }
        private void UpdateHealth(Scene scene, LoadSceneMode loadSceneMode)
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        }
        private void Update()
        {
            text.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}
