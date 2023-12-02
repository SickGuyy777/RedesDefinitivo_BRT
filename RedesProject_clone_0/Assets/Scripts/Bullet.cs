using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Bullet : NetworkBehaviour
{
    [SerializeField] float _maxTimerTime;
    [SerializeField] float _currentTimerTime;
    public float _bulletSpeed;
    [SerializeField] NetworkRigidbody2D _rb;
    Vector3 _lastVel;
    private void Awake()
    {
        _rb = GetComponent<NetworkRigidbody2D>();
        _currentTimerTime = _maxTimerTime;
    }
    private void Start()
    {
        _rb.Rigidbody.velocity = transform.up * _bulletSpeed;
        _lastVel = _rb.Rigidbody.velocity;
    }

    private void Update()
    {
        _currentTimerTime -= 1 * Time.deltaTime;
        if (_currentTimerTime <= 0)
        {
            Destroy(gameObject);
            Desaparesco();
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shield" || collision.gameObject.tag == "Pared")
        {
            Desaparesco();
        }

        if (!Object || !Object.HasStateAuthority) return;

        if (collision.TryGetComponent(out TankController enemy))
        {
            enemy.TakeDamage(25f);
        }
        Desaparesco();

    }
    public void Desaparesco()
    {
        Runner.Despawn(Object);
    }
}
