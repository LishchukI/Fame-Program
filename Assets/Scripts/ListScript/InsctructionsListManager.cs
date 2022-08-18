using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InsctructionsListManager : MonoBehaviour
{
    public GameObject itemTemplate;
    public GameObject content;
    public void AddInsctructionInListClick()
    {
        GameObject copyItemTemplate = Instantiate(itemTemplate);
        copyItemTemplate.transform.SetParent(content.transform, false);
        copyItemTemplate.GetComponent<Button>().name = EventSystem.current.currentSelectedGameObject.name;
        AlgorithmList.AddInstructionInList(copyItemTemplate);
    }

    public void DeleteInsctructionInListClick()
    {
        Destroy(AlgorithmList.DeleteInstructionInList());
    }

    public void SelectingItemListClick()
    {
        ApplyUnselectedColor(AlgorithmList.selectedItemList);
        ApplySelectedColor(EventSystem.current.currentSelectedGameObject);
    }

    public static void ApplyUnselectedColor(GameObject selectedObject)
    {
        if (AlgorithmList.selectedItemList != null)
        {
            Button selectedButton = selectedObject.GetComponent<Button>();
            ColorBlock unselectedColor = selectedButton.colors;
            unselectedColor.selectedColor = new Color32(255, 255, 255, 255);
            unselectedColor.normalColor = new Color32(255, 255, 255, 255);
            selectedButton.colors = unselectedColor;
            AlgorithmList.selectedItemList = null;
        }
    }

    public static void ApplySelectedColor(GameObject selectedObject)
    {
        AlgorithmList.selectedItemList = selectedObject;
        Button selectedButton = AlgorithmList.selectedItemList.GetComponent<Button>();
        ColorBlock selectedColor = selectedButton.colors;
        selectedColor.selectedColor = new Color32(67, 135, 255, 255);
        selectedColor.normalColor = new Color32(67, 135, 255, 255);
        selectedButton.GetComponent<Button>().colors = selectedColor;
    }

    public void LogAllListItemsClick()
    {
        AlgorithmList.LogAllListItems();
    }
}
