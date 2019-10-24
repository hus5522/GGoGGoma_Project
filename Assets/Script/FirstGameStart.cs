using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FirstGameStart : MonoBehaviour {


    public void GoToSelectGame()
    {
        SceneManager.LoadScene("MainGameScreen");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
