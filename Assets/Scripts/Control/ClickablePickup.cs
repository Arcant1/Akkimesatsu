using RPG.Control;
using RPG.Inventories;
using UnityEngine;

public class ClickablePickup : MonoBehaviour, IRaycastable
{
    Pickup pickup;

    private void Awake()
    {
        pickup = GetComponent<Pickup>();
    }
    public CursorType GetCursorType()
    {
        if (pickup.CanBePickedUp())
        {
            return CursorType.Pickup;
        }
        else return CursorType.FullPickup;
    }

    public bool HandleRaycast(PlayerController playerController)
    {
        if (Input.GetMouseButtonDown(0))
        {
            pickup.PickupItem();
        }
        return true;
    }
}
