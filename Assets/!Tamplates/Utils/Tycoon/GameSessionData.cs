using System;
using UnityEngine;

public class GameSessionData : MonoBehaviour
{
    public IntContainer R1Container;
    public IntContainer R2Container;
    public IntContainer R3Container;
    public IntContainer R4Container;

    public Action<IntContainer> OnChangeResource;

    public void Init(ReadOnlyGameData gameGlobalData)
    {
        R1Container = gameGlobalData.R1Container.CloneForEdit();
        R2Container = gameGlobalData.R2Container.CloneForEdit();
        R3Container = gameGlobalData.R3Container.CloneForEdit();
        R4Container = gameGlobalData.R4Container.CloneForEdit();

        OnChangeResource = null;
    }
}