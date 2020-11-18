using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM_ChangeScene : MonoBehaviour
{
    //Once this method is called by a button, if this is scene A, load scene B, and viceversa.
    public void LoadOtherScene()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) SceneManager.LoadScene(1);

        else SceneManager.LoadScene(0);
    }

    //Once this method is called by a button, quit the application.
    public void Quit()
    {
        Application.Quit();
    }
}