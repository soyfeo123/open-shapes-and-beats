using DG.Tweening;
using UnityEngine;
using OSB.Editor;

public class BigExpandingCircle : LevelActor
{
    public BigExpandingCircle() : base()
    {
        needsWarning = true; // what do you think the default size speed should be?
        objParams.Add("circleSizeSpeed", 0.48f);
    }

    public override void Prepare()
    {
        base.Prepare();
        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_CIRCLE);
        mainObject.transform.localScale = Vector3.one * 4;
        mainObject.transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-5, 5));
        rc.renderer.color = new Color(RenderComponent.pink.r, RenderComponent.pink.g, RenderComponent.pink.b, 0.25f);
        
    }

    public override void ActivateAttack()
    {
        base.ActivateAttack();
        LogicHitbox.AddToLA(this);
        visual.transform.localScale = Vector3.zero;
        visual.transform.DOScale(1, 0.08f).SetEase(Ease.Linear);
        rc.SetColorToPink();
    }

    public override void Frame()
    {
        base.Frame();
        if (hasActivated)
        {
            mainObject.transform.localScale += Vector3.one * ((float)objParams["circleSizeSpeed"]) * Time.deltaTime;
        }
    }

    public override void Dispose()
    {
        mainObject.transform.DOScale(0, 0.06f).SetEase(Ease.Linear).OnComplete(() =>
        {
            base.Dispose();
        });
    }
}

// that is adjusted using the obj params
// talking about the obj params i should finish making them