using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Transform transForm;

    public Sprite[] faces;
    public Sprite cardBack;

    public int cardIndex;   // ex) faces[cardIndex];

    public void ToggleFace(bool showFace)
    {

        if (showFace)
        {
            // ToDo : Show the card face
            transForm.localScale = new Vector2(2f, 2f); // 사이즈 보완
            spriteRenderer.sprite = faces[cardIndex];
            
        }
        else
        {
            //ToDo : Show the card back 
            //transForm.localScale = new Vector2(0.485f, 0.485f); //사이즈 보완
            transForm.localScale = new Vector2(2f, 2f); // 사이즈 보완
            spriteRenderer.sprite = cardBack;
            
        }
    }


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transForm = GetComponent<Transform>();
    }

}
