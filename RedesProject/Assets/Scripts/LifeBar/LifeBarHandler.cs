using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarHandler : MonoBehaviour
{

    public static LifeBarHandler Instance { get; private set; }


    [SerializeField] LifeBar _lifebarPrefab;

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }

    public void SpawnLifeBar(PlayerModel target)
    {
        var newLifeBar = Instantiate(_lifebarPrefab, transform);
        newLifeBar.SetTarget(target);
    }

}
