using TMPro;
using UnityEngine;
using DG.Tweening;

public static class SongIndicator
{
    public static void Show(string line1, string line2, string line3, string charter)
    {
        GameObject indicator = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/LevelStuff/SongName"));
        indicator.transform.Find("Container/Name").GetComponent<TextMeshProUGUI>().text = line1;
        indicator.transform.Find("Container/MiddleLine").GetComponent<TextMeshProUGUI>().text = line2;
        indicator.transform.Find("Container/Artist").GetComponent<TextMeshProUGUI>().text = line3;
        indicator.transform.Find("Container/Charter").GetComponent<TextMeshProUGUI>().text = "charted by " + charter;

        GameObject.Destroy(indicator, 4.5f);
    }
}
