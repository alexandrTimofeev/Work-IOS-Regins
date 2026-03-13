using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ResourceManager
{
    public event Action<string> OnDownfullResource;
    public event Action<string> OnOverfullResource;

    public void Init()
    {
        GameG.SessionData.R1Container.OnChangeValue += (value) => InterfaceManager.BarMediator.ShowForID("R1", value);
        GameG.SessionData.R2Container.OnChangeValue += (value) => InterfaceManager.BarMediator.ShowForID("R2", value);
        GameG.SessionData.R3Container.OnChangeValue += (value) => InterfaceManager.BarMediator.ShowForID("R3", value);
        GameG.SessionData.R4Container.OnChangeValue += (value) => InterfaceManager.BarMediator.ShowForID("R4", value);
        InterfaceManager.BarMediator.ShowForID("R1", GameG.SessionData.R1Container.Value);
        InterfaceManager.BarMediator.ShowForID("R2", GameG.SessionData.R2Container.Value);
        InterfaceManager.BarMediator.ShowForID("R3", GameG.SessionData.R3Container.Value);
        InterfaceManager.BarMediator.ShowForID("R4", GameG.SessionData.R4Container.Value);  

        GameG.SessionData.R1Container.OnDownfullValue += (d) => LoseDownfall(GameG.SessionData.R1Container.Title);
        GameG.SessionData.R2Container.OnDownfullValue += (d) => LoseDownfall(GameG.SessionData.R2Container.Title);
        GameG.SessionData.R3Container.OnDownfullValue += (d) => LoseDownfall(GameG.SessionData.R3Container.Title);
        GameG.SessionData.R4Container.OnDownfullValue += (d) => LoseDownfall(GameG.SessionData.R4Container.Title);

        GameG.SessionData.R1Container.OnOverfullValue += (d) => LoseOverfull(GameG.SessionData.R1Container.Title);
        GameG.SessionData.R2Container.OnOverfullValue += (d) => LoseOverfull(GameG.SessionData.R2Container.Title);
        GameG.SessionData.R3Container.OnOverfullValue += (d) => LoseOverfull(GameG.SessionData.R3Container.Title);
        GameG.SessionData.R4Container.OnOverfullValue += (d) => LoseOverfull(GameG.SessionData.R4Container.Title);
    }

    public void AddOrRemoveResource (int R1Delta, int R2Delta, int R3Delta, int R4Delta)
    {
        GameG.SessionData.R1Container.AddValue(R1Delta);
        GameG.SessionData.R2Container.AddValue(R2Delta);
        GameG.SessionData.R3Container.AddValue(R3Delta);
        GameG.SessionData.R4Container.AddValue(R4Delta);

        if (R1Delta != 0)
            CreateFlyingText(R1Delta, InterfaceManager.BarMediator.GetBarsID("R1")[0].transform.position);
        if (R2Delta != 0)
            CreateFlyingText(R2Delta, InterfaceManager.BarMediator.GetBarsID("R2")[0].transform.position);
        if (R3Delta != 0)
            CreateFlyingText(R3Delta, InterfaceManager.BarMediator.GetBarsID("R3")[0].transform.position);
        if (R4Delta != 0)
            CreateFlyingText(R4Delta, InterfaceManager.BarMediator.GetBarsID("R4")[0].transform.position);
    }

    public void SetResources(int r1, int r2, int r3, int r4)
    {
        GameG.SessionData.R1Container.SetValue(r1);
        GameG.SessionData.R2Container.SetValue(r2);
        GameG.SessionData.R3Container.SetValue(r3);
        GameG.SessionData.R4Container.SetValue(r4);
    }

    private void CreateFlyingText (int valueDelta, Vector3 point)
    {
        string p = valueDelta > 0 ? "<Color=green>+" : (valueDelta == 0 ? "<Color=grey>+" : "<Color=red>");
        TextScoreUpLR textScoreUpLR = InterfaceManager.CreateFlyingText($"{p}{valueDelta}</Color>", Color.white, point, null, true);

        //textScoreUpLR.transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);
        //textScoreUpLR.transform.DOScale(1f, 15f);
    }

    private void LoseDownfall(string title)
    {
        OnDownfullResource?.Invoke(title);
    }

    private void LoseOverfull(string title)
    {
        OnOverfullResource?.Invoke(title);
    }
}