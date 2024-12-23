using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    [SerializeField] private LanguageSO defaultLanguage;
    private LanguageSO currentLanguage;

    private List<LocalizedTextBase> localizedTextComponents = new List<LocalizedTextBase>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetLanguage(defaultLanguage);
    }

    public void SetLanguage(LanguageSO language)
    {
        currentLanguage = language;
        UpdateAllText();
    }

    public LanguageSO GetCurrentLanguage()
    {
        return currentLanguage;
    }

    // Help from ChatGPT
    public void RegisterDisplayText(LocalizedTextBase localizedText)
    {
        if (!localizedTextComponents.Contains(localizedText))
        {
            localizedTextComponents.Add(localizedText);
        }
    }

    // Help from ChatGPT
    public void UnregisterDisplayText(LocalizedTextBase localizedText)
    {
        if (localizedTextComponents.Contains(localizedText))
        {
            localizedTextComponents.Remove(localizedText);
        }
    }

    // Help from ChatGPT
    private void UpdateAllText()
    {
        foreach (var localizedText in localizedTextComponents)
        {
            localizedText.UpdateText();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Clear all previously registered text components
        ClearAllTextComponents();
    }

    public void ClearAllTextComponents()
    {
        localizedTextComponents.Clear();
    }

}
