using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PowerUpManager : NetworkBehaviour
{
    [SerializeField] NetworkObject[] _powerUps;

    [Header("VALUES")]
    [SerializeField] float _boundWidth, _boundHeight;
    [SerializeField] float _maxTimerTime;
    float _currentTimerTime;
    int _puIndex;
    public int currentBuffs;
    public int maxBuffs;

    private void Start()
    {
        _currentTimerTime = _maxTimerTime;
        maxBuffs = 5;
    }

    private void Update()
    {
        _currentTimerTime -= 1 * Time.deltaTime;
        if (_currentTimerTime <= 0 /*&& currentBuffs>=maxBuffs*/)
        {
            Runner.Spawn(_powerUps[Random.Range(0, _powerUps.Length)], new Vector3(Random.Range(-_boundWidth / 2, _boundWidth / 2), Random.Range(-_boundHeight / 2, _boundHeight / 2), 0), transform.rotation);
            _currentTimerTime = _maxTimerTime;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 topLeft = new Vector3(-_boundWidth / 2, _boundHeight / 2);
        Vector3 topRigth = new Vector3(_boundWidth / 2, _boundHeight / 2);
        Vector3 botRigth = new Vector3(_boundWidth / 2, -_boundHeight / 2);
        Vector3 botLeft = new Vector3(-_boundWidth / 2, -_boundHeight / 2);
        Gizmos.DrawLine(topLeft, topRigth);
        Gizmos.DrawLine(topRigth, botRigth);
        Gizmos.DrawLine(botRigth, botLeft);
        Gizmos.DrawLine(botLeft, topLeft);
    }
}
