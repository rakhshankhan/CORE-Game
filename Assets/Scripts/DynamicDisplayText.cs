using TMPro;
using UnityEngine;

public class DynamicDisplayText : LocalizedTextBase
{
    public TMP_Text displayText;   
    public string displayKey;     

    private string localizedTemplate;

    private void Start()
    {
        LocalizationManager.Instance.RegisterDisplayText(this);
        UpdateTemplate();
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.UnregisterDisplayText(this);
        }
    }

    // Help from ChatGPT
    private void UpdateTemplate()
    {
        var currentLanguage = LocalizationManager.Instance.GetCurrentLanguage();
        if (currentLanguage.languageDictionary.ContainsKey(displayKey))
        {
            localizedTemplate = currentLanguage.languageDictionary[displayKey];
        }
        else
        {
            localizedTemplate = $"Key '{displayKey}' not found.";
        }

        UpdateText();
    }

    public override void UpdateText()
    {
        displayText.text = localizedTemplate;
    }

    // Help from ChatGPT
    public void UpdateDynamicText(params object[] args)
    {
        if (!string.IsNullOrEmpty(localizedTemplate))
        {
            displayText.text = string.Format(localizedTemplate, args);
        }
    }

    // Help from ChatGPT
    public void SetDisplayKey(string newKey)
    {
        displayKey = newKey;
        UpdateTemplate();
    }
}
