using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;

public class SpawnNetworkPlayer : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPlayer _playerPrefab;

    CharacterInputsHandler _characterInputs;

    public void OnConnectedToServer(NetworkRunner runner)
    {
        if (runner.Topology == SimulationConfig.Topologies.Shared)
        {
            Debug.Log("[Custom Msg] On Connected To Server - Spawning Player as Local...");
            runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, runner.LocalPlayer);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!NetworkPlayer.Local) return;

        if (!_characterInputs)
        {
            _characterInputs = NetworkPlayer.Local.GetComponent<CharacterInputsHandler>();
        }
        else
        {
            input.Set(_characterInputs.GetNetworkInputs());
        }
    }
}
