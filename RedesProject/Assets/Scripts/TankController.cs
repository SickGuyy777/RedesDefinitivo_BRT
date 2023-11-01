using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject _bulletPrefab, _missilePrefab;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _bulletSpeed;
    public bool homingMissile, laser, canShoot;

    [Header("Values")]
    [SerializeField] float _rotSpeed;
    [SerializeField] float _maxSpeed;
    [SerializeField] int _maxAmmo;
    [SerializeField] float _maxTimeTimer;
    public int maxAmmo;
    public int currentAmmo;

    float _currentTimerTime;
    float _movementSpeed;
    float _rotZ;

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

        if (Input.GetKeyDown(KeyCode.Space) && currentAmmo > 0 && canShoot)
        {
            if(homingMissile == true)
            {
                homingMissile = false;
                Shoot(_missilePrefab);
            }
            else Shoot(_bulletPrefab);
        }

        if(currentAmmo < maxAmmo)
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

    void Shoot(GameObject currentBullet)
    {
        currentAmmo--;
        var bullet = Instantiate(currentBullet, _shootPoint.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = _shootPoint.up * _bulletSpeed;
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
