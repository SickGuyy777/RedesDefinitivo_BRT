using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class DisparoRebote : NetworkBehaviour
{
    public NetworkRigidbody2D ballRb;
    [SerializeField] int speed;
    [SerializeField] int currentRevote;
    Vector3 _lastVel;
    private void Start()
    {
        ballRb.Rigidbody.velocity = transform.up * speed;
        _lastVel = ballRb.Rigidbody.velocity;
    }
    private void Update()
    {
        if (currentRevote <= 0)
        {
            Desaparesco();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pared"))
        {
            var speed = _lastVel.magnitude;
            var direction = Vector3.Reflect(_lastVel.normalized, collision.contacts[0].normal);
            ballRb.Rigidbody.velocity = direction * Mathf.Max(speed, 0f);
            currentRevote--;
        }
        if (!Object || !Object.HasStateAuthority) return;
        else if (collision.collider.GetComponent<TankController>() != null)
        {
            collision.collider.GetComponent<TankController>().TakeDamage(1f);
        }
        Desaparesco();
    }

    void Desaparesco()
    {
        Runner.Despawn(Object);
    }
}
