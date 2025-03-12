using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OSB_Warning : MonoBehaviour
{
    Color originalColor = Color.HSVToRGB(0.916f, 1f, 1f);

    // Start is called before the first frame update
    void OnEnable()
    {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        originalColor = render.color;
        originalColor.a = 0f;
        render.color = originalColor;
        render.DOFade(0.25f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
