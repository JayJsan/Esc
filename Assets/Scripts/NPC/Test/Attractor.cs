using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [Header("References")]
    public Transform target;
    public Rigidbody2D rb;
    [Header("Configuration")]
    public bool randomiseDirectionSpread = false;
    public float spreadAngle = 0f;
    public float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;

        if (randomiseDirectionSpread)
        {
            float angle = Random.Range(-spreadAngle, spreadAngle);
            direction = Quaternion.Euler(0, 0, angle) * direction;
        }

        direction.Normalize();
        Vector2 force = direction * speed;
        rb.AddForce(force);
    }
}
