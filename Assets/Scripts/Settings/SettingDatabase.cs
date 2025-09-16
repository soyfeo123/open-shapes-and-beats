using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "OSB/Settings Database", fileName = "OSBSettingsDatabase")]
public class SettingDatabase : ScriptableObject
{
    public List<SettingCategory> categories;
}

public enum SettingType { Toggle, Slider, Dropdown }

[Serializable]
public class Setting
{
    public string name;
    public string id;
    public SettingType type;

    [Header("For sliders")]
    public float minValue;
    public float maxValue;

    [Header("For dropdowns")]
    public string[] options;

    [Header("Current values (or default)")]
    public float floatValue;
    public bool boolValue;
    public int intValue;

    [NonSerialized] public Action<float> OnFloatChanged;
    [NonSerialized] public Action<bool> OnBoolChanged;
    [NonSerialized] public Action<int> OnIntChanged;
}

[Serializable]
public class SettingCategory
{
    public string categoryName;
    public string categoryId;
    public List<Setting> settings;
}

