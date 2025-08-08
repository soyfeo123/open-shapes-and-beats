using DG.Tweening;
using TMPro;
using UnityEngine;

public class MenuNotifications : MBSingletonDestroy<MenuNotifications>
{
    private GameObject m_notificationPrefab;

    private void Start()
    {
        
    }

    public void Show(string title, string body, string caption = "")
    {
        if(m_notificationPrefab is null) m_notificationPrefab = Resources.Load<GameObject>("Prefabs/MenuNotification");

        GameObject clone = Instantiate(m_notificationPrefab, transform);
        clone.transform.localScale = new Vector3(0, 1, 1);
        clone.transform.DOScaleX(1f, 0.3f).SetEase(Ease.OutExpo);

        clone.transform.Find("Top").GetComponent<TextMeshProUGUI>().text = caption;
        clone.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = title;
        clone.transform.Find("Body").GetComponent<TextMeshProUGUI>().text = body;

        Utils.Timer(5f, () =>
        {
            clone.transform.DOScaleX(0f, 0.3f).SetEase(Ease.InExpo).OnComplete(() =>
            {
                Destroy(clone);
            });

            clone.transform.DOScaleY(0f, 0.3f).SetEase(Ease.InExpo);
        });
    }
}
