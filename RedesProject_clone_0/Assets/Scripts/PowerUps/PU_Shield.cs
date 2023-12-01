using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PU_Shield : NetworkBehaviour
{
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] TankController _player;
    [SerializeField] PowerUpManager Buffs;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player = collision.GetComponent<TankController>();
        if (collision.gameObject.tag == "Player" )
        {
            var bla = Runner.Spawn(shieldPrefab, _player.transform.position, transform.rotation);
            bla.transform.parent = _player.transform;
        }
        if (!Object || !Object.HasStateAuthority) return;

        Desaparesco();
    }
    public void Desaparesco()
    {
        Runner.Despawn(Object);
    }

}
