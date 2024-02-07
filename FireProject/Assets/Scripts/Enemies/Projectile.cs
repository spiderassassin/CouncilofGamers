using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 dest;

    // Start is called before the first frame update
    void Start()
    {
        dest = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the projectile toward the player's position at the time of firing.
        transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);

        // If the projectile reaches the destination, destroy it.
        if (transform.position == dest) {
            Destroy(gameObject);
        }
    }
}
