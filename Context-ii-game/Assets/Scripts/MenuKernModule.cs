using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuKernModule : MonoBehaviour
{
    public string sceneToLoad;

    public void Restart()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void KillGame()
    {
        Application.Quit();
    }
}
