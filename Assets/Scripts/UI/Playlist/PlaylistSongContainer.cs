using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaylistSongContainer : MonoBehaviour
{
    public int index = 0;
    public List<PlaylistTrackSelection> buttons = new List<PlaylistTrackSelection>();

    public void UpdateButtons()
    {
        buttons.Clear();

        // WHY DOESN'T THIS FREAKING WORK
        foreach (Transform btn in transform)
        {
            if(btn.GetComponent<PlaylistTrackSelection>() != null)
            buttons.Add(btn.GetComponent<PlaylistTrackSelection>());
        }
        //index = 0;
        if (index > buttons.Count) index = 0;

        if (buttons.Count <= 0) return;

        buttons[index].OnPointerEnter(null);
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);

        EnsureScrollVisible(buttons[index].GetComponent<RectTransform>());
    }

    public void RemoveAllPlaylistEntries()
    {
        buttons.Clear();
        foreach(Transform btn in transform)
        {
            Destroy(btn.gameObject);
        }
    }

    Vector3 LocalPositionWithinAncestor(Transform ancestor, Transform target)
    {
        var result = Vector3.zero;
        while (ancestor != target && target != null)
        {
            result += target.localPosition;
            target = target.parent;
        }
        return result;
    }

    public void EnterCurrentLevel()
    {
        if (buttons.Count <= 0) return;
        buttons[index].OnPointerClick(null);
    }

    public void EnsureScrollVisible(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        var targetPosition = LocalPositionWithinAncestor(transform, target);
        var top = (-targetPosition.y) - target.rect.height / 2;
        var bottom = (-targetPosition.y) + target.rect.height / 2;

        var topMargin = 100; // this is here because there are headers over the buttons sometimes
        var bottomMargin = 100; // this is here because i totally didn't just take this function fromm stackoverflow
        // actually nevermind lol
        var bottomPadding = 250f; // Adjust this value to control the amount of padding

        var result = transform.GetComponent<RectTransform>().anchoredPosition;

        // Top margin logic remains the same
        if (result.y > top - topMargin)
            result.y = top - topMargin;

        // Add bottomPadding to prevent the selected item from touching the bottom of the screen
        if (result.y + transform.parent.GetComponent<RectTransform>().rect.height < bottom + bottomPadding)
            result.y = bottom - transform.parent.GetComponent<RectTransform>().rect.height + bottomPadding;

        //Debug.Log($"{targetPosition} {target.rect.size} {top} {bottom} {scrollRect.content.anchoredPosition}->{result}");

        transform.GetComponent<RectTransform>().DOKill();
        transform.GetComponent<RectTransform>().DOAnchorPos(result, 0.5f).SetEase(Ease.OutExpo);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < -0.2f)
        {
            buttons[index].OnPointerExit(null);
            index++;
            if (index >= buttons.Count)
            {
                index = 0;
            }
            Debug.Log("index: " + index + ", count: " + buttons.Count);

            Debug.Log(index);

            if (buttons.Count <= 0) return;
            buttons[index].OnPointerEnter(null);
            EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
            EnsureScrollVisible(buttons[index].GetComponent<RectTransform>());
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0.2f)
        {
            buttons[index].OnPointerExit(null);
            index--;
            if (index < 0)
            {
                index = buttons.Count - 1;
            }
            if (buttons.Count <= 0) return;
            buttons[index].OnPointerEnter(null);
            EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);

            EnsureScrollVisible(buttons[index].GetComponent<RectTransform>());
        }
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    buttons[index].OnPointerClick(null);
        //}
    }
}
