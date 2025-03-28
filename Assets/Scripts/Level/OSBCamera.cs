using UnityEngine;
using DG.Tweening;
using OSB.Editor;

public class OSBCamera : MBSingletonDestroy<OSBCamera>
{
    Vector3 defaultCamPos = new Vector3(0, 0, -2f);
    GameObject flash;

    private void Start()
    {
        transform.position = defaultCamPos;
        flash = new GameObject("Flash", typeof(SpriteRenderer));
        flash.GetComponent<SpriteRenderer>().sprite = LevelSpawnSprites.GENERIC_SQUARE;
        flash.GetComponent<SpriteRenderer>().color = new Color(0.3113208f, 0.2936074f, 0.2768779f);
        flash.GetComponent<SpriteRenderer>().sortingOrder = 5;
        flash.transform.localScale = new Vector3(19f, 11f, 1f);
        flash.SetActive(false);
    }

    public void CameraMove(float x, float y, float duration)
    {
        transform.DOMoveX(x, duration / 2).OnComplete(() =>
        {
            transform.DOMove(defaultCamPos, duration/ 2);
        });
        transform.DOMoveY(y, duration / 2).OnComplete(() =>
        {
            transform.DOMove(defaultCamPos, duration / 2);
        });
    }

    public void Flash(float duration)
    {
        flash.SetActive(true);
        // hacky af but it works so don't mess with it
        // - every programmer in the world
        flash.transform.DOLocalMove(Vector3.zero, duration).OnComplete(() =>
        {
            flash.SetActive(false);
        });
    }
}

public enum CameraDirection
{
    Top, Bottom, Left, Right
}