using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;
using UnityEditor;

public class SettingManager : MBSingleton<SettingManager>
{
    public List<SettingCategory> Categories { get; set; }
    public List<(Resolution res, string displayRes)> Resolutions { get; set; }
    public int NativeResolutionIndex { get; set; }

    public static string DEFAULT_SETTING_LOCATION;

    private SettingDatabase m_db;

    protected override void Awake()
    {
        DEFAULT_SETTING_LOCATION = Path.Combine(Application.persistentDataPath, "config.json");

        Resolutions = new();
        List<string> displayResolutions = new();

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Resolution resolution = Screen.resolutions[i];
            string resolutionDisplay = resolution.width.ToString() + "x" + resolution.height.ToString() + "@" + Mathf.RoundToInt((float)resolution.refreshRateRatio.value).ToString() + "hz";

            Resolutions.Add((resolution, resolutionDisplay));
            displayResolutions.Add(resolutionDisplay);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                NativeResolutionIndex = i;
            }
        }

        base.Awake();

        m_db = new SettingDatabase
        {
            categories = new List<SettingCategory>
            {
                new () {
                    categoryName = "Audio",
                    categoryId = "Audio",
                    settings = new List<Setting>{
                        new Setting {
                            name = "Master",
                            id = "Master",
                            type = SettingType.Slider,
                            minValue = 0,
                            maxValue = 100,
                            floatValue = 50
                        },
                        new Setting {
                            name = "Music",
                            id = "Music",
                            type = SettingType.Slider,
                            minValue = 0,
                            maxValue = 100,
                            floatValue = 100
                        },
                        new Setting {
                            name = "SFX",
                            id = "SFX",
                            type = SettingType.Slider,
                            minValue = 0,
                            maxValue = 100,
                            floatValue = 100
                        }
                    }
                },
                new SettingCategory {
                    categoryName = "Video",
                    categoryId = "Video",
                    settings = new List<Setting> {
                        new Setting{
                            name = "Resolution",
                            id = "Res",
                            type = SettingType.Dropdown,
                            options = Resolutions.Select(x => x.displayRes).ToArray(),
                            intValue = NativeResolutionIndex
                        },
                        new Setting{
                            name = "Fullscreen",
                            id = "FS",
                            type = SettingType.Toggle,
                            boolValue = true
                        }
                    }
                },
                new SettingCategory {
                    categoryName = "Gameplay",
                    categoryId = "Gameplay",
                    settings = new List<Setting>{
                        new Setting{
                            name = "Photosensitive Mode",
                            id = "NoFlash",
                            type = SettingType.Toggle,
                            boolValue = false
                        },
                        new Setting{
                            name = "Player Trail",
                            id = "PlrTrail",
                            type = SettingType.Dropdown,
                            options = new string[] {
                                "Default",
                                "Line",
                                "The Shapes Trail",
                                "The Shapes Editor Trail"
                            },
                            boolValue = false
                        }
                    }
                }
            }
        };

        Categories = Utils.CloneObject(m_db.categories);
        if (File.Exists(DEFAULT_SETTING_LOCATION))
        {
            SettingDatabase savedDb = JsonConvert.DeserializeObject<SettingDatabase>(File.ReadAllText(DEFAULT_SETTING_LOCATION));

            foreach (var category in savedDb.categories)
            {
                SettingCategory realCategory = Categories.Find(m => m.categoryId == category.categoryId);
                if (realCategory is null)
                {
                    continue;
                }

                foreach (var option in category.settings)
                {
                    Setting realSetting = realCategory.settings.Find(m => m.id == option.id);

                    if (realSetting is null) continue;

                    realSetting.boolValue = option.boolValue;
                    realSetting.intValue = option.intValue;
                    realSetting.floatValue = option.floatValue;
                }
            }
        }

        ApplyResolutionAndFullScreen();
    }

    /// <summary>
    /// Stub to get the instance
    /// </summary>
    public void Init() { }

    public void Save()
    {
        File.WriteAllText(DEFAULT_SETTING_LOCATION, JsonConvert.SerializeObject(new SettingDatabase { categories = Categories }, Formatting.Indented));
    }

    public void ApplyResolutionAndFullScreen()
    {
        var resSetting = GetSetting("Video", "Res");
        var fsSetting = GetSetting("Video", "FS");

        var selected = Resolutions[resSetting.intValue].res;
        Screen.SetResolution(selected.width, selected.height, fsSetting.boolValue ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, selected.refreshRateRatio);

        Debug.Log($"Applied new resolution (native res index: {NativeResolutionIndex})");
    }

    public void OnApplicationQuit()
    {
        Save();
    }

    public SettingCategory GetCategory(string categoryName)
    {
        return Categories.Find(m => m.categoryName == categoryName);
    }

    public Setting GetSetting(string categoryName, string setting)
    {
        return GetCategory(categoryName).settings.Find(m => m.id == setting);
    }
}