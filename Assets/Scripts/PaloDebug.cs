using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSB.Editor;

public class PaloDebug : OSBInit
{
    public override void Init()
    {
        base.Init();
        //MainLevelManager.Singleton.LoadLevel(MainGameLevels.I_SAID_MEOW);

        Debug.Log("64: " + PaloUtils.ConvertExpression("64", new PaloUtils.ExpressionVariables("i", 0f)));
        Debug.Log("64+75: " + PaloUtils.ConvertExpression("64+74", new PaloUtils.ExpressionVariables("i", 0f)));
        Debug.Log("12*7: " + PaloUtils.ConvertExpression("12*7", new PaloUtils.ExpressionVariables("i", 0f)));
        Debug.Log("[i]*5: " + PaloUtils.ConvertExpression("[i]*5", new PaloUtils.ExpressionVariables("i", 5f)));
    }
}
