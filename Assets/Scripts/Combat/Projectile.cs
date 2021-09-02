using RPG.Attributes;
using RPG.Control;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] bool isHoming = false;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] GameObject[] destroyOnImpact = null;
    [SerializeField] float lifeAfterImpact = .2f;
    [SerializeField] UnityEvent onProjectileHit = null;
    private Health target = null;
    private float aliveTime = 10f;
    private float damage = 0;
    GameObject instigator = null;
    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }
    private void Update()
    {
        if (target == null) return;

        if (isHoming && !target.IsDead())
            transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    IEnumerator KillStrayProjectile(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
        yield return null;
    }
    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;
        StartCoroutine("KillStrayProjectile", aliveTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Health target = other.GetComponent<Health>();
        onProjectileHit.Invoke();
        target?.TakeDamage(instigator, damage);
        speed = 0;
        if (hitEffect)
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        foreach (GameObject gameObj in destroyOnImpact)
        {
            Destroy(gameObj);
        }
        Destroy(gameObject, lifeAfterImpact);
    }
    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCollider = target.transform.GetComponent<CapsuleCollider>();
        return target.transform.position + (Vector3.up * 3 * targetCollider.height / 4f);
    }
}
