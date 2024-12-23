#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LocalizationWindow : EditorWindow
{
    private List<LanguageSO> languages = new List<LanguageSO>();
    private int selectedLanguageIndex = 0;
    private Vector2 scrollPosition;
    private string newKey = "";
    private string newValue = "";
    private string languageFolderPath = "Assets/Scripts/Languages";

    private string addKeyErrorMessage = "";
    private string editKeyErrorMessage = "";

    [MenuItem("Window/Localization Editor")]
    static void OpenWindow()
    {
        LocalizationWindow window = (LocalizationWindow)GetWindow(typeof(LocalizationWindow));
        window.Show();
    }

    private void OnEnable()
    {
        LoadLanguages();
    }

    private void OnGUI()
    {
        // Language Selection Dropdown
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Select Language:", GUILayout.Width(100));

        string[] languageOptions = new string[languages.Count];
        for (int i = 0; i < languages.Count; i++)
        {
            languageOptions[i] = languages[i].name;
        }

        selectedLanguageIndex = EditorGUILayout.Popup(selectedLanguageIndex, languageOptions);
        EditorGUILayout.EndHorizontal();

        if (languages.Count == 0)
        {
            EditorGUILayout.LabelField($"No LanguageSO found in {languageFolderPath}.");
            if (GUILayout.Button("Reload Languages"))
            {
                LoadLanguages();
            }
            return;
        }

        DrawLanguageEditor();
    }

    private void LoadLanguages()
    {
        languages.Clear();

        // Help from ChatGPT
        string[] guids = AssetDatabase.FindAssets("t:LanguageSO", new[] { languageFolderPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            LanguageSO language = AssetDatabase.LoadAssetAtPath<LanguageSO>(path);
            languages.Add(language);
        }
    }

    private void DrawLanguageEditor()
    {
        if (selectedLanguageIndex < 0 || selectedLanguageIndex >= languages.Count)
            return;

        LanguageSO selectedLanguage = languages[selectedLanguageIndex];

        EditorGUILayout.Space();

        if (GUILayout.Button("Sync Keys"))
        {
            SyncKeys();
        }

        EditorGUILayout.Space();

        // Help from ChatGPT
        // Begin scroll view
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        try
        {
            // Editable key-value pairs
            List<string> keysToDelete = new List<string>();
            List<string> keysToModify = new List<string>();
            List<string> newKeys = new List<string>();

            // Iterate over a copy of the keys
            foreach (var key in new List<string>(selectedLanguage.languageDictionary.Keys))
            {
                EditorGUILayout.BeginHorizontal();
                try
                {
                    string newKeyName = EditorGUILayout.TextField(key, GUILayout.Width(200));

                    if (newKeyName != key)
                    {
                        keysToModify.Add(key);
                        newKeys.Add(newKeyName);
                    }

                    string value = selectedLanguage.languageDictionary[key];
                    string newValue = EditorGUILayout.TextField(value);

                    if (newValue != value)
                    {
                        // Update value in the selected language - Help from ChatGPT
                        Undo.RecordObject(selectedLanguage, "Edit Localization Value");
                        selectedLanguage.languageDictionary[key] = newValue;
                        EditorUtility.SetDirty(selectedLanguage);
                    }

                    // Delete Key Button
                    if (GUILayout.Button("Delete", GUILayout.Width(60)))
                    {
                        keysToDelete.Add(key);
                    }
                }
                finally
                {
                    EditorGUILayout.EndHorizontal();
                }
            }

            for (int i = 0; i < keysToModify.Count; i++)
            {
                string errorMessage = ModifyKey(keysToModify[i], newKeys[i]);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    editKeyErrorMessage = errorMessage;
                }
                else
                {
                    editKeyErrorMessage = "";
                }
            }

            if (!string.IsNullOrEmpty(editKeyErrorMessage))
            {
                EditorGUILayout.HelpBox(editKeyErrorMessage, MessageType.Error);
            }

            // Delete keys from list of keys to delete
            foreach (var key in keysToDelete)
            {
                DeleteKey(key);
            }
        }
        finally
        {
            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.Space();

        // Add New Key-Value Pair
        EditorGUILayout.LabelField("Add New Key-Value Pair:");
        newKey = EditorGUILayout.TextField("Key", newKey);
        newValue = EditorGUILayout.TextField("Value", newValue);

        if (GUILayout.Button("Add Key-Value Pair"))
        {
            // Clear previous error message
            addKeyErrorMessage = "";

            if (string.IsNullOrEmpty(newKey))
            {
                addKeyErrorMessage = "Key cannot be empty.";
            }
            else if (KeyExists(newKey))
            {
                addKeyErrorMessage = $"The key '{newKey}' already exists.";
            }
            else
            {
                AddNewKey(newKey, newValue);
                newKey = "";
                newValue = "";
            }
        }

        if (!string.IsNullOrEmpty(addKeyErrorMessage))
        {
            EditorGUILayout.HelpBox(addKeyErrorMessage, MessageType.Error);
        }
    }


    private bool KeyExists(string key)
    {
        foreach (var lang in languages)
        {
            if (lang.languageDictionary.ContainsKey(key))
            {
                return true;
            }
        }
        return false;
    }

    private void AddNewKey(string key, string value)
    {
        foreach (var lang in languages)
        {
            // Help from ChatGPT
            Undo.RecordObject(lang, "Add Localization Key");
            lang.languageDictionary.Add(key, lang == languages[selectedLanguageIndex] ? value : "");
            EditorUtility.SetDirty(lang);
        }
    }

    private void DeleteKey(string key)
    {
        foreach (var lang in languages)
        {
            if (lang.languageDictionary.ContainsKey(key))
            {
                Undo.RecordObject(lang, "Delete Localization Key");
                lang.languageDictionary.Remove(key);
                EditorUtility.SetDirty(lang);
            }
        }
    }

    private string ModifyKey(string oldKey, string newKey)
    {
        if (string.IsNullOrEmpty(newKey))
        {
            return "Key cannot be empty.";
        }

        if (KeyExists(newKey))
        {
            return $"The key '{newKey}' already exists.";
        }

        foreach (var lang in languages)
        {
            if (lang.languageDictionary.ContainsKey(oldKey))
            {
                Undo.RecordObject(lang, "Edit Localization Key");
                string value = lang.languageDictionary[oldKey];
                lang.languageDictionary.Remove(oldKey);
                lang.languageDictionary.Add(newKey, value);
                EditorUtility.SetDirty(lang);
            }
        }

        return ""; // No error
    }

    private void SyncKeys()
    {
        HashSet<string> allKeys = new HashSet<string>();
        foreach (var lang in languages)
        {
            foreach (var key in lang.languageDictionary.Keys)
            {
                allKeys.Add(key);
            }
        }

        foreach (var lang in languages)
        {
            foreach (var key in allKeys)
            {
                if (!lang.languageDictionary.ContainsKey(key))
                {
                    // Help from ChatGPT
                    Undo.RecordObject(lang, "Sync Localization Keys");
                    lang.languageDictionary.Add(key, "");
                    EditorUtility.SetDirty(lang);
                }
            }
        }

    }
}
#endif