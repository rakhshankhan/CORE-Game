using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadScores : MonoBehaviour
{
    public TMP_Text score1, score2, score3, name1, name2, name3;
    void Start()
    {
        score1.text = PlayerPrefs.GetFloat("Score1").ToString();
        score2.text = PlayerPrefs.GetFloat("Score2").ToString();
        score3.text = PlayerPrefs.GetFloat("Score3").ToString();

        name1.text = PlayerPrefs.GetString("Name1");
        name2.text = PlayerPrefs.GetString("Name2");
        name3.text = PlayerPrefs.GetString("Name3");

        if (score1.text=="1000" || score1.text == "0")
        {
            score1.text = "---";
        }

        if (score2.text == "1000" || score2.text == "0")
        {
            score2.text = "---";
        }

        if (score3.text == "1000" || score3.text == "0")
        {
            score3.text = "---";
        }

        if (name1.text == "")
        {
            name1.text = "---";
        }
        if (name2.text == "")
        {
            name2.text = "---";
        }
        if (name3.text == "")
        {
            name3.text = "---";
        }
    }
}
