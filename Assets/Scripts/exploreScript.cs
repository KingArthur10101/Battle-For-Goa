using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class exploreScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float timer = 0f;
    [SerializeField] private float decayTime;
    void Update()
    {
        timer += Time.deltaTime;
        Color currentColor = spriteRenderer.color;
        currentColor.a = Mathf.Lerp(1f, 0f, timer / decayTime);
        spriteRenderer.color = currentColor;
    }
}
