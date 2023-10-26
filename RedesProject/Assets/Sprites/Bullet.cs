using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _maxTimerTime;
    [SerializeField] float _currentTimerTime;

    Rigidbody2D _rb;
    Vector3 _lastVel;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentTimerTime = _maxTimerTime;
    }

    private void Update()
    {
        _lastVel = _rb.velocity;

        _currentTimerTime -= 1 * Time.deltaTime;
        if (_currentTimerTime <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            var speed = _lastVel.magnitude;
            var dir = Vector3.Reflect(_lastVel.normalized, collision.contacts[0].normal);
            _rb.velocity = dir * Mathf.Max(speed, 0f);
        }
    }
}
