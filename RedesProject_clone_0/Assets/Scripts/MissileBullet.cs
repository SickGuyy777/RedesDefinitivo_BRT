using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissileBullet : MonoBehaviour
{
    [SerializeField] float _maxSearchingTime, _maxTimerTime;
    [SerializeField] GameObject[] _tanks;
    [SerializeField] GameObject _currentObjetive;
    [SerializeField] float _targetSpeed, _rotateSpeed;

    bool _isSearching;
    float _currentSearchingTime, _currentTimerTime;
    Rigidbody2D _rb;
    Vector3 _lastVel;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tanks = GameObject.FindGameObjectsWithTag("Player");
        _currentSearchingTime = _maxSearchingTime;
        _currentTimerTime = _maxTimerTime;
    }

    private void Update()
    {
        _lastVel = _rb.velocity;
        _currentSearchingTime -= 1 * Time.deltaTime;
        if(_currentSearchingTime > 0)
        {
            GetClosestEnemy(_tanks);
        }
        else
        {
            Debug.Log("Follow" + _currentObjetive.name);
        }
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            var speed = _lastVel.magnitude;
            var dir = Vector3.Reflect(_lastVel.normalized, collision.contacts[0].normal);
            transform.up = dir;
            _rb.velocity = dir * Mathf.Max(speed, 0f);
        }
        if (collision.gameObject.tag == "Shield")
        {
            Destroy(this.gameObject);
        }
    }

    GameObject GetClosestEnemy(GameObject[] tanks)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in tanks)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
                _currentObjetive = tMin;
            }
        }
        return tMin;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _currentObjetive.transform.position);
    }
}
