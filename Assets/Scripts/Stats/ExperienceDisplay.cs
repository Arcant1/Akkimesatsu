using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        TextMeshProUGUI text;

        private void Awake()
        {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += UpdateExperience;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= UpdateExperience;
        }
        private void UpdateExperience(Scene scene, LoadSceneMode loadSceneMode)
        {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();

        }
        private void Update()
        {
            text.text = string.Format("{0:0}", experience.GetExperience());
        }
    }
}
