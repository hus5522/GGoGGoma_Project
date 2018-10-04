using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public bool onoff = true;
    public GameObject Enemy; // Prefab을 받을 public 변수 입니다.
    GameObject bullet = null;
    float px, py, bx0, by0;
    float bx, by, speed = 15;
    int i, ran = 0, num = 2, second = 0;
    float[,] rand_f = new float[4, 2];

    void SpawnEnemy()
    {
        /*
         * X좌표       Y좌표
         * small_left + large
         * small_right + large
         * large        + small_top
         * large        + small_bottom
         */

        if (onoff)
        {

            for (i = 0; i < num; i++)
            {
                float randomX_small_left = Random.Range(-9.7f, -9.0f); // 왼쪽 X 좌표
                float randomX_small_right = Random.Range(9.0f, 9.7f); // 오른쪽 X 좌표

                float randomX_large = Random.Range(-9.7f, 9.7f); // 가로 X 좌표

                float randomY_small_top = Random.Range(5.2f, 5.7f); // 위쪽 Y 좌표
                float randomY_small_bottom = Random.Range(-5.7f, -5.2f); // 아래쪽 Y 좌표

                float randomY_large = Random.Range(-5.7f, 5.7f); // 세로 Y 좌표

                rand_f[0, 0] = randomX_small_left; rand_f[0, 1] = randomY_large;
                rand_f[1, 0] = randomX_small_right; rand_f[1, 1] = randomY_large;
                rand_f[2, 0] = randomX_large; rand_f[2, 1] = randomY_small_top;
                rand_f[3, 0] = randomX_large; rand_f[3, 1] = randomY_small_bottom;
                ran = Random.Range(0, 4);



                bullet = (GameObject)Instantiate(Enemy, new Vector3(rand_f[ran, 0], rand_f[ran, 1], 0f), Quaternion.identity);
                // 랜덤한 위치와, 화면 제일 위에서 Enemy를 하나 생성해줍니다.

                bx0 = bullet.transform.position.x;
                by0 = bullet.transform.position.y;

                px = GameObject.Find("player").transform.position.x;
                py = GameObject.Find("player").transform.position.y;

                bx = speed * (bx0 - px);

                by = speed * (by0 - py);

                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector3((-1) * bx, (-1) * by));
            }


            if (second < 30) // 30초 이하일 경우에 초 증가(무한정 증가 방지)
                second++;

            if (second == 10) // 10초 되면 총알 4개로 증가
                num += 2;

            else if (second == 20) // 20초 되면 총알 6개로 증가
                num += 2;

        }
    }

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0, 1); // 0초후 부터, SpawnEnemy함수를 1초마다 반복해서 실행 시킵니다.
    }

    void Update()
    {
    
    }


}
