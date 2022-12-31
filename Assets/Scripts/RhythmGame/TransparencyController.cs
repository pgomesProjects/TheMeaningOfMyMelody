using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    private float globalAlpha;

    private void Start()
    {
        globalAlpha = 1f;
    }

    public void SetAlpha(float alpha)
    {
        globalAlpha = alpha;
        foreach(SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            Color spriteColor = sprite.color;
            spriteColor.a = alpha;
            sprite.color = spriteColor;
        }
    }

    public float GetAlpha() => globalAlpha;
}
