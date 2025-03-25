using UnityEngine;
using OSB.Editor;
using DG.Tweening;

// laser direction enum:
// 0: top to bottom
// 1: bottom to top
// 2: left to right
// 3: right to left

public class Laser : LevelActor
{
    public Laser() : base()
    {
        objParams.Add("laserDirection", new(0));
        objParams.Add("position (0-100)", new(50));
        objParams.Add("thickness", new(3f));
    }

    float finalPosition = 0;

    float getPositionX()
    {
        return -8.5f + (finalPosition * 17f / 100f);
    }

    public override void Prepare()
    {
        base.Prepare();
        finalPosition = objParams["position (0-100)"].number.Value;
        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_SQUARE);
        mainObject.transform.localScale = new Vector3(0, 10, 1f);
        
        mainObject.transform.DOScaleX(objParams["thickness"].number.Value * 0.1f, objParams["Warning"].number.Value * 0.001f);
        mainObject.transform.position = new Vector3(getPositionX(), 0, 0f);
        rc.SetColorToPink();
        rc.renderer.color = new Color(RenderComponent.pink.r, RenderComponent.pink.g, RenderComponent.pink.b, 0.25f);
    }

    public override void ActivateAttack()
    {
        base.ActivateAttack();
        rc.renderer.color = Color.white;
        mainObject.transform.position = new Vector3(getPositionX(), 10, 0f);
        mainObject.transform.DOMoveY(0f, 0.1f).SetEase(Ease.OutSine);
        rc.renderer.DOColor(RenderComponent.pink, 0.2f);
        mainObject.transform.localScale = new Vector3(0, 10, 1f);

        mainObject.transform.DOScaleX(objParams["thickness"].number.Value * 0.1f, 0.1f).SetEase(Ease.OutSine);

        LogicHitbox.AddToLA(this);
    }

    public override void Dispose()
    {

        rc.renderer.DOFade(0, 0.05f);
        mainObject.transform.DOScaleX(0, 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            base.Dispose();
        });
        
    }
}