using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour {

    public Text TimeCount;
    public static float TimeCost;
    public float TimeLimit;
    private bool getMoney;


    void Start () {
        TimeCost = 0;
        getMoney = false;
	}

	void Update () {

        if (TimeCost < TimeLimit)
        {
            TimeCost += Time.deltaTime;
            TimeCount.text = String.Format("{0:f1} sec", TimeCost).ToString();
        }
        else
        {
            // 이겼을 때의 처리
            Destroy(GameObject.Find("player"));

            if (getMoney == false)
            {
                WholeGameManager.instance.MoneyCheck += CameraScript.instance.Betting;
                getMoney = true;
            }

            CameraScript.instance.StoreHealth();
            CameraScript.instance.ShowWinWindow();
            Time.timeScale = 0;
            /*
            GameObject.Find("Canvas").transform.Find("GameWinPanel").gameObject.SetActive(true);
            Time.timeScale = 0;
            */
        }

	}
}
