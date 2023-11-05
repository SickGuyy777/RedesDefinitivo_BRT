using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject[] _powerUps;

    [Header("VALUES")]
    [SerializeField] float _boundWidth, _boundHeight;
    [SerializeField] float _maxTimerTime;
    float _currentTimerTime;
    int _puIndex;

    private void Start()
    {
        _currentTimerTime = _maxTimerTime;
    }

    private void Update()
    {
        _currentTimerTime -= 1 * Time.deltaTime;
        if(_currentTimerTime <= 0)
        {
            _puIndex = Random.Range(0, _powerUps.Length);
            Instantiate(_powerUps[_puIndex].gameObject, new Vector3(Random.Range(-_boundWidth / 2, _boundWidth / 2), Random.Range(-_boundHeight / 2, _boundHeight / 2), 0), transform.rotation);
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
