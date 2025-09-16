using UnityEngine;
using System.Collections;
using TMPro;

public class Warning : MonoBehaviour
{
    public GameObject text;
    private IEnumerator Start()
    {
        text.SetActive(false);
        SettingMenuManager.ApplyAllSettingsAtOnce();
        yield return new WaitForSeconds(5f);
        text.SetActive(true);
    }
    private void Update()
    {
        if (text.activeInHierarchy && Input.anyKeyDown)
        {
            OSBScenes.LoadGameplayScene();
        }
    }
}
