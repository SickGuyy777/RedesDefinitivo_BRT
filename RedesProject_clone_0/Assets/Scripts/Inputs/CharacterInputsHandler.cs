using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputsHandler : MonoBehaviour
{
    NetworkInputsData _networkInputs;

    public bool _isJumpPressed;
    public bool _isFirePressed;

    private void Start()
    {
        _networkInputs = new NetworkInputsData();
    }

    void Update()
    {
        _networkInputs.xMovement = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            _isJumpPressed = true;
        }
    }

    public NetworkInputsData GetNetworkInputs()
    {
        _networkInputs.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _networkInputs.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        return _networkInputs;
    }
}
