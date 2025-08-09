using UnityEngine;
using OSB.Editor;

public class SpawnMsg : LevelActor
{
    public SpawnMsg() : base()
    {
        needsWarning = false;
        objParams.Add("Message", new("", ""));
    }

    public override void Prepare()
    {
        base.Prepare();
        Debug.Log(GetTParam("Message"));
        Messages?.Invoke(GetTParam("Message"));
    }
}