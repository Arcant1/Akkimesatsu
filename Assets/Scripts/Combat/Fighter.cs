using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using RPG.Utils;
using System;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] public float TimeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] UnityEvent onProjectileLaunch = null;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Health target;
        public Health GetTarget() => target;

        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);

        }

        private void Start()
        {
            currentWeapon.ForceInit();
            AttachWeapon(currentWeaponConfig);
            EquipWeapon(defaultWeapon);
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (!target) return;
            if (target.IsDead()) return;
            bool isInRange = GetIsInRange(target.transform);
            if (!isInRange)
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private bool GetIsInRange(Transform target)
        {
            return (Vector3.Distance(transform.position, target.position) < currentWeaponConfig.WeaponRange);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > TimeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }
        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
        // Called by animator event
        void Hit()
        {
            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            target?.TakeDamage(gameObject, GetDamageValue());
        }
        // Called by animator event
        void Shoot()
        {
            if (!target)
                return;
            onProjectileLaunch.Invoke();
            currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, GetDamageValue());
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
                !GetIsInRange(combatTarget.transform)) return false;
            Health testTarget = combatTarget.GetComponent<Health>();
            return testTarget != null && !testTarget.IsDead();
        }
        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.target = target.GetComponent<Health>();
        }
        private float GetDamageValue() => GetComponent<BaseStats>().GetStat(Stat.Damage);

        #region Saving
        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }
        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
        public WeaponConfig GetWeapon() => currentWeaponConfig;


        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            switch (stat)
            {
                case Stat.Health:
                    yield return 0;
                    break;
                case Stat.ExperienceReward:
                    yield return 0;
                    break;
                case Stat.ExperienceToLevelUp:
                    yield return 0;
                    break;
                case Stat.Damage:
                    yield return currentWeaponConfig.GetPercentageBonus();
                    break;
                default:
                    yield return 0;
                    break;
            }
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            switch (stat)
            {
                case Stat.Health:
                    yield return 0;
                    break;
                case Stat.ExperienceReward:
                    yield return 0;
                    break;
                case Stat.ExperienceToLevelUp:
                    yield return 0;
                    break;
                case Stat.Damage:
                    yield return currentWeaponConfig.GetDamage();
                    break;
                default:
                    yield return 0;
                    break;
            }
        }

        #endregion
    }
}
