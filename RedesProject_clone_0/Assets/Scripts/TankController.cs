using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;

public class TankController : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnLifeChanged))]
    [SerializeField] float _life { get; set; }

    [SerializeField] Bullet _missilePrefab;
    [SerializeField] Transform _shootPoint;
    [SerializeField] NetworkRigidbody2D _MyRb;
    NetworkInputsData _networkInputs;
    public bool homingMissile, laser;
    public bool _canShoot { get; set; }
    public float _rotSpeed;
    public float _movementSpeed;
    public float _maxSpeed;
    public event Action<float> OnLifeChange = delegate { };
    public event Action OnDespawned = delegate { };
    public int _maxAmmo;
    public int maxAmmo;
    public int currentAmmo;
    float _rotZ;
    public float _currentTimerTime;
    float _lastFiredTime;
    private void Start()
    {
        _movementSpeed = _maxSpeed;
        currentAmmo = _maxAmmo;
        currentAmmo = maxAmmo;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out _networkInputs))
        {
            Movement(_networkInputs.h, _networkInputs.v);
            if (_networkInputs.canShoot && homingMissile == false)
            {
                Shoot(_missilePrefab);
            }
        }


    }
    void Shoot(Bullet _bulletPrefab)
    {
        if (Time.time - _lastFiredTime < 0.15f) return;

        _lastFiredTime = Time.time;

        StartCoroutine(FiringCooldown());
        Runner.Spawn(_bulletPrefab, _shootPoint.position, transform.rotation);
    }
    IEnumerator FiringCooldown()
    {
        _canShoot = true;

        yield return new WaitForSeconds(0.15f);

        _canShoot = false;
    }

    void Movement(float h, float v)
    {
        h = Input.GetAxisRaw("Horizontal");
        if (h < 0)
            _rotZ += Time.deltaTime * _rotSpeed;
        else if (h > 0)
            _rotZ += -Time.deltaTime * _rotSpeed;

        transform.rotation = Quaternion.Euler(0, 0, _rotZ);

        v = Input.GetAxisRaw("Vertical");
        if (v > 0)
        {
            _movementSpeed = _maxSpeed;
            transform.position += transform.up * _movementSpeed * Time.deltaTime;
        }
        else if (v < 0)
        {
            _movementSpeed = _maxSpeed / 1.5f;
            transform.position += -transform.up * _movementSpeed * Time.deltaTime;
        }
    }
    public void TakeDamage(float dmg)
    {
        RPC_GetHit(dmg);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_GetHit(float dmg)
    {
        _life -= dmg;

        OnLifeChange(_life / 100);

        if (_life <= 0)
        {
            Dead();
        }
    }
    public override void Spawned()
    {
        if (!LifeBarHandler.Instance) return;

        LifeBarHandler.Instance.SpawnLifeBar(this);
    }
    static void OnLifeChanged(Changed<TankController> changed)
    {
        var behaviour = changed.Behaviour;
        behaviour.OnLifeChange(behaviour._life / 100);
    }

    void Dead()
    {
        Runner.Shutdown();
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawned();
    }
}
