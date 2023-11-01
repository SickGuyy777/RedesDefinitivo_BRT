using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] float _maxTime;

    public TankController _player;
    float _currentTime;

    private void Awake()
    {
        _currentTime = _maxTime;
    }

    private void Update()
    {
        _player = GetComponentInParent<TankController>();
        _currentTime -= 1 * Time.deltaTime;
        if (_currentTime <= 0)
        {
            _player.canShoot = true;
            Destroy(this.gameObject);
        }
    }
}
