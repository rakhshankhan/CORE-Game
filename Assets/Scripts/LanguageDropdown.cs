using UnityEngine;
using TMPro;

public class LanguageDropdown : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;   
    public LanguageSO[] availableLanguages;

    private void Start()
    {
        // Populate the dropdown options with language names
        languageDropdown.ClearOptions();
        foreach (var language in availableLanguages)
        {
            languageDropdown.options.Add(new TMP_Dropdown.OptionData(language.name));
        }

        // Help from ChatGPT
        // Set the dropdown to match the current language
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        languageDropdown.value = GetCurrentLanguageIndex();
    }

    // Help from ChatGPT
    private int GetCurrentLanguageIndex()
    {
        LanguageSO currentLanguage = LocalizationManager.Instance.GetCurrentLanguage();
        for (int i = 0; i < availableLanguages.Length; i++)
        {
            if (availableLanguages[i] == currentLanguage)
                return i;
        }
        return 0; // Default to the first language
    }

    private void OnLanguageChanged(int index)
    {
        LocalizationManager.Instance.SetLanguage(availableLanguages[index]);
    }
}
