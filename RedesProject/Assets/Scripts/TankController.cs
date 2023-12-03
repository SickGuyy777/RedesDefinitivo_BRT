using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;
using UnityEngine.UI;
public class TankController : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnLifeChanged))]
    [SerializeField] float _life { get; set; }
    [SerializeField] GameObject _missilePrefab,RevoteBullet;
    [SerializeField] Transform _shootPoint;
    [SerializeField] NetworkRigidbody2D _MyRb;
    public NetworkInputsData _networkInputs;
    public bool _canShoot { get; set; }
    public bool RevoteMissile = false;
    public bool lose = false;
    public float _rotSpeed;
    public float _movementSpeed;
    public float _maxSpeed;
    public event Action<float> OnLifeChange = delegate { };
    public event Action OnDespawned = delegate { };
    float _rotZ;
    public float _currentTimerTime;
    float _lastFiredTime;
    public GameObject ImageDead;
    //timer del bufo de bala rebotina
    public float _maxBTimeufRebot;
    float _currentTimeBufRebot;
    private void Start()
    {
        _movementSpeed = _maxSpeed;
        _currentTimeBufRebot = _maxBTimeufRebot;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out _networkInputs))
        {
            Movement(_networkInputs.h, _networkInputs.v);
            if (_networkInputs.canShoot && !RevoteMissile)
            {
                Shoot(_missilePrefab);
            }
            if (_networkInputs.canShoot && RevoteMissile)
            {
                Shoot(RevoteBullet);
                TimerRevote();
            }

        }
    }
    void TimerRevote()
    {
        _currentTimeBufRebot -= 1 * Time.deltaTime;
        if(_currentTimerTime<=0)
        {
            RevoteMissile = false;
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
            transform.position += _movementSpeed * Time.deltaTime * transform.up;
        }
        else if (v < 0)
        {
            _movementSpeed = _maxSpeed / 1.5f;
            transform.position += _movementSpeed * Time.deltaTime * -transform.up;
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
        if (_life <= 0)
        {
            Dead();
        }
    }
    static void OnLifeChanged(Changed<TankController> changed)
    {
        var behaviour = changed.Behaviour;

        behaviour.OnLifeChange(behaviour._life / 100);
    }
    public override void Spawned()
    {
        if (!LifeBarHandler.Instance) return;

        LifeBarHandler.Instance.SpawnLifeBar(this);
    }

    void Dead()
    {
        Instantiate(ImageDead, transform.position, transform.rotation);
        Runner.Shutdown();
        lose = true;
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawned();
    }
}
