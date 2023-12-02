using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
public class GameManager : NetworkBehaviour
{
    public List<NetworkPlayer> tanques;
    public GameObject canvaswin;

    //void Update()
    //{

    //        foreach (var tanque in tanques)
    //        {
    //            if (tanque != null && tanque.lose)
    //            {
    //                var tanqueConFalse = tanques.FirstOrDefault(t => t != null && !t.lose);

    //                if (tanqueConFalse != null)
    //                {
    //                 Instantiate(canvaswin, transform.position, transform.rotation);
    //                }
    //            }
    //        }
    //}
}
