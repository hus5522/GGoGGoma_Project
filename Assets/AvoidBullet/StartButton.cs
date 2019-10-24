using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

    void Start()
    {

    }

    void Update()
    {

    }

	public void Start_Btn()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
