using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    void Start()
    {
        SceneChange();
    }

    void SceneChange()
    {
       SceneManager.LoadSceneAsync("Digmode");
    }
}
