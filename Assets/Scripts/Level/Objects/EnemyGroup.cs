using UnityEngine;
using OSB.Editor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Linq;

public class EnemyGroup : LevelActor
{
    int index;
    List<LevelActor> actorList = new();

    public EnemyGroup() : base()
    {
        needsWarning = false;
        objParams.Add("Enemy", new("FxBounceAppear", "FxBounceAppear"));
        objParams.Add("Amount", new(10));

        
    }

    public override void Prepare()
    {
        mainObject = new GameObject(Utils.GenerateUniqueName("Spawn"));
        if (OSBLevelEditorStaticValues.IsInEditor)
            MainLevelManager.Singleton.onFrame.AddListener(Frame);

        

        actorList.Clear();

        for(int i = 0; i < objParams["Amount"].number.GetValue(); i++)
        {
            LevelActor actor = Activator.CreateInstance(Type.GetType(objParams["Enemy"].text)) as LevelActor;

            if(actor == null)
            {
                continue;
            }

            foreach(KeyValuePair<string, ActorParam> ownParam in objParams)
            {
                if(ownParam.Key.Length > 6 && ownParam.Key.Substring(0, 6) == "ENEMY_")
                {
                    Debug.Log("Found param for clone!!!");

                    actor.objParams[ownParam.Key.Substring(6)].number.expression = Utils.CloneObject(ownParam.Value.number.expression);
                    actor.objParams[ownParam.Key.Substring(6)].text = Utils.CloneObject(ownParam.Value.text);
                }
            } 

            foreach(KeyValuePair<string, ActorParam> param in actor.objParams)
            {
                param.Value.number.Overrides.Add(new("i", i));
            }
            actorList.Add(actor);
        }
    }

    public override void Frame()
    {
        base.Frame();

        List<LevelActor> actorsToKill = new();
        foreach(LevelActor actor in actorList)
        {
            if (MainLevelManager.Singleton.msTime >= (actor.objParams["Time"].number.GetValue() - actor.objParams["Warning"].number.GetValue()) + objParams["Time"].number.GetValue() && actor.needsWarning && !actor.hasPrepared)
            {
                Debug.Log("prepared");
                actor.Prepare();
                actor.hasPrepared = true;
            }
            if(MainLevelManager.Singleton.msTime >= (actor.objParams["Time"].number.GetValue()) + objParams["Time"].number.GetValue() && !actor.needsWarning && !actor.hasActivated)
            {
                Debug.Log("prepared (no warning!)");
                actor.Prepare();
                actor.hasActivated = true;
            }
            if (MainLevelManager.Singleton.msTime >= (actor.objParams["Time"].number.GetValue()) + objParams["Time"].number.GetValue() && actor.needsWarning && !actor.hasActivated)
            {
                Debug.Log("activated attack");
                actor.ActivateAttack();
                actor.hasActivated = true;
            }
            if (MainLevelManager.Singleton.msTime >= actor.objParams["Time"].number.GetValue() + actor.objParams["Duration"].number.GetValue() + objParams["Time"].number.GetValue() && !actor.shouldBeDisposed && actor.needsWarning)
            {
                Debug.Log("disposed");
                actor.Dispose();
                actor.shouldBeDisposed = true;
                actorsToKill.Add(actor);
            }
        }

        actorList = actorList.Where(x => !actorsToKill.Contains(x)).ToList();

        if(actorList == null || actorList.Count <= 0)
        {
            Dispose();
        }
    }

    public override void Dispose()
    {
        Debug.Log("disposing enemy list");

        foreach(LevelActor actor in actorList)
        {
            actor.Dispose();
        }

        actorList.Clear();
        
        base.Dispose();
    }

    public void OnEnemyFieldChanged()
    {
        Debug.Log("called update");

        objParams = objParams.Where(x => !x.Key.StartsWith("ENEMY_")).ToDictionary(x => x.Key, x => x.Value);

        Type enemyType = Type.GetType(objParams["Enemy"].text);
        if(enemyType == null)
        {
            return;
        }

        LevelActor instance = Activator.CreateInstance(enemyType) as LevelActor;
        foreach(KeyValuePair<string, ActorParam> param in instance.objParams)
        {
            objParams.Add("ENEMY_" + param.Key, param.Value);
        }
    }
}