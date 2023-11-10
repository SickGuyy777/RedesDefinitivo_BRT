using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PU_HomingMissile : NetworkBehaviour
{
    [SerializeField] TankController _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player = collision.GetComponent<TankController>();
        _player.homingMissile = true;
        Destroy(this.gameObject);
    }
}
