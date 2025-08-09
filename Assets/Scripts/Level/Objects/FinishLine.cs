using UnityEngine;
using OSB.Editor;

public class FinishLine : LevelActor
{
    public FinishLine() : base()
    {
        needsWarning = false;
    }

    public override void Prepare()
    {
        base.Prepare();

        Debug.Log("finish line cloned");

        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_SQUARE);

        mainObject.transform.localScale = new Vector3(0.05f, 10f, 1f);
        mainObject.transform.position = new Vector3(9f, 0, 0);
        rc.renderer.color = new Color(0f, 1f, 1f);

        LogicHitbox.AddToLA(this);
        hitbox.tag = "FinishLine";

        
    }

    public override void Frame()
    {
        

        base.Frame();

        MoveBy(-2.5f, 0);
    }
}