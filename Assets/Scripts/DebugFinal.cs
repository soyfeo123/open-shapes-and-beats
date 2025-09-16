using UnityEngine;
using UnityEngine.UI;

public class DebugFinal : MonoBehaviour
{
    public GameObject TheGoofy; // audio source 2, goofy cartoon villain like music
    public GameObject TheSpoopy; // audio source 1, slowed down, creepy music
    public Text YourTakingTooLong; // screen text

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TheGoofy.SetActive(true);
            TheSpoopy.SetActive(false);

            YourTakingTooLong.text = @"Hey there!
We couldn't detect a legitimate installation of JSAB on your system.
Don't worry, we're not gonna explode your computer...

It might be just a mistake, maybe Steam is in a custom folder
or the game hasn't been installed yet.
Or, you pirated the game.
Either way, we can't let you play right now.

Please make sure JSAB is installed properly through Steam before trying again.

Thanks for understanding!!
- Palo";
        }
    }
}
