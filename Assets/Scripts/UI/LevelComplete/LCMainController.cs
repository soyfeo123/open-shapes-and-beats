using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LCMainController : MonoBehaviour
{
    public GameObject levelCompleteText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI bpPerPrize;
    public TextMeshProUGUI bpCounter;
    public int currentBPCount;
    public List<GameObject> objsToMoveToBeat;
    Music music;

    bool canMoveOn = false;

    void Start()
    {
        HideBPAmount();
        music = new Music();
        music.Clip = Resources.Load<AudioClip>("Sound/MUS/mus_levelcomplete");
        music.Play();
        music.audioSrc.loop = true;

        foreach (GameObject obj in objsToMoveToBeat)
        {
            
            Vector3 org = obj.transform.localPosition;
            obj.transform.localPosition += new Vector3(0, 15f);

            
            obj.transform.DOLocalMoveY(org.y, 0.4615f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Restart);
        }

        levelCompleteText.transform.localScale = Vector3.one * 100f;
        levelCompleteText.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutExpo);
        rankText.alpha = 0;

        LevelRank rank = LevelRank.C;

        if(OSB_Player.numberOfRespawns == 0 && ThePlayersParents.Singleton.PlayerOnScreen.Lives >= ThePlayersParents.Singleton.PlayerOnScreen.MaxLives)
        {
            rank = LevelRank.S;
        }
        else if (OSB_Player.numberOfRespawns == 0)
        {
            rank = LevelRank.A;
        }
        else if(OSB_Player.numberOfRespawns == 1)
        {
            rank = LevelRank.B;
        }
        else
        {
            rank = LevelRank.C;
        }

        rankText.text = rank.ToString();

        Utils.Timer(2.5f, () =>
        {
            rankText.transform.localScale = Vector3.one * 7.5f;
            rankText.alpha = 0f;
            rankText.DOFade(1f, 0.8f).SetEase(Ease.InExpo);
            rankText.transform.DOScale(0.7f, 0.8f).SetEase(Ease.InExpo).onComplete += delegate
            {
                levelCompleteText.GetComponent<RectTransform>().DOAnchorPosY(220, 0.5f).SetEase(Ease.OutExpo);
                rankText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutExpo);

                Utils.Timer(0.5f, () =>
                {
                    switch (rank)
                    {
                        case LevelRank.S:
                            ShowBPAmount(20);
                            break;
                        case LevelRank.A:
                            ShowBPAmount(10);
                            break;
                        case LevelRank.B:
                            ShowBPAmount(5);
                            break;
                        case LevelRank.C:
                            ShowBPAmount(1);
                            break;
                    }
                    

                    Utils.Timer(3.5f, () =>
                    {
                        bpPerPrize.alpha = 0f;
                        bpPerPrize.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -567.47f);
                        bpPerPrize.text = "press [enter] to continue";
                        bpPerPrize.DOFade(1f, 0.5f).SetEase(Ease.OutExpo);
                        bpPerPrize.GetComponent<RectTransform>().DOAnchorPosY(-197.5f, 0.5f).SetEase(Ease.OutExpo);

                        canMoveOn = true;
                    });
                });
            };
        });
    }

    

    private void Update()
    {
        bpCounter.text = currentBPCount + " <b>BP</b>";
        if(canMoveOn && Input.GetKeyDown(KeyCode.Return))
        {
            canMoveOn = false;
            music.FadeOut(() =>
            {
                music.Dispose();
            });
            FadeManager.FadeOut(1f, () =>
            {
                ThePlayersParents.Singleton.DestroyPlayer();
                FadeManager.FadeIn(0.5f);
                UIController.FadeOut(0f);
                UIController.OpenMenu(UIMenus.MAIN_MENU);
            });
            
        }
    }

    

    public void ShowBPAmount(int amount)
    {
        bpPerPrize.alpha = 0f;
        bpPerPrize.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -567.47f);
        bpPerPrize.text = "+" + amount.ToString() + " <b>BP</b>";
        bpPerPrize.DOFade(1f, 0.5f).SetEase(Ease.OutExpo);
        bpPerPrize.GetComponent<RectTransform>().DOAnchorPosY(-197.5f, 0.5f).SetEase(Ease.OutExpo);

        Utils.Timer(1.5f, () =>
        {
            RedeemBPAmount(amount);
        });
    }

    public void RedeemBPAmount(int amount)
    {
        bpPerPrize.transform.localScale = Vector3.one * 1.2f;
        bpPerPrize.transform.DOScale(1f, 0.5f).SetEase(Ease.OutExpo);
        bpPerPrize.GetComponent<RectTransform>().DOAnchorPos(bpCounter.GetComponent<RectTransform>().anchoredPosition, 1f).SetEase(Ease.InBack);
        Utils.Timer(1f, () =>
        {
            currentBPCount += amount;
            HideBPAmount();
        });
    }

    public void HideBPAmount()
    {
        bpPerPrize.alpha = 0f;
        bpPerPrize.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -567.47f);
    }
}

public enum LevelRank
{
    S, A, B, C
}
