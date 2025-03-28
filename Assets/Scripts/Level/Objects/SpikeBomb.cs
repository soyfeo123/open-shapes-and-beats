using UnityEngine;
using OSB.Editor;
using DG.Tweening;

public class SpikeBomb : LevelActor
{
    public SpikeBomb() : base()
    {
        objParams["Duration"].number.expression = "100";
        objParams.Add("XPos", new("rand|2|4"));
        objParams.Add("YPos", new("rand|-3|3"));
        objParams.Add("numberOfSpikes", new(8));
        needsWarning = true;
    }

    public override void Prepare()
    {
        base.Prepare();

        RenderComponent.AddToLA(this, LevelSpawnSprites.BOMB);
        rc.SetColorToPink();
        mainObject.transform.position = new Vector3(8f, Random.Range(-5f, 5f));
        mainObject.transform.localScale = Vector3.zero;

        LogicSpin youSpinMe = visual.AddComponent<LogicSpin>();
        youSpinMe.speed = 1000f;

        LogicHitbox.AddToLA(this);

        mainObject.transform.DOMove(new Vector3(objParams["XPos"].number.GetValue(), objParams["YPos"].number.GetValue()), objParams["Warning"].number.GetValue() * 0.001f).SetEase(Ease.OutExpo);
        mainObject.transform.DOScale(Vector3.one, objParams["Warning"].number.GetValue() * 0.001f);
    }

    public override void ActivateAttack()
    {
        base.ActivateAttack();
        OSBCamera.Singleton.Flash(0.03f);
        GameProjectileManager.CreateCircleProjectiles(mainObject.transform.position, (int)objParams["numberOfSpikes"].number.GetValue());
        Dispose();
    }
}
