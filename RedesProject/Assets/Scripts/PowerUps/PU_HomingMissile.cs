using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PU_HomingMissile : NetworkTransform
{
    [SerializeField] TankController _player;
    [SerializeField] PowerUpManager spawnBuffs;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player = collision.GetComponent <TankController>();
        if (collision.gameObject.tag == "Player")
        {
            spawnBuffs.currentBuffs--;
            _player.RevoteMissile = true;
        }
        if (!Object || !Object.HasStateAuthority) return;

        Desaparesco();
    }
    public void Desaparesco()
    {
        Runner.Despawn(Object);
    }
}
