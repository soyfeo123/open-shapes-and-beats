using UnityEngine;

[CreateAssetMenu(fileName ="SecretKey", menuName ="OSB/APIs/Secret Key", order = 5)]
public class APISecretKey : ScriptableObject
{
    public string key;
    public int keyNumber;

    public static string GetKey(string apiName)
    {
        return Resources.Load<APISecretKey>("Secrets/" + apiName + "_SECRET").key;
    }
}
