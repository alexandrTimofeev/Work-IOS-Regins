using System.Collections;
using UnityEngine;

public class StepMainGameState : GameStepHoldUpdate<IPlayGameMainGameState>
{
    public override string Id => "StepMainGameState";

    protected override IEnumerator OnExecute(GameLoopContext context)
    {
        context.RequestStepTransition();
        yield return null;
    }
}


//-----------------------------------------

public interface IPlayGameMainGameState : IPlayGameBase { new IEnumerator PlayGame(); }