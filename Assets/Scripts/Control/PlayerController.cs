using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Health health;
        [System.Serializable]
        struct CursorMapping
        {
#pragma warning disable CS0649
            [SerializeField] internal Vector2 hotspot;
            [SerializeField] internal Texture2D texture;
            [SerializeField] internal CursorType cursorType;
#pragma warning restore CS0649
        }
        [SerializeField] float navMeshProjectionDistance = 1f;
        [SerializeField] CursorMapping[] cursorMapping = { };
        [SerializeField] float raycastRadius=1f;

        private void Awake()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }
        void Update()
        {
            if (InteractWithUI()) return;

            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }


        private bool InteractWithComponent()
        {
            RaycastHit[] raycastHits = SortRaycasts();
            foreach (RaycastHit hit in raycastHits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            if (RaycastNavMesh(out target))
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }
        private bool RaycastNavMesh(out Vector3 targetPosition)
        {
            RaycastHit raycastHit;
            NavMeshHit navMeshHit;
            targetPosition = new Vector3();
            if (!Physics.Raycast(GetMouseRay(), out raycastHit))
                return false;
            if (NavMesh.SamplePosition(
                raycastHit.point, out navMeshHit, navMeshProjectionDistance, NavMesh.AllAreas))
            {
                targetPosition = navMeshHit.position;

                return GetComponent<Mover>().CanMoveTo(targetPosition);
            }
            return false;
        }


        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (CursorMapping cursor in cursorMapping)
            {
                if (cursor.cursorType == cursorType)
                    return cursor;
            }
            return cursorMapping[0];
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping cursorMapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }
        private static Ray GetMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
        private RaycastHit[] SortRaycasts()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(GetMouseRay(),raycastRadius);
            float[] distances = new float[raycastHits.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = raycastHits[i].distance;
            }
            Array.Sort(distances, raycastHits);
            return raycastHits;

        }
    }
}
