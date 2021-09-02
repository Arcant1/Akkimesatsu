using UnityEngine;
using RPG.Saving;
namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        public float health = 100f;
        private bool alreadyDead = false;


        public bool IsDead() => alreadyDead;


        public void TakeDamage(float damage)
        {
            health = Mathf.Clamp(health - damage, 0f, health);
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (alreadyDead) return;
            GetComponent<Animator>().SetTrigger("die");
            alreadyDead = true; // Omae wa mo shindeiru
            GetComponent<ActionScheduler>().CancelCurrentAction(); // NANI?
            DeactivateRigidBody();
        }
        public object CaptureState()
        {
            return new SaveInfo(health, alreadyDead);
        }
        public void RestoreState(object state)
        {
            SaveInfo saveInfo = (SaveInfo)state;
            (health, alreadyDead) = (saveInfo.health, saveInfo.alreadyDead);
            if (!alreadyDead)
                GetComponent<Animator>().Play("Locomotion", -1, 0f);
            else
            {
                DeactivateRigidBody();
                GetComponent<Animator>().Play("Die", -1, 0f);
            }

        }

        private void DeactivateRigidBody()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.detectCollisions = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }

        [System.Serializable]
        internal class SaveInfo
        {
            internal float health;
            internal bool alreadyDead;
            internal SaveInfo(float health, bool alreadyDead)
            {
                this.alreadyDead = alreadyDead;
                this.health = health;
            }
        }
    }
}
