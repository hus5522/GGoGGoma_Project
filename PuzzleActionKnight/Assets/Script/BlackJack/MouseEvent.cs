using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent : MonoBehaviour {

    [SerializeField]
    private GameObject obj;
    
    private RectTransform rt;
    private RectTransform temp;

    void Start()
    {
        rt = obj.GetComponent<RectTransform>();
        temp = obj.GetComponent<RectTransform>();
    }
    // 적용안됨. 
    public void OnMouseEnter()
    {
        rt.localScale *= 1.2f;
        //obj.transform.localScale = rt.localScale;
        obj.GetComponent<RectTransform>().localScale = rt.localScale;
    }

    public void OnMouseExit()
    {
        rt.localScale = temp.localScale;
        //obj.transform.localScale = rt.localScale;
        obj.GetComponent<RectTransform>().localScale = rt.localScale;
    }

}
