using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Attributes
{
    public class LevelDisplay: MonoBehaviour
    {
        BaseStats baseStats;
        TextMeshProUGUI text;

        private void Awake()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += UpdateBaseStats;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= UpdateBaseStats;
        }
        private void UpdateBaseStats(Scene scene, LoadSceneMode loadSceneMode)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();

        }
        private void Update()
        {
            text.text = string.Format("{0}", GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>().GetLevel());
        }
    }
}
