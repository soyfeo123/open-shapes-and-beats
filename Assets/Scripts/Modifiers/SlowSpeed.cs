using UnityEngine;

public class SlowSpeed : Modifier
{
    // The argument in base(x) is the modifier's multiplier.
    public SlowSpeed() : base(0.7f) {  }
    
    public override void ReadyToModify()
    {
        base.ReadyToModify();
        MainLevelManager.Singleton.levelMusic.audioSrc.pitch -= 0.2f;
    }
}