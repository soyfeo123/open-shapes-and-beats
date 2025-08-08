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
            go.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = param.Key.Split("///")[0];

            string[] split = param.Key.Split("///");
            string path = "Prefabs/LevelEditorPrefabs/Params/Fields/" + (split.Length > 1 ? split[1] : "Normal");

            Debug.Log(path);
            GameObject fieldObj = Instantiate(Resources.Load<GameObject>(path));
            fieldObj.transform.SetParent(go.transform);
            fieldObj.name = "Field";

            ParamFieldGetter field = go.transform.Find("Field").GetComponent<ParamFieldGetter>();
            field.SetValue(param.Value.number != null ? param.Value.number.expression : param.Value.text);
            field.OnValueChanged.AddListener((string val) =>
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
                field.Focus(param.Value.text.Length);
            }
        }
    }

    

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
