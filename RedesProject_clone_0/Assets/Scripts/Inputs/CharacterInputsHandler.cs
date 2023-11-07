using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputsHandler : MonoBehaviour
{
    NetworkInputsData _networkInputs;
    public bool canshoot;
    private void Start()
    { 
        _networkInputs = new NetworkInputsData();
    }
    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            canshoot = true;
        }

    }
    public NetworkInputsData GetNetworkInputs()
    {
        _networkInputs.canShoot = canshoot;
        canshoot = false;

        return _networkInputs;
    }
}
