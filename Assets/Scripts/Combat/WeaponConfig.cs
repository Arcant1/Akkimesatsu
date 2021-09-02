using RPG.Attributes;
using UnityEngine;
namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Akki Messatsu/Weapons/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] public AnimatorOverrideController animatorOverride = null;
        [SerializeField] public Weapon equippedPrefab = null;
        [SerializeField] public float WeaponDamage = 1f;
        [SerializeField] public float percentageBonus = 0f;
        [SerializeField] public float WeaponRange = 1;
        [SerializeField] public bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;
        const string weaponName = "Weapon";
        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float damage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, damage);
        }
        public float GetDamage() => WeaponDamage;
        public float GetPercentageBonus() => percentageBonus;
        public bool HasProjectile() => projectile == null;
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (!oldWeapon)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (!oldWeapon) return;
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return (isRightHanded) ? rightHand : leftHand;
        }
    }
}
