using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarHandler : MonoBehaviour
{

    public static LifeBarHandler Instance { get; private set; }
    [SerializeField] LifeBar _lifebarPrefab;
    List<LifeBar> _lifeBars;
    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
        _lifeBars = new();
    }

    public void SpawnLifeBar(TankController target)
    {
        var newLifebar = Instantiate(_lifebarPrefab, transform);
        newLifebar.SetTarget(target);
        _lifeBars.Add(newLifebar);
        target.OnDespawned += () =>
        {
            _lifeBars.Remove(newLifebar);
            Destroy(newLifebar.gameObject);
        };
    }
    private void LateUpdate()
    {
        foreach (var bar in _lifeBars)
        {
            bar.UpdatePosition();
        }
    }
}
