using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScreens : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level");
    }
    public void back()
    {
        Application.Quit();
    }
}
