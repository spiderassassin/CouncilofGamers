using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : FireSource
{
    public Rigidbody body;
    public float speed = 10f;
    public GameObject enable;

    private Collider homing;

    private void FixedUpdate()
    {
        if (!homing) return;
        Vector3 v = body.velocity;
        Vector3 dir = (homing.bounds.center - transform.position).normalized;
        Vector3 newv = dir * v.magnitude;
        newv.y = v.y;
        body.velocity = newv;
    }

    public void Launch(Vector3 direction, Vector3 initialVelocity)
    {
        initialVelocity.y = 0;// giving it jump contribution plays weird
        if (!gameObject.activeSelf) SetActive(true);
        body.AddForce(direction * speed +initialVelocity, ForceMode.VelocityChange);
    }

    public void Home(Collider c)
    {
        homing = c;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!TriggerValid(other)) return;

        Projectile p = other.GetComponent<Projectile>();
        if (p)
        {
            p.SetTarget(p.transform.position + Random.insideUnitSphere);
            return;
        }

        base.OnTriggerEnter(other);

        enable.SetActive(true);
        enable.transform.SetParent(null);
        SoundManager.Instance.PlayOneShot(FMODEvents.Instance.fireballexplode, transform.position);
        Destroy(gameObject);
    }
}
