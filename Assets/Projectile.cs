using RPG.Control;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Health target = null;
    [SerializeField] float speed = 1f;
    float damage = 0;
    private void Update()
    {
        if (!target)
        {
            print("no target");
            return;
        }

        if (target.IsDead()) Destroy(gameObject);
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        AIController aIController = other.GetComponent<AIController>();
        if (aIController)
        {
            aIController.Alert();
        }
        Health target = other.GetComponent<Health>();
        target?.TakeDamage(damage);
        Destroy(gameObject);
    }
    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCollider = target.transform.GetComponent<CapsuleCollider>();
        if (targetCollider == null)
            return target.transform.position;
        return target.transform.position + (Vector3.up * 3 * targetCollider.height / 4f);
    }
}
