//  Distant Lands 2024
//  COZY: Stylized Weather 3
//  All code included in this file is protected under the Unity Asset Store Eula

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DistantLands.Cozy
{
    public class CozySaveLoadModule : CozyModule
    {

        struct DataSave
        {
            public string settings;
            public string weather;
            public string time;
            public string ambience;
        }

        // Start is called before the first frame update
        void Awake()
        {

            if (!enabled)
                return;

            InitializeModule();

        }


        public void Save()
        {

            if (weatherSphere == null)
                InitializeModule();

            Debug.Log(JsonUtility.ToJson(weatherSphere));

            PlayerPrefs.SetString("CZY_Save_Settings", JsonUtility.ToJson(weatherSphere));
            if (weatherSphere.timeModule)
                PlayerPrefs.SetString("CZY_Save_Time", JsonUtility.ToJson(weatherSphere.timeModule));
            if (weatherSphere.weatherModule)
                PlayerPrefs.SetString("CZY_Save_Weather", JsonUtility.ToJson(weatherSphere.weatherModule));
            if (weatherSphere.GetModule(out CozyAmbienceModule module))
                PlayerPrefs.SetString("CZY_Save_Ambience", JsonUtility.ToJson(module));

            Debug.Log("Saved COZY instance");

        }

        public string SaveToExternalJSON()
        {

            DataSave save = new DataSave()
            {
                settings = JsonUtility.ToJson(weatherSphere),
                weather = weatherSphere.weatherModule ? JsonUtility.ToJson(weatherSphere.weatherModule) : "",
                time = weatherSphere.timeModule ? JsonUtility.ToJson(weatherSphere.timeModule) : "",
                ambience = weatherSphere.GetModule(out CozyAmbienceModule module) ? JsonUtility.ToJson(module) : ""
            };

            Debug.Log("Wrote COZY instance to external JSON");
            return JsonUtility.ToJson(save);
        }

        public void Load()
        {


            if (weatherSphere == null)
                InitializeModule();

            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("CZY_Save_Settings"), weatherSphere);
            if (weatherSphere.timeModule)
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("CZY_Save_Time"), weatherSphere.timeModule);
            if (weatherSphere.weatherModule)
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("CZY_Save_Weather"), weatherSphere.weatherModule);
            if (weatherSphere.GetModule(out CozyAmbienceModule module))
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("CZY_Save_Ambience"), module);

            Debug.Log("Loaded COZY save to current instance");
        }

        public void LoadFromExternalJSON(string JSONSave)
        {

            DataSave save = JsonUtility.FromJson<DataSave>(JSONSave);

            JsonUtility.FromJsonOverwrite(save.settings, weatherSphere);
            if (weatherSphere.timeModule)
                JsonUtility.FromJsonOverwrite(save.time, weatherSphere.timeModule);
            if (weatherSphere.weatherModule)
                JsonUtility.FromJsonOverwrite(save.weather, weatherSphere.weatherModule);
            if (weatherSphere.GetModule(out CozyAmbienceModule module))
                JsonUtility.FromJsonOverwrite(save.ambience, module);

            Debug.Log("Loaded external JSON to current COZY instance");

        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CozySaveLoadModule))]
    public class E_CozySaveLoad : E_CozyModule, IControlPanel
    {

        CozySaveLoadModule saveLoad;

        void OnEnable()
        {

            saveLoad = (CozySaveLoadModule)target;

        }

        public override GUIContent GetGUIContent()
        {

            return new GUIContent("    Save & Load", (Texture)Resources.Load("Save"), "Manage save and load commands within the COZY system.");

        }

        public override void OpenDocumentationURL()
        {
            Application.OpenURL("https://distant-lands.gitbook.io/cozy-stylized-weather-documentation/how-it-works/modules/save-and-load-module");
        }

        public override void OnInspectorGUI()
        {


        }

        public override void DisplayInCozyWindow()
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Save"))
                saveLoad.Save();
            if (GUILayout.Button("Load"))
                saveLoad.Load();

            EditorGUILayout.EndHorizontal();

        }
        public void GetControlPanel()
        {
            if (GUILayout.Button("Save"))
                saveLoad.Save();
            if (GUILayout.Button("Load"))
                saveLoad.Load();
        }

    }
#endif
}