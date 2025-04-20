using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMain : MonoBehaviour
{
    public void LoadMainScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
}
