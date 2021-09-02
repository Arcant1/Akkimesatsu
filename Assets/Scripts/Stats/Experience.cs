using RPG.Saving;
using System;
using UnityEngine;
namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;

        public event Action onExperienceGained;
        public float GetExperience() => experiencePoints;
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        #region Saving
        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }

        #endregion
    }
}
