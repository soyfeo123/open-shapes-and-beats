using UnityEngine;
using OSB.Editor;
using DG.Tweening;

// laser direction enum:
// 0: top to bottom
// 1: bottom to top
// 2: left to right
// 3: right to left

// 18: full width
// 10: full height

public class Laser : LevelActor
{
    public Laser() : base()
    {
        needsWarning = true;

        objParams.Add("laserDirection", new("rand|0|3"));
        objParams.Add("position (0-100)", new("rand|0|100"));
        objParams.Add("thickness", new(3f));
    }

    float finalPosition = 0;
    int finalDirection = 0;

    Vector3 getPosition()
    {
        float necessaryAxis = 0;
        if(finalDirection == 0 || finalDirection == 1)
        {
            necessaryAxis = Mathf.Lerp(-9f, 9f, finalPosition * 0.01f);
        }
        if(finalDirection == 2 || finalDirection == 3)
        {
            necessaryAxis = Mathf.Lerp(-5, 5, finalPosition * 0.01f);
        }
        Vector3 newPos = new Vector3(finalDirection == 0 || finalDirection == 1 ? necessaryAxis : 0, finalDirection == 3 || finalDirection == 2 ? necessaryAxis : 0, 0);
        return newPos;
    }

    

    public override void Prepare()
    {
        base.Prepare();
        finalPosition = objParams["position (0-100)"].number.GetValue();
        finalDirection = Mathf.Clamp(Mathf.RoundToInt(objParams["laserDirection"].number.GetValue()), 0, 3);
        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_SQUARE);


        mainObject.transform.localScale = new Vector3(finalDirection == 0 || finalDirection == 1 ? 0 : 18, finalDirection == 2 || finalDirection == 3 ? 0 : 10, 1f);

        if (finalDirection == 0 || finalDirection == 1)
        {
            mainObject.transform.DOScaleX(objParams["thickness"].number.GetValue() * 0.1f, objParams["Warning"].number.GetValue() * 0.001f);
        }
        else
        {
            mainObject.transform.DOScaleY(objParams["thickness"].number.GetValue() * 0.1f, objParams["Warning"].number.GetValue() * 0.001f);
        }
        mainObject.transform.position = getPosition();
        rc.SetColorToPink();
        rc.renderer.color = new Color(RenderComponent.pink.r, RenderComponent.pink.g, RenderComponent.pink.b, 0.25f);
    }

    public override void ActivateAttack()
    {
        base.ActivateAttack();
        rc.renderer.color = Color.white;
        mainObject.transform.position = getPosition();



        if (finalDirection == 0 || finalDirection == 1)
        {
            if(finalDirection == 0)
            {
                mainObject.transform.position += new Vector3(0, 10f, 0);

                OSBCamera.Singleton.CameraMove(0, 0.1f, 0.2f);
            }
            else
            {
                mainObject.transform.position -= new Vector3(0, 10f, 0);
                OSBCamera.Singleton.CameraMove(0, -0.1f, 0.2f);
            }
            mainObject.transform.DOMoveY(0f, 0.1f).SetEase(Ease.OutSine);
        }
        else
        {
            if (finalDirection == 2)
            {
                mainObject.transform.position += new Vector3(18f, 0, 0);
                OSBCamera.Singleton.CameraMove(0.1f, 0, 0.2f);
            }
            else
            {
                mainObject.transform.position -= new Vector3(18f, 0, 0);
                OSBCamera.Singleton.CameraMove(-0.1f, 0, 0.2f);
            }
            mainObject.transform.DOMoveX(0f, 0.1f).SetEase(Ease.OutSine);
        }
        rc.renderer.DOColor(RenderComponent.pink, 0.2f);
        mainObject.transform.localScale = new Vector3(finalDirection == 0 || finalDirection == 1 ? 0 : 18, finalDirection == 2 || finalDirection == 3 ? 0 : 10, 1f);

        if (finalDirection == 0 || finalDirection == 1)
        {
            mainObject.transform.DOScaleX(objParams["thickness"].number.GetValue() * 0.1f, 0.1f).SetEase(Ease.OutSine);
        }
        else
        {
            mainObject.transform.DOScaleY(objParams["thickness"].number.GetValue() * 0.1f, 0.1f).SetEase(Ease.OutSine);
        }

        LogicHitbox.AddToLA(this);
    }

    public override void Dispose()
    {
        if (mainObject == null) return;

        rc.renderer.DOFade(0, 0.05f);
        mainObject.transform.DOScaleX(0, 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            base.Dispose();
        });
        
    }
}