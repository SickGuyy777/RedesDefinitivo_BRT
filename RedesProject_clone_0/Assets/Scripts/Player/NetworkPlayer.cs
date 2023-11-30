using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }
    public bool lose;
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
        }
    }

}
