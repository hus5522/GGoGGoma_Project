using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlipper : MonoBehaviour {
    

    SpriteRenderer spriteRenderer;
    CardModel model;

    public AnimationCurve scaleCurve; // 카드를 뒤집는 애니매이션을 위한 것(x축 : 시간, y축 : scale)
    public float duration = 0.5f;
    Transform transForm;
    Vector3 localScale;
    void Awake()
    { 

        spriteRenderer = GetComponent<SpriteRenderer>();
        model = GetComponent<CardModel>();
        transForm = GetComponent<Transform>();
        localScale = transForm.localScale;
        localScale = new Vector2(2f, 2f); // 사이즈 보완
    }
    
    //startImage = 카드 뒷면, endImage = 카드 앞면
    public void FlipCard(Sprite startImage, Sprite endImage, int cardIndex)
    {
        StopCoroutine(Flip(startImage, endImage, cardIndex));
        StartCoroutine(Flip(startImage, endImage, cardIndex));
    }
    

    
    IEnumerator Flip(Sprite startImage, Sprite endImage, int cardIndex)
    {
        spriteRenderer.sprite = startImage;

        float time = 0f;
        while (time <= 1f)
        {
            float scale = scaleCurve.Evaluate(time);
            time = time + Time.deltaTime / duration;

            //Vector3 localScale = transform.localScale;
            localScale.x = scale;   //x축으로 회전시켜서 뒤집기 위함
            transform.localScale = localScale;

            if (time >= 0.5f)
            {
                spriteRenderer.sprite = endImage;
            }

            yield return new WaitForFixedUpdate();
        }

        if (cardIndex == -1)
        {
            model.ToggleFace(false);
        }
        else
        {
            model.cardIndex = cardIndex;
            model.ToggleFace(true);
        }
    }
    
}
