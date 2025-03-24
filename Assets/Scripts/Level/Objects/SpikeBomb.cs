using UnityEngine;
using OSB.Editor;
using DG.Tweening;

public class SpikeBomb : LevelActor
{
    public SpikeBomb() : base()
    {
        objParams["Duration"].number.expression = "100";
        objParams.Add("XPos", new("rand|4|0"));
        objParams.Add("YPos", new("rand|-3|3"));
        objParams.Add("numberOfSpikes", new(8));
    }

    public override void Prepare()
    {
        base.Prepare();

        RenderComponent.AddToLA(this, LevelSpawnSprites.BOMB);
        rc.SetColorToPink();
        mainObject.transform.position = new Vector3(8f, Random.Range(-5f, 5f));

        LogicSpin youSpinMe = visual.AddComponent<LogicSpin>();
        youSpinMe.speed = 1000f;

        LogicHitbox.AddToLA(this);

        mainObject.transform.DOMove(new Vector3(objParams["XPos"].number.Value, objParams["YPos"].number.Value), objParams["Warning"].number.Value * 0.001f).SetEase(Ease.OutExpo);
    }

    public override void ActivateAttack()
    {
        base.ActivateAttack();
        GameProjectileManager.CreateCircleProjectiles(mainObject.transform.position, (int)objParams["numberOfSpikes"].number.Value);
        Dispose();
    }
}
