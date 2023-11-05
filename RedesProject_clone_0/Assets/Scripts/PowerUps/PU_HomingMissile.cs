using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_HomingMissile : MonoBehaviour
{
    [SerializeField] TankController _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player = collision.GetComponent<TankController>();
        _player.homingMissile = true;
        Destroy(this.gameObject);
    }
}
