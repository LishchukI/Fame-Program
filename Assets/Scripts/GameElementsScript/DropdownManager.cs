using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DropdownManager : MonoBehaviour
{
    private static Dictionary<int, string> valueDropdownByNameInstruction = new Dictionary<int, string>
    {
        {0, "FieldWtihTag"},
        {1, "FieldEmpty"},
        {2, "ObstacleRight"},
        {3, "ObstacleLeft"},
        {4, "ObstacleDown"},
        {5, "ObstacleUp"},
        {6, "ObstacleNotRight"},
        {7, "ObstacleNotLeft"},
        {8, "ObstacleNotDown"},
        {9, "ObstacleNotUp"}
    };
    public void GetTextDroopdownIf()
    {
        AlgorithmList.conditionTextForIf = valueDropdownByNameInstruction[GetComponent<TMP_Dropdown>().value];
        //LocalizationManager.SetLanguage(LocalizationManager.SelectedLanguage);
    }
}
