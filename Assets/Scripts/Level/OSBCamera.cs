using UnityEngine;
using DG.Tweening;
using OSB.Editor;
using System;

public class OSBCamera : MBSingletonDestroy<OSBCamera>
{
    Vector3 defaultCamPos = new Vector3(0, 0, -50f);
    GameObject flash;

    private Vector2 m_cameraOffset = new Vector2(0, 0);
    private Vector2 m_cameraPosition = new Vector2();

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

    public void Update()
    {
        transform.position = new Vector3(m_cameraPosition.x + m_cameraOffset.x, m_cameraPosition.y + m_cameraOffset.y, defaultCamPos.z);
    }

    public void CameraMoveOffset(float x, float y, float duration)
    {
        TweenOffset(new Vector2(x, y), duration * 0.5f, () =>
        {
            TweenOffset(Vector2.zero, duration * 0.5f);
        });
    }

    public void CameraMovePerm(float x, float y, float duration, Ease ease)
    {
        DOTween.To(() => m_cameraPosition, n => m_cameraPosition = n, new Vector2(x, y), duration).SetEase(ease);
    }

    public void Flash(float duration)
    {
        if(!SettingManager.Singleton.GetSetting("Gameplay", "NoFlash").boolValue)
            flash.SetActive(true);
        // hacky af but it works so don't mess with it
        // - every programmer in the world
        flash.transform.DOLocalMove(Vector3.zero, duration).OnComplete(() =>
        {
            flash.SetActive(false);
        });
    }

    public void CameraRotationPerm(float direction, float duration, Ease ease)
    {
        transform.DORotate(new Vector3(0, 0, direction), duration).SetEase(ease);
    }

    private void TweenOffset(Vector2 endValue, float duration, Action onComplete = null)
    {
        DOTween.To(() => m_cameraOffset, n => m_cameraOffset = n, endValue, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}

public enum CameraDirection
{
    Top, Bottom, Left, Right
}