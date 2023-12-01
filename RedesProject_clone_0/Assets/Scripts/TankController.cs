using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;

public class TankController : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnLifeChanged))]
    [SerializeField] float _life { get; set; }
    [SerializeField] GameObject _missilePrefab,RevoteBullet;
    [SerializeField] Transform _shootPoint;
    [SerializeField] NetworkRigidbody2D _MyRb;
    public NetworkInputsData _networkInputs;
    public bool _canShoot { get; set; }
    public float _rotSpeed;
    public float _movementSpeed;
    public float _maxSpeed;
    public event Action<float> OnLifeChange = delegate { };
    public event Action OnDespawned = delegate { };
    float _rotZ;
    public float _currentTimerTime;
    float _lastFiredTime;
    //timer del bufo de bala rebotina
    public float _maxBTimeufRebot;
    float _currentTimeBufRebot;
    private void Start()
    {
        _movementSpeed = _maxSpeed;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out _networkInputs))
        {
            Movement(_networkInputs.h, _networkInputs.v);
            if (_networkInputs.canShoot && !_networkInputs.RevoteMissile)
            {
                Shoot(_missilePrefab);
            }
            if (_networkInputs.canShoot && _networkInputs.RevoteMissile)
            {
                Shoot(RevoteBullet);
            }

        }
    }
    void TimerRevote()
    {
        _currentTimeBufRebot -= 1 * Time.deltaTime;
        if(_currentTimerTime<=0)
        {
            _networkInputs.RevoteMissile = false;
            _currentTimerTime = _maxBTimeufRebot;
        }
    }
    void Shoot(GameObject _bulletPrefab)
    {
        if (Time.time - _lastFiredTime < 1f) return;

        _lastFiredTime = Time.time;

        StartCoroutine(FiringCooldown());
        Runner.Spawn(_bulletPrefab, _shootPoint.position, transform.rotation);
    }
    IEnumerator FiringCooldown()
    {
        _canShoot = true;

        yield return new WaitForSeconds(1f);

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
        //pl.lose = true;
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawned();
    }
}
