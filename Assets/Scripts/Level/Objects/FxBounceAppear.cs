using DG.Tweening;
using UnityEngine;
using OSB.Editor;

public class FxBounceAppear : LevelActor
{
    public FxBounceAppear() : base()
    {
        needsWarning = true;
        objParams.Add("XPos", new("rand|-8.5|8.5"));
        objParams.Add("YPos", new("rand|-5|5"));
    }

    public override void Prepare()
    {
        base.Prepare();
        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_SQUARE);
        mainObject.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
        mainObject.transform.position = new Vector3(objParams["XPos"].number.GetValue(), objParams["YPos"].number.GetValue());
        rc.renderer.color = new Color(RenderComponent.pink.r, RenderComponent.pink.g, RenderComponent.pink.b, 0.25f);
        rc.renderer.DOColor(new Color(1, 0.6933962f, 0.8485016f, 0.25f), 0.05f).SetLoops(-1, LoopType.Yoyo);
    }

    public override void ActivateAttack()
    {
        base.ActivateAttack();
        LogicHitbox.AddToLA(this);

        rc.renderer.DOKill();

        visual.transform.localScale = Vector3.one * 1.7f;
        visual.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutSine);
        rc.renderer.color = Color.white;
        rc.renderer.DOColor(RenderComponent.pink, 0.2f);

        
    }

    public override void Dispose()
    {
        if (mainObject == null)
            return;

        mainObject.transform.DOScale(0, 0.2f).SetEase(Ease.InSine).OnComplete(()=> base.Dispose());
    }
}