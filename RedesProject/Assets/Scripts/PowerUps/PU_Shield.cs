using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Shield : MonoBehaviour
{
    [SerializeField] GameObject _shieldPrefab;
    [SerializeField] TankController _player;
    [SerializeField] PowerUpManager Buffs;
    private void Awake()
    {
        Buffs.GetComponent<PowerUpManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Buffs.currentBuffs--;
            _player = collision.GetComponent<TankController>();
            var inst = Instantiate(_shieldPrefab, _player.transform.position, _player.transform.rotation);
            inst.transform.parent = _player.transform;
            Destroy(this.gameObject);
        }
    }
}
