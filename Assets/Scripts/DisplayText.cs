using TMPro;
using UnityEngine;

public class DisplayText : LocalizedTextBase
{
    public TMP_Text displayText;
    public string displayKey;

    private void Start()
    {
        LocalizationManager.Instance.RegisterDisplayText(this);
        UpdateText();
    }

    // Help from ChatGPT
    private void OnDestroy()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.UnregisterDisplayText(this);
        }
    }

    public override void UpdateText()
    {
        var currentLanguage = LocalizationManager.Instance.GetCurrentLanguage();

        if (currentLanguage.languageDictionary.ContainsKey(displayKey))
        {
            displayText.text = currentLanguage.languageDictionary[displayKey];
        }
        else
        {
            displayText.text = $"Key '{displayKey}' not found.";
        }
    }
}
