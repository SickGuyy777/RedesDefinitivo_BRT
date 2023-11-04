using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;

public class TankController : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject _bulletPrefab, _missilePrefab;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _bulletSpeed;
    public bool homingMissile, laser, canShoot;
    public float shootRate;//esto modifica el cooldown de disparo en el inspector
    public float ShootRateTime = 0;
    [Header("Values")]

    [Networked(OnChanged = nameof(OnLifeChanged))]
    [SerializeField] float _rotSpeed;
    [SerializeField] float _life { get; set; }
    [SerializeField] float _maxSpeed;
    [SerializeField] int _maxAmmo;
    [SerializeField] float _maxTimeTimer;
    [SerializeField] Transform _rootHeadTanq;
    [SerializeField] float rootSpeedHead;
    public int maxAmmo;
    public int currentAmmo;
    float _currentTimerTime;
    float _movementSpeed;
    float _rotZ;

    public event Action<float> OnLifeChange = delegate { };

    private void Start()
    {
        _movementSpeed = _maxSpeed;
        currentAmmo = _maxAmmo;
        _currentTimerTime = _maxTimeTimer;
        currentAmmo = maxAmmo;
        canShoot = true;
    }

    private void Update()
    {
        Movement();
        RotHeadTank(_rootHeadTanq);
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0 && canShoot)
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
    }


    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h < 0)
            _rotZ += Time.deltaTime * _rotSpeed;
        else if (h > 0)
            _rotZ += -Time.deltaTime * _rotSpeed;

        transform.rotation = Quaternion.Euler(0, 0, _rotZ);

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

    public void RotHeadTank(Transform head)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - head.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        head.rotation = Quaternion.RotateTowards(head.rotation, Quaternion.Euler(0, 0, angle - 90), rootSpeedHead * Time.deltaTime);

    }

    void Shoot(GameObject currentBullet)
    {
        currentAmmo--;
        var bullet = Instantiate(currentBullet, _shootPoint.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = _shootPoint.up * _bulletSpeed;
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

    static void OnLifeChanged (Changed<PlayerModel> changed)
    {
        var behaviour = changed.Behaviour;

        behaviour.OnlifeChange(behaviour._life / 100);
    }

    void Dead()
    {
        Runner.Shutdown();
    }

    void Timer()
    {
        _currentTimerTime -= 1 * Time.deltaTime;
        if(_currentTimerTime <= 0)
        {
            currentAmmo = maxAmmo;
            _currentTimerTime = _maxTimeTimer;
        }
    }
}
