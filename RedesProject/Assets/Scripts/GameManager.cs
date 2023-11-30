using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<NetworkPlayer> tanques;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var tanque in tanques)
        {
            if (tanque != null && tanque.lose)
            {
                obj.SetActive(true);
            }
        }
    }
}
