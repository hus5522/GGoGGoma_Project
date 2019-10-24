using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    float xMove, yMove, x_data, y_data;
    int speed = 5;
    bool cheat=false;

    private bool coolTime;  // 무적 난발 방지

	// Use this for initialization
	void Start () {
        coolTime = false;
        
    }

	
	// Update is called once per frame
	void Update () {

        xMove = 0;
        yMove = 0;

        x_data = this.gameObject.transform.position.x;
        y_data = this.gameObject.transform.position.y;

        // y = +-4.48
        // x = +-8.35

        if (Input.GetKey(KeyCode.RightArrow) && (x_data <= 8.35)) // 오른쪽
            xMove = speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.LeftArrow) && (x_data >= -8.35)) // 왼쪽
            xMove = -speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow) && (y_data <= 4.48)) // 위쪽
            yMove = speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.DownArrow) && (y_data >= -4.48)) // 아래쪽
            yMove = -speed * Time.deltaTime;

        /*
        if (Input.GetKeyDown(KeyCode.Space)) // Space 바를 눌렀을 때
            cheat = true;
        if (Input.GetKeyUp(KeyCode.Space)) // Space 바를 땠을 때
            cheat = false;
            */
        if (Input.GetKeyDown(KeyCode.Space) && !coolTime) // Space 바를 눌렀을 때 1초동안 무적, 쿨타임 7초
            StartCoroutine(Immune());

        this.transform.Translate(new Vector3(xMove, yMove, 0));

    }

    IEnumerator Immune()
    {
        // 무적 쿨타임 7초
        cheat = true;
        coolTime = true;
        yield return new WaitForSeconds(1.0f);
        cheat = false;
        yield return new WaitForSeconds(6.0f);
        coolTime = false;
        yield break;

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag.Equals("bullet"))
        // 총알일 경우
        {
            if (!cheat)
            {
                Destroy(this.gameObject);

                WholeGameManager.instance.MoneyCheck -= CameraScript.instance.Betting;
                CameraScript.instance.StoreHealth();
                CameraScript.instance.ShowLoseWindow();
                Time.timeScale = 0;
                /*
                GameObject.Find("Canvas").transform.Find("GameOverPanel").gameObject.SetActive(true);
                Time.timeScale = 0;
                */
            }
        }

    }

}
