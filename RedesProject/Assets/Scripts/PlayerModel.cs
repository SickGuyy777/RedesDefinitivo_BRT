using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class PlayerModel : NetworkBehaviour
{
    [SerializeField] NetworkRigidbody _rgbd;
    [SerializeField] NetworkMecanimAnimator _animator;

    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] ParticleSystem _shootParticle;
    [SerializeField] Transform _shootPosition;

    [Networked(OnChanged = nameof(OnLifeChanged))]
    [SerializeField] float _life { get; set; }
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;

    [Networked (OnChanged = nameof(OnFiringChanged))]
    bool _isFiring { get; set; }

    int _previousSign, _currentSign;

    float _lastFiredTime;

    NetworkInputData _networkInput;

    public event Action<float> OnLifeChange = delegate { };
    public event Action OnDespawned = delegate { };

    void Start()
    {
        transform.forward = Vector3.right;
    }

    public override void Spawned()
    {
        if (!LifeBarsHandler.Instance) return;

        LifeBarsHandler.Instance.SpawnLifebar(this);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out _networkInput)) 
        {
            Move(_networkInput.xMovement);

            if (_networkInput.isJumpPressed)
            {
                Jump();
            }
            else if (_networkInput.isFirePressed)
            {
                Shoot();
            }
        }
        
    }

    void Move(float xAxi)
    {
        if (xAxi != 0)
        {
            _rgbd.Rigidbody.MovePosition(transform.position + Vector3.right * (xAxi * _speed * Time.fixedDeltaTime));

            _currentSign = (int)Mathf.Sign(xAxi);
        
            if (_previousSign != _currentSign)
            {
                _previousSign = _currentSign;

                transform.rotation = Quaternion.Euler(Vector3.up * 90 * _currentSign);
            }

            _animator.Animator.SetFloat("MovementValue", Mathf.Abs(xAxi));
        }
        else if (_currentSign != 0)
        {
            _currentSign = 0;
            _animator.Animator.SetFloat("MovementValue", 0);
        }
    }

    void Jump()
    {
        _rgbd.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }

    void Shoot()
    {
        if (Time.time - _lastFiredTime < 0.15f) return;

        _lastFiredTime = Time.time;

        StartCoroutine(FiringCooldown());

        Runner.Spawn(_bulletPrefab, _shootPosition.position, transform.rotation);
    }

    IEnumerator FiringCooldown()
    {
        _isFiring = true;

        yield return new WaitForSeconds(0.15f);

        _isFiring = false;
    }

    static void OnFiringChanged(Changed<PlayerModel> changed)
    {
        bool updateFiring = changed.Behaviour._isFiring;

        changed.LoadOld();

        bool oldFiring = changed.Behaviour._isFiring;

        if (!oldFiring && updateFiring)
        {
            changed.Behaviour._shootParticle.Play();
        }
    }

    public void TakeDamage(float dmg)
    {
        RPC_GetHit(dmg);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_GetHit(float dmg)
    {
        _life -= dmg;

        if (_life <= 0)
        {
            Dead();
        }
    }

    static void OnLifeChanged(Changed<PlayerModel> changed)
    {
        var behaviour = changed.Behaviour;

        behaviour.OnLifeChange(behaviour._life / 100);
    }

    void Dead()
    {
        Runner.Shutdown();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnDespawned();
    }
}
