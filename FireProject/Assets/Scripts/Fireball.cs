using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : FireSource
{
    public Rigidbody body;
    public float speed = 10f;

    public void Launch(Vector3 direction)
    {
        if (!gameObject.activeSelf) SetActive(true);
        body.AddForce(direction * speed, ForceMode.VelocityChange);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.isTrigger) return;

        Destroy(gameObject);
    }
}
