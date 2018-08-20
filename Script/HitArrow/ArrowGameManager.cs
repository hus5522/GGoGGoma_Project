using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGameManager : MonoBehaviour {

    private Vector3 startPosition;
    private float offsetPosition;
    public GameObject upArrowPrefab;
    public GameObject downArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject rightArrowPrefab;
    private int arrowCount;
    private int firstLevel;
    private int count;

    Queue<GameObject> arrowObjectQ = new Queue<GameObject>();
    Queue<string> arrowStringQ = new Queue<string>();

    private string tempArrow;

    // Use this for initialization
    void Start () {
        //startPosition = new Vector3(1.5f, 2f, 0f);
        startPosition = new Vector3(0f, 2f, 0f);
        offsetPosition = 1.5f;
        arrowCount = 0;
        firstLevel = 5;
        count = firstLevel / 2;
        count = count * (-1);

        for (int i = 0; i < firstLevel; i++)
        {
            startPosition = new Vector3(0f + (offsetPosition * count), 2f, 0f);
            AddArrows(startPosition);
            count++;
        }

	}
	
	// Update is called once per frame
	void Update () {
        HandleInput();

        // 다 없앴으면, 새로운것을 만들면서 가운데 정렬 하는 과정
        if (arrowCount == 0)
        {
            if (firstLevel <= 10)
            {
                firstLevel += 2;
                count = firstLevel / 2;
                count = count * (-1);    // 좌우로 화살표들이 퍼져 나가게 하기 위한 연산
            }
            else
            {
                count = firstLevel / 2;
                count = count * (-1);
            }
            for (int i = 0; i < firstLevel; i++)
            {
                startPosition = new Vector3(0f + (offsetPosition * count), 2f, 0f);
                AddArrows(startPosition);
                count++;
            }
        }
	}

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tempArrow = "Up";
            if (arrowStringQ.Peek().Equals(tempArrow))
                DeleteArrows();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tempArrow = "Down";
            if (arrowStringQ.Peek().Equals(tempArrow))
                DeleteArrows();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempArrow = "Left";
            if (arrowStringQ.Peek().Equals(tempArrow))
                DeleteArrows();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempArrow = "Right";
            if (arrowStringQ.Peek().Equals(tempArrow))
                DeleteArrows();
        }
    }

    public void AddArrows(Vector3 position)
    {
        int arrNum = Random.Range(0, 4);
        Vector3 temp = position; 

        if (arrNum == 0)
        {
            GameObject arrowCopy = (GameObject)Instantiate(upArrowPrefab);
            arrowCopy.transform.position = temp;
            arrowObjectQ.Enqueue(arrowCopy);
            arrowStringQ.Enqueue("Up");
        }
        else if (arrNum == 1)
        {
            GameObject arrowCopy = (GameObject)Instantiate(downArrowPrefab);
            arrowCopy.transform.position = temp;
            arrowObjectQ.Enqueue(arrowCopy);
            arrowStringQ.Enqueue("Down");
        }
        else if (arrNum == 2)
        {
            GameObject arrowCopy = (GameObject)Instantiate(leftArrowPrefab);
            arrowCopy.transform.position = temp;
            arrowObjectQ.Enqueue(arrowCopy);
            arrowStringQ.Enqueue("Left");
        }
        else if (arrNum == 3)
        {
            GameObject arrowCopy = (GameObject)Instantiate(rightArrowPrefab);
            arrowCopy.transform.position = temp;
            arrowObjectQ.Enqueue(arrowCopy);
            arrowStringQ.Enqueue("Right");
        }

        arrowCount++;

    } // addArrows()

    public void DeleteArrows()
    {
        GameObject tempObject = arrowObjectQ.Dequeue();
        Destroy(tempObject);
        string garbageInt = arrowStringQ.Dequeue();
        arrowCount--;
    }
}
