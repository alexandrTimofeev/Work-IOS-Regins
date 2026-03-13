using System;

[Serializable]
public class AddSizeGrapAction : GrappableObjectBehaviourAction
{
    public float AddSize;
    public float Duration;

    public override void Work(BonusActionContext context) { }
}