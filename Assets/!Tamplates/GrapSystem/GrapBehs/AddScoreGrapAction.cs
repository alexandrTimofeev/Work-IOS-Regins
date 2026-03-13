using System;

[Serializable]
public class AddScoreGrapAction : GrappableObjectBehaviourAction
{
    public int AddScore;

    public override void Work(BonusActionContext context) { }
}