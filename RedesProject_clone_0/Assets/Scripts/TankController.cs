using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;

public class TankController : NetworkBehaviour
{
    //[Networked(OnChanged = nameof(OnLifeChanged))]
    [SerializeField] float _life { get; set; }
    [SerializeField] GameObject _bulletPrefab, _missilePrefab;
    [SerializeField] Transform _shootPoint;
    NetworkInputsData _networkInputs;
    public float _bulletSpeed;
    public bool homingMissile, laser;
    public float _rotSpeed;
    public float _movementSpeed;
    public float _maxSpeed;
    public event Action<float> OnLifeChange = delegate { };
    public float shootRate;//esto modifica el cooldown de disparo en el inspector
    public float ShootRateTime;
    public int _maxAmmo;
    public float _maxTimeTimer;
    public int maxAmmo;
    public int currentAmmo;
    public float _currentTimerTime;
    private void Start()
    {
        _movementSpeed = _maxSpeed;
        currentAmmo = _maxAmmo;
        _currentTimerTime = _maxTimeTimer;
        currentAmmo = maxAmmo;
        _networkInputs.canShoot = true;
    }
    private void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0 && _networkInputs.canShoot)
        {
            if (Time.time > ShootRateTime) //coldown de disparo
            {
                ShootRateTime = Time.time + shootRate;
                if (homingMissile == true)
                {
                    homingMissile = false;
                    Shoot(_missilePrefab);
                }
                else Shoot(_bulletPrefab);
            }
        }
        if (currentAmmo < maxAmmo)
            Timer();

        if (GetInput(out _networkInputs))
        {

        }
    }
    void Shoot(GameObject currentBullet)
    {
        currentAmmo--;
        var bullet = Instantiate(currentBullet, _shootPoint.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = _shootPoint.up * _bulletSpeed;
    }
    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h < 0)
            _networkInputs._rotZ += Time.deltaTime * _rotSpeed;
        else if (h > 0)
            _networkInputs._rotZ += -Time.deltaTime * _rotSpeed;

        transform.rotation = Quaternion.Euler(0, 0, _networkInputs._rotZ);

        float v = Input.GetAxisRaw("Vertical");
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

    void Timer()
    {
        _currentTimerTime -= 1 * Time.deltaTime;
        if (_currentTimerTime <= 0)
        {
            currentAmmo = maxAmmo;
            _currentTimerTime = _maxTimeTimer;
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
            //Dead();
        }
    }

    //static void OnLifeChanged(Changed<TankController> changed)
    //{
    //    var behaviour = changed.Behaviour;

    //    behaviour.OnlifeChange(behaviour._life / 100);
    //}

    //void Dead()
    //{
    //    Runner.Shutdown();
    //}
}
