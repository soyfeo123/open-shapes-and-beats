using UnityEngine;

public class TripleSpeed : Modifier
{
    // The argument in base(x) is the modifier's multiplier.
    public TripleSpeed() : base(2.0f) {  }
    
    public override void ReadyToModify()
    {
        base.ReadyToModify();
        MainLevelManager.Singleton.levelMusic.audioSrc.pitch += 0.4f;
    }
}