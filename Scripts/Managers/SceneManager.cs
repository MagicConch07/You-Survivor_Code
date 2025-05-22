using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoSingleton<SceneManager>
{
    public void LoadStartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void LoadTitleScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ExitUnity()
    {
        Application.Quit();
    }
}

