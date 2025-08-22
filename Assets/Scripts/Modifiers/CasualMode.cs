using UnityEngine;

public class CasualMode : Modifier
{
    // The argument in base(x) is the modifier's multiplier.
    public CasualMode() : base(0.5f) {  }
    
    public override void ReadyToModify()
    {
        base.ReadyToModify();
        ThePlayersParents.Singleton.PlayerOnScreen.MaxLives = 6;
        ThePlayersParents.Singleton.PlayerOnScreen.Lives = 6;
    }
}