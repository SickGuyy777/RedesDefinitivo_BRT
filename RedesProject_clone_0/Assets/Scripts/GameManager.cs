using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class GameManager : NetworkBehaviour
{
    public List<NetworkPlayer> tanques;
    public GameObject lose;
    public GameObject win;
    public GameObject canvas;
    public StartGameArgs handler;

    void Update()
    {
        if (handler.PlayerCount != 2)
        {
            foreach (var tanque in tanques)
            {
                if (tanque != null && tanque.lose)
                {
                    lose.SetActive(true);

                }
            }
        }

    }
}
