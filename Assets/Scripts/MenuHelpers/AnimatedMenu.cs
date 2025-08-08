using UnityEngine;

public class AnimatedMenu : MonoBehaviour
{
    public void Animate()
    {
        var animatedUI = GetComponentsInChildren<AnimatedMenuOpener>();

        foreach (AnimatedMenuOpener animated in animatedUI)
        {
            animated.Animate();
        }
    }

    public void AnimateExit()
    {
        var animatedUI = GetComponentsInChildren<AnimatedMenuOpener>();

        foreach (AnimatedMenuOpener animated in animatedUI)
        {
            animated.AnimateExit();
        }
    }
}
