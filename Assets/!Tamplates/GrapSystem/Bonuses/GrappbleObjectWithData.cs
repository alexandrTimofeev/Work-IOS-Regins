using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GrappableObjectBehaviour", menuName = "SGames/GrapSystem/GOBehaviourWithData")]
public class GrappbleObjectWithData : GrappableObjectBehaviour
{
    [Space, Header("Data")]
    public Sprite Icon;
    public float ScaleIcon = 1f;
    public Color Color;

    public void ApplyDataTo(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.sprite = Icon;
        spriteRenderer.color = Color;
        spriteRenderer.transform.localScale *= ScaleIcon;
    }
}
