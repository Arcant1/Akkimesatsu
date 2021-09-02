using UnityEngine;
public class DayCicle : MonoBehaviour
{
    public float daySpeed = 2f;
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, daySpeed * Time.deltaTime);
    }
}
