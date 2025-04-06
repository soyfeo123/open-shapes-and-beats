using UnityEngine;

public class DoubleSpeed : Modifier
{
    // The argument in base(x) is the modifier's multiplier.
    public DoubleSpeed() : base(1.5f) {  }
    
    public override void ReadyToModify()
    {
        base.ReadyToModify();
        MainLevelManager.Singleton.levelMusic.audioSrc.pitch += 0.2f;
    }
}