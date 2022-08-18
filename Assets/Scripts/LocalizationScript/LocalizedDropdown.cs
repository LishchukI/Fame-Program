using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Dropdown))]
public class LocalizedDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private List<string> keys;

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

    public void OnLanguageChange()
    {
        Localize();
    }

    private void Localize(List<string> newKeys = null)
    {
        if (dropdown == null)
            Init();

        if (newKeys != null)
            keys = newKeys;

        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var key in keys)
            options.Add(new TMP_Dropdown.OptionData(LocalizationManager.GetTranlate(key)));

        dropdown.options = options;
        dropdown.RefreshShownValue();
    }

    private void Init()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        keys = new List<string>();

        foreach (var option in dropdown.options)
            keys.Add(option.text);
    }


}
