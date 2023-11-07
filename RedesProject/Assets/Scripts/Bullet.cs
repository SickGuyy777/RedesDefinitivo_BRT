using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Bullet : NetworkBehaviour
{
    [SerializeField] float _maxTimerTime;
    [SerializeField] float _currentTimerTime;

    NetworkRigidbody2D _rb;
    Vector3 _lastVel;

    private void Awake()
    {
        _rb = GetComponent<NetworkRigidbody2D>();
        _currentTimerTime = _maxTimerTime;
    }

    private void Update()
    {
        _lastVel = _rb.Rigidbody.velocity;

        _currentTimerTime -= 1 * Time.deltaTime;
        if (_currentTimerTime <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            var speed = _lastVel.magnitude;
            var dir = Vector3.Reflect(_lastVel.normalized, collision.contacts[0].normal);
            _rb.Rigidbody.velocity = dir * Mathf.Max(speed, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shield")
            Destroy(this.gameObject);
    }
}
