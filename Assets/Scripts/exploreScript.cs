using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class exploreScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    [SerializeField] private float decayTime;
    public bool selected = false;
    void Update()
    {
        if(!selected && timer <= decayTime)
        {
            timer += Time.deltaTime;
        }
        Color currentColor = spriteRenderer.color;
        currentColor.a = Mathf.Lerp(1f, 0f, timer / decayTime);
        spriteRenderer.color = currentColor;
    }
}
