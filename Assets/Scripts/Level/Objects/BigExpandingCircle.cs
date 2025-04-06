using DG.Tweening;
using UnityEngine;
using OSB.Editor;

public class BigExpandingCircle : LevelActor
{
    public BigExpandingCircle() : base()
    {
        needsWarning = true; // what do you think the default size speed should be?
        objParams.Add("circleSizeSpeed", new ActorParam(1.25f));
        objParams.Add("XPos", new(640f));
        objParams.Add("YPos", new(360f));
        objParams.Add("Size", new(100f));
    }

    public override void Prepare()
    {
        base.Prepare();
        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_CIRCLE);
        SetPosition();
        SetSize(4f, 4f);
        rc.renderer.color = new Color(rc.pink.r, rc.pink.g, rc.pink.b, 0.25f);
        
    }

    public override void ActivateAttack()
    {
        base.ActivateAttack();
        LogicHitbox.AddToLA(this);
        visual.transform.localScale = Vector3.zero;
        visual.transform.DOScale(1, 0.08f).SetEase(Ease.Linear);
        rc.SetColorToPink();
        rc.renderer.DOColor(Color.white, 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    public override void Frame()
    {
        base.Frame();
        if (hasActivated)
        {
            mainObject.transform.localScale += Vector3.one * (objParams["circleSizeSpeed"].number.GetValue()) * OSBLevelEditorStaticValues.deltaTime;
        }
    }

    public override void Dispose()
    {
        if (mainObject != null)
            rc.renderer.DOKill();
        if(mainObject != null)
        mainObject.transform.DOScale(0, 0.06f).SetEase(Ease.Linear).OnComplete(() =>
        {
            base.Dispose();
        });
    }
}

// that is adjusted using the obj params
// talking about the obj params i should finish making them