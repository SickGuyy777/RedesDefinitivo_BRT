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
        if (collision.gameObject.tag == "Player")
        {
            _player._networkInputs.RevoteMissile = true;
        }
        if (!Object || !Object.HasStateAuthority) return;

        Desaparesco();
    }
    public void Desaparesco()
    {
        Runner.Despawn(Object);
    }
}
