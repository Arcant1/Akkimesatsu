using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using RPG.Utils;
using UnityEngine.Events;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] TakeDamageEvent takeDamage = null;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }
        [SerializeField] UnityEvent onDie = null;


        private LazyValue<float> health;
        private bool alreadyDead = false;
        public bool IsDead() => alreadyDead;
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            if (!IsDead())
            {
                return GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            return 0;
        }


        private void OnEnable()
        {
            if (baseStats)
            {
                baseStats.onLevelUp += RegenerateHealth;
            }
        }
        private void OnDisable()
        {
            if (baseStats)
            {
                baseStats.onLevelUp -= RegenerateHealth;
            }
        }
        private void Start()
        {
            health.ForceInit();
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            health.value = Mathf.Clamp(health.value - damage, 0f, health.value);
            if (health.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }
        public void Heal(float healthToRestore)
        {
            health.value = Mathf.Clamp(health.value + healthToRestore, health.value, GetMaxHealthPoints());
        }
        public float GetHealthPoints() => health.value;
        public float GetPercentage() => 100f * health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        public float GetFraction() => health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        public float GetMaxHealthPoints() => GetComponent<BaseStats>().GetStat(Stat.Health);
        private void RegenerateHealth()
        {
            health.value = Mathf.Max(health.value, GetMaxHealthPoints());
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();
            if (!exp) return;
            exp.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
        private void Die()
        {
            if (alreadyDead) return;
            GetComponent<Animator>().SetTrigger("die");
            alreadyDead = true; // Omae wa mo shindeiru
            GetComponent<ActionScheduler>().CancelCurrentAction(); // NANI?
            GetComponent<Collider>().enabled = false;
        }
        #region Saving
        public object CaptureState()
        {
            SaveInfo data;
            data.health = health.value;
            data.alreadyDead = alreadyDead;
            return data;
        }
        public void RestoreState(object state)
        {
            SaveInfo saveInfo = (SaveInfo)state;
            (health.value, alreadyDead) = (saveInfo.health, saveInfo.alreadyDead);
            GetComponent<Collider>().enabled = !alreadyDead;
            if (!alreadyDead)
                GetComponent<Animator>().Play("Locomotion", -1, 0f);
            else
            {
                GetComponent<Animator>().Play("Die", -1, 0f);
            }

        }
        [System.Serializable]
        internal struct SaveInfo
        {
            internal float health;
            internal bool alreadyDead;
        }
        #endregion
    }
}
