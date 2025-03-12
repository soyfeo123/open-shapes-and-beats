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

    GameObject innerCircle;

    public FlyingProjectile() : base()
    {
        
    }

    public override void Prepare()
    {
        base.Prepare();
        mainObject.transform.localScale = Vector3.one * 0.15f;
        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_SQUARE);
        innerCircle = new GameObject("CoolCircleBecauseItLooksGood");
        innerCircle.transform.parent = visual.transform;
        innerCircle.AddComponent<SpriteRenderer>().sprite = LevelSpawnSprites.GENERIC_CIRCLE;
        innerCircle.transform.localScale = Vector3.one * 4;
        innerCircle.transform.DOScale(0, 0.5f).SetEase(Ease.Linear);
        innerCircle.GetComponent<SpriteRenderer>().DOColor(RenderComponent.pink,0.5f).SetEase(Ease.Linear);

        rc.SetColorToPink();

        visualGoRoundRound = visual.AddComponent<LogicSpin>();
        visualGoRoundRound.speed = 410;
        visualGoRoundRound.startAtRandomDir = false;
        LogicHitbox.AddToLA(this);

        mainObject.transform.position = new Vector3(9, Random.Range(-4f, 4f), 0);
        direction = Random.Range(-30, -122) * Mathf.Deg2Rad; // ??? // NO WAY THAT ACTUALLY WORKED?????

        //rc.AddVisibilityRenderer();
        //Debug.Log(direction);
    }

    public override void Frame()
    {
        MoveBy(5f * Time.deltaTime * Mathf.Sin(direction), 5f * Time.deltaTime * Mathf.Cos(direction));
    }
}
