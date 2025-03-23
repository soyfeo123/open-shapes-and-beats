using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSB.Editor;
using DG.Tweening;

// TO SELF: unity apparently takes in radians? when did unity do that


/// <summary>
/// i can't get enough of spamming these guys
/// to future self: add these in every single damn level i make
/// </summary>
///

// i really like how organized i made the system like i can make objects quickly
// i don't like how certain games use prefabs instead of stuff like this

public class FlyingProjectile : LevelActor
{
    LogicSpin visualGoRoundRound;

    float direction;
    float randomSpeed;

    GameObject innerCircle;

    public FlyingProjectile() : base()
    {
        needsWarning = false;
    }

    public override void Prepare()
    {
        base.Prepare();
        mainObject.transform.localScale = Vector3.one * 0.15f;
        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_SQUARE);
        innerCircle = new GameObject("CoolCircleBecauseItLooksGood");
        innerCircle.transform.parent = visual.transform;
        innerCircle.AddComponent<SpriteRenderer>().sprite = LevelSpawnSprites.GENERIC_CIRCLE;
        innerCircle.transform.position = new Vector3(0,0,0.1f);
        innerCircle.transform.localScale = Vector3.one * 5;
        innerCircle.transform.DOScale(0, 0.35f).SetEase(Ease.Linear);
        innerCircle.GetComponent<SpriteRenderer>().DOColor(RenderComponent.pink,0.3f).SetEase(Ease.Linear);

        rc.SetColorToPink();

        visualGoRoundRound = visual.AddComponent<LogicSpin>();
        visualGoRoundRound.speed = 600;
        visualGoRoundRound.startAtRandomDir = false;
        LogicHitbox.AddToLA(this);

        int randomValue = Random.Range((int)0, (int)3);
        mainObject.transform.position = new Vector3(9, randomValue == 0 ? -3f : (randomValue == 1 ? 0 : (randomValue == 2 ? 3f : 0)), 0);
        direction = Random.Range(-30, -122) * Mathf.Deg2Rad; // ??? // NO WAY THAT ACTUALLY WORKED?????
        randomSpeed = Random.Range(4f, 6f);
    }

    public override void Frame()
    {
        base.Frame();
        if(hasPrepared)
        MoveBy(randomSpeed * OSBLevelEditorStaticValues.deltaTime * Mathf.Sin(direction), randomSpeed * OSBLevelEditorStaticValues.deltaTime * Mathf.Cos(direction));
    }
}
