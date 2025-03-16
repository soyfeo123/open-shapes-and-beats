using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using GameJolt.API;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace GameJolt.Editor {
	public class Tools {
		private const string DefaultSettingsPath = "Assets/Plugins/GameJolt/GJAPISettings.asset";
		private const string ManagerPrefabPath = "Assets/Plugins/GameJolt/Prefabs/GameJoltAPI.prefab";

		private static Settings GetOrCreateSettings() {
			Settings settings;
			var assets = AssetDatabase.FindAssets("t:GameJolt.API.Settings");
			if(assets.Length == 0) {
				settings = ScriptableObject.CreateInstance<Settings>();
				AssetDatabase.CreateAsset(settings, DefaultSettingsPath);
				AssetDatabase.SaveAssets();
			} else {
				settings = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), typeof(Settings)) as Settings;
			}
			return settings;
		}

#if UNITY_2018_3_OR_NEWER
		[SettingsProvider]
		public static SettingsProvider SettingsProvider() {
			return new SettingsProvider("Project/Game Jolt API", SettingsScope.Project) {
				guiHandler = (searchContext) => SettingsEditor.DrawSettings(GetOrCreateSettings()),
				keywords = new HashSet<string> { "GameJolt", "Game Jolt" }
			};
		}
#else
		[MenuItem("Edit/Project Settings/Game Jolt API")]
		public static void Settings() {
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = GetOrCreateSettings();
		}
#endif

		[MenuItem("GameObject/Game Jolt API Manager")]
		public static void Manager() {
			var manager = Object.FindObjectOfType<GameJoltAPI>();
			if(manager != null) {
				Selection.activeObject = manager;
			} else {
				var prefab = AssetDatabase.LoadAssetAtPath(ManagerPrefabPath, typeof(GameObject)) as GameObject;
				if(prefab == null) {
					Debug.LogError("Unable to locate Game Jolt API prefab.");
				} else {
					var clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
					Selection.activeObject = clone;

					if(Object.FindObjectOfType<EventSystem>() == null) {
						// ReSharper disable once ObjectCreationAsStatement
						new GameObject(
							"EventSystem",
							typeof(EventSystem),
							typeof(StandaloneInputModule),
							typeof(StandaloneInputModule)
						);
					}
				}
			}
		}
	}
}