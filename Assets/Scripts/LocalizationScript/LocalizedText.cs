using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private string key;

    // Start is called before the first frame update
    void Start()
    {
        Localize();
        LocalizationManager.OnLanguageChange += OnLanguageChange;
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChange -= OnLanguageChange;
    }

    private void OnLanguageChange()
    {
        Localize();
    }

    private void Init()
    {
        text = GetComponent<TextMeshProUGUI>();
        key = text.text;
    }

    private void Localize(string newKey = null)
    {
        if (text == null)
            Init();

        if (newKey != null)
            key = newKey;

        int countSpace = text.text.TakeWhile(Char.IsWhiteSpace).Count();
        key = key.TrimStart(' ');

        string forLoop = null;
        if (key.Contains("RepeatTimes"))
        {
            forLoop = key.Substring(11, key.Length-11);
            key = "RepeatTimes";
        }

        text.text = LocalizationManager.GetTranlate(key);

        for (int i = 0; i < countSpace; i++)
            text.text = " " + text.text;
        if (forLoop != null)
            text.text = text.text + forLoop;
    }
}