using UnityEngine;

[RequireComponent(typeof(GrapObject))]
public class BonusObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private GrappableObjectBehaviour behaviour;
    private GrappbleObjectWithData withData;

    public void Init(GrappableObjectBehaviour behaviour)
    {
        this.behaviour = behaviour;
        withData = this.behaviour as GrappbleObjectWithData;

        GetComponent<GrapObject>().Behaviour = behaviour;

        if (withData == null)
            return;

        withData.ApplyDataTo(spriteRenderer);
    }
}
