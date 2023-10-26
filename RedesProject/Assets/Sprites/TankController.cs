using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _bulletSpeed;

    [Header("Values")]
    [SerializeField] float _rotSpeed;
    [SerializeField] float _maxSpeed;
    [SerializeField] int _maxAmmo;
    public int currentAmmo;

    float _movementSpeed;
    float _rotZ;

    private void Start()
    {
        _movementSpeed = _maxSpeed;
        currentAmmo = _maxAmmo;
    }

    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) && currentAmmo > 0)
        {
            currentAmmo--;
            var bullet = Instantiate(_bulletPrefab, _shootPoint.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = _shootPoint.up * _bulletSpeed;
        }
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
}
