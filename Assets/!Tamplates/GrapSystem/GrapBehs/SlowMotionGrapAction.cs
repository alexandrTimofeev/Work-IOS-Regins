using System;

[Serializable]
public class SlowMotionGrapAction : GrappableObjectBehaviourAction
{
    public float Duration;

    public override void Work(BonusActionContext context) { }
}