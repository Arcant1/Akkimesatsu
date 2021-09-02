using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        TextMeshProUGUI text;

        private void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += UpdateFighter;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= UpdateFighter;
        }
        private void UpdateFighter(Scene scene, LoadSceneMode loadSceneMode)
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();

        }
        private void Update()
        {
            if (!fighter.GetTarget())
            {
                text.text = "N/A";
                return;
            }
            else
            {
                text.text = string.Format("{0:0}/{1:0}",
                    fighter.GetTarget().GetHealthPoints(),
                    fighter.GetTarget().GetMaxHealthPoints()
                    );
            }
        }
    }
}
