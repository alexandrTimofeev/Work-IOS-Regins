using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Collider2D))]
public class DamageHitBox2D : MonoBehaviour
{
    [SerializeField] private List<string> colliderTargetTags = new List<string>();
    [SerializeField] private List<string> destoryMeTags = new List<string>();
    [SerializeField] private bool deliteIfCollisionTarget = true;

    [Space]
    [SerializeField] private bool destroyGOForDelite =   true;

    public event Action<DamageHurtBox2D> OnCollisionDamageCollider;
    public event Action<DamageHurtBox2D> OnHitDamageCollider;
    public event Action<DamageHitBox2D> OnDelite;

    public void CollisionDamageCollider(DamageHurtBox2D damageCollider)
    {
        OnCollisionDamageCollider?.Invoke(damageCollider);
        if (destoryMeTags.Any((s) => damageCollider.ColliderTags.Contains(s)))
            Delite();
    }

    public void HitDamageCollider(DamageHurtBox2D damageCollider)
    {
        OnHitDamageCollider?.Invoke(damageCollider);
        if (deliteIfCollisionTarget)        
            Delite();
    }

    private void Delite()
    {
        OnDelite?.Invoke(this);
        if(destroyGOForDelite)
         Destroy(gameObject);
    }

    public bool ContainsAnyTags(params string[] tags)
    {
        return colliderTargetTags.Any((s) => tags.Contains(s));
    }
}