using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (!playerController.GetComponent<Fighter>().CanAttack(gameObject))
                return false;
            if (Input.GetMouseButton(0))
            {
                playerController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
    }
}
