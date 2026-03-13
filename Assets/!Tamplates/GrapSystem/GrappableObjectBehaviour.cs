using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Базовый класс всех захватываемых объектов.
/// </summary>
[CreateAssetMenu(fileName = "GrappableObjectBehaviour", menuName = "SGames/GrapSystem/GrappableObjectBehaviour")]
public class GrappableObjectBehaviour : ScriptableObject
{
    public string ID;
    [SerializeReference]
    public List<GrappableObjectBehaviourAction> behaviourActions;

    public virtual void Work(BonusActionContext context)
    {
        if (context == null)
            return;

        foreach (var action in behaviourActions)        
            action.Work(context);        
    }

    public virtual BonusActionContext GetContext(GrapObject grapObject)
    {
        return new BonusActionContext { GrapObject = grapObject };
    }
}

[System.Serializable]
public abstract class GrappableObjectBehaviourAction
{
    public string Title;

    public abstract void Work(BonusActionContext context);
}

public class BonusActionContext
{
       public GrapObject GrapObject;
}