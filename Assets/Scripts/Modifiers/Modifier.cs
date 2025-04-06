using UnityEngine;

public class Modifier
{
    public static float GlobalMultiplier = 1;
    float multiplier_;

    public Modifier(float multiplier)
    {
        multiplier_ = multiplier - 1f;
    }

    public void ApplyMultipler()
    {
        GlobalMultiplier += multiplier_;
    }

    public virtual void ReadyToModify()
    {
        Debug.Log(this.GetType().Name + " - modifier");
    }
}