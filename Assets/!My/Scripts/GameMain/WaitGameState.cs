using System.Collections;
using UnityEngine;

public class WaitGameState : GameStepBase
{
    public override string Id => "Wait";

    protected override IEnumerator OnExecute(GameLoopContext context)
    {
        Debug.Log("Start [WaitGameState]");
        yield return new WaitForSeconds(0.3f);
    }
}