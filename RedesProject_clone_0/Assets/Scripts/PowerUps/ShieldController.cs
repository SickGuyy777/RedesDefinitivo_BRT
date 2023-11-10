using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class ShieldController : NetworkBehaviour
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
            Destroy(this.gameObject);
        }
    }
}
