using UnityEngine;

public class BonusExecuter : MonoBehaviour
{
    public void Init()
    {
        GrapCollider.Mediator = GameG.GrappableObjectMediator;

        GameG.GrappableObjectMediator.Subscribe<AddLifeGrapAction>((action, grapObject) =>
        {
            //GameG.SessionData.LifeContainer.AddValue(action.AddLife);
        });

        GameG.GrappableObjectMediator.Subscribe<AddScoreGrapAction>((action, grapObject) =>
        {
            GameG.ScoreSys.AddScore(action.AddScore, grapObject.GrapObject.transform.position);
        });

        GameG.GrappableObjectMediator.Subscribe<InvictibleGrapAction>((action, grapObject) =>
        {
            //GameG.Player.Invictible(action.Duration);
        });

        GameG.GrappableObjectMediator.Subscribe<SlowMotionGrapAction>((action, grapObject) =>
        {
            //GameG.Player.SlowMo(action.Duration);
        });

        GameG.GrappableObjectMediator.Subscribe<InfinityFlyGrapAction>((action, grapObject) =>
        {
            //GameG.Player.SuperHot(action.Duration);
        });
    }
} 
