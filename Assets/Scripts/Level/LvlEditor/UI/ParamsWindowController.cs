using UnityEngine;
using OSB.Editor;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

public class ParamsWindowController : MonoBehaviour
{
    public LevelActor actor;
    public Transform content;
    public GameObject paramsPrefab;

    public void InitWindow()
    {
        Debug.Log(actor);
        transform.Find("Title").Find("TitleText").GetComponent<TextMeshProUGUI>().text = "PARAMS FOR \"" + actor.GetType().ToString() + "\"";

        foreach(Transform defParam in content)
        {
            Debug.Log(defParam);
            Destroy(defParam.gameObject);
        }
        foreach(KeyValuePair<string, ActorParam> param in actor.objParams)
        {
            GameObject go = Instantiate(paramsPrefab);
            go.transform.SetParent(content);
            go.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = param.Key;

            TMP_InputField field = go.transform.Find("Field").GetComponent<TMP_InputField>();
            field.text = param.Value.number != null ? param.Value.number.expression : param.Value.text;
            field.onValueChanged.AddListener((string val) =>
            {
                param.Value.number.expression = val;
                param.Value.text = val;

                if(param.Key == "Enemy")
                {
                    (actor as EnemyGroup).OnEnemyFieldChanged();
                    InitWindow();
                }
            });
            if(param.Key == "Enemy")
            {
                field.ActivateInputField();
                field.caretPosition = param.Value.text.Length;
                field.selectionAnchorPosition = param.Value.text.Length;
                field.selectionFocusPosition = param.Value.text.Length;
            }
        }
    }

    

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
