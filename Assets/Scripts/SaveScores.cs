using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveScores : MonoBehaviour
{
    public TMP_InputField inputField;
    private float finalTime;

    private void Start()
    {
        // Retrieve the final time stored in PlayerPrefs
        finalTime = PlayerPrefs.GetFloat("FinalTime", 0f);
    }

    public void SetScores()
    {
        List<float> scores = new List<float>();
        List<string> names = new List<string>();

        // Retrieve existing scores or default to 1000 (a high value indicating no score)
        scores.Add(PlayerPrefs.GetFloat("Score1", 1000f));
        scores.Add(PlayerPrefs.GetFloat("Score2", 1000f));
        scores.Add(PlayerPrefs.GetFloat("Score3", 1000f));

        names.Add(PlayerPrefs.GetString("Name1", "---"));
        names.Add(PlayerPrefs.GetString("Name2", "---"));
        names.Add(PlayerPrefs.GetString("Name3", "---"));

        bool scoreInserted = false;

        for (int i = 0; i < scores.Count; i++)
        {
            if (finalTime < scores[i] && !scoreInserted)
            {
                scores.Insert(i, finalTime);
                names.Insert(i, inputField.text);
                scoreInserted = true;
            }
        }

        if (scores.Count > 3)
        {
            scores.RemoveAt(3);
            names.RemoveAt(3);
        }

        // Save the updated scores and names
        PlayerPrefs.SetFloat("Score1", scores[0]);
        PlayerPrefs.SetFloat("Score2", scores[1]);
        PlayerPrefs.SetFloat("Score3", scores[2]);

        PlayerPrefs.SetString("Name1", names[0]);
        PlayerPrefs.SetString("Name2", names[1]);
        PlayerPrefs.SetString("Name3", names[2]);

        PlayerPrefs.Save();

        SceneManager.LoadScene("Load Scores");
    }
}
