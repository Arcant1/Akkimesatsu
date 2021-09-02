using UnityEngine;
namespace RPG.Combat
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy = null;
        void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (!targetToDestroy)
                    Destroy(targetToDestroy);
                Destroy(gameObject);
            }
        }
    }
}
