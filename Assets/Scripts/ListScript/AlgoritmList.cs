using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Gravitons.UI.Modal;
using System.Linq;
using System;

public class AlgorithmList
{
    public AlgorithmList() => algorithmList = new List<ItemList>();                                            //����������� ������� �����
    public AlgorithmList(List<ItemList> savedAlgorithmList) => algorithmList = savedAlgorithmList;             //����������� ������ � �����

    private static Dictionary<string, int> nameInstructionByAmountNeedIndents = new Dictionary<string, int>         //������� ������� - �������� �������� ������ 
    {
        {"MoveRight", 0},
        {"MoveLeft", 0},
        {"MoveDown", 0},
        {"MoveUp", 0},
        {"If", 1},
        {"Else", 1},
        {"EndCondition", -1},
        {"RepeatTimes", 1},
        {"EndLoop", -1},
        {"PutChest", 0},
        {"PickUpChest", 0}
    };

    public static GameObject selectedItemList;          //��������� ������� ������
    public static List<ItemList> algorithmList;         //������ ����������
    public static string conditionTextForIf;
    public static int numberRepeat;
    private static int globalCountIntends = 0;          //����������� �������
    public class ItemList
    {
        public GameObject GameObject { get; set; }          //������ �������� ������ � ����
        public string NameInstruction { get; set; }         //�������� ���������� � ����
        public int AmountNeedIndents { get; set; }          //����������� ������ �������� ����� ����������
        public int CountIndents { get; set; }               //����������� �������� ��� ������� ����������
        public string ExtraNameInstruction { get; set; }    //����������� ����� ���������� (��� �������/������)
    }

    public static void AddInstructionInList(GameObject copyItemTemplate)
    {
        string nameInstruction = copyItemTemplate.GetComponent<Button>().name;      //��� ����������
        string extraNameInstruction = null;

        //!!!!
        if (nameInstruction == "If")
            extraNameInstruction = conditionTextForIf;
        if (nameInstruction == "RepeatTimes")
            extraNameInstruction = Convert.ToString(numberRepeat);
        if (nameInstruction == "Else" || nameInstruction == "EndCondition" || nameInstruction == "EndLoop")
            globalCountIntends--;
        //!!!!

        ItemList itemList = new ItemList
        {
            GameObject = copyItemTemplate,
            NameInstruction = nameInstruction,
            AmountNeedIndents = nameInstructionByAmountNeedIndents[nameInstruction],
            CountIndents = globalCountIntends,
            ExtraNameInstruction = extraNameInstruction
        };


        //!!!!
        if (nameInstruction == "EndCondition" || nameInstruction == "EndLoop")
            globalCountIntends++;
        //!!!!

        if (selectedItemList == null)                                                                                   //�� ������ �� ������� � ������ � ������ ������? 
        {
            algorithmList.Add(itemList);                                                                                //���������� �������� � ����� ������
            EditAmountIntendsBeforeInstruction(itemList.GameObject, itemList.NameInstruction, itemList.CountIndents, itemList.ExtraNameInstruction);      //��������� ������ ����������� ��������
            globalCountIntends += nameInstructionByAmountNeedIndents[nameInstruction];                                  //�������� ���������� ����������� �������� �� -1, ��� 0, ��� +1
        }
        else
        {
            int itemIndex = algorithmList.FindIndex(x => x.GameObject == selectedItemList);                             //���������� ������ ���������� ��������
            itemList.CountIndents = algorithmList[itemIndex].CountIndents;                                              //���������� ���������� �������� ���������� �������� ����� �����������
            
            algorithmList.Insert(itemIndex, itemList);                                                                  //��������� ����� ������� � ������
            copyItemTemplate.transform.SetSiblingIndex(itemIndex);                                                      //��������� ����� ������� � ������ � ����
            EditAmountIntendsBeforeInstruction(itemList.GameObject, itemList.NameInstruction, itemList.CountIndents, itemList.ExtraNameInstruction);   //��������� ������ ���������� �������� ����� ����������� � ����

            if (itemList.AmountNeedIndents != 0)                                                                        //����� �� ������ ���������� �������� ����� ������ ����������?
                EditAmountIndents(itemList.AmountNeedIndents, itemIndex + 1);                                           //�������� ���������� �������� � ��������� �����������
        }
    }

    //���������� �������� � ����������
    public static void EditAmountIntendsBeforeInstruction(GameObject copyItemTemplate, string nameInstruction, int countIntends, string extraNameInstruction)
    {
        if (countIntends >= 0)
            for (int i = 0; i < countIntends; i++)
                nameInstruction = "   " + nameInstruction;

        if (extraNameInstruction != null)
            copyItemTemplate.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = nameInstruction + " " + extraNameInstruction;
        else
            copyItemTemplate.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = nameInstruction;
        //LocalizationManager.SetLanguage(LocalizationManager.SelectedLanguage);
    }

    //�������� ���������� � ������
    public static GameObject DeleteInstructionInList()
    {
        if (selectedItemList == null)
        {
            if(algorithmList.Count != 0)
            {
                int lastItemListIndex = algorithmList.Count - 1;                                    
                globalCountIntends -= algorithmList[lastItemListIndex].AmountNeedIndents;           //���� ��������� ���������� ������ �� ����������� �������� - ������ ������ �������
                GameObject itemForDelete = algorithmList[lastItemListIndex].GameObject;             //��������� ������ �� ������ �������� ������ ��� ��������
                algorithmList.RemoveAt(lastItemListIndex);                                          //������� ��������� ������� ������
                return itemForDelete;
            }
            else
                ModalManager.Show("�������!", "���� ������ ��� ���������!", new[] { new ModalButton() { Text = "�����" } });
            return null;
        }
        else
        {
            int indexSelectedItemList = algorithmList.FindIndex(x => x.GameObject == selectedItemList);                         //������ ���������� �������
            if (indexSelectedItemList + 1 != algorithmList.Count)
            {
                if (algorithmList[indexSelectedItemList].AmountNeedIndents != 0)                                                //���� ���� ������� � �������� - ����� ������������� ������� ��������� ��������� ������
                {
                    EditAmountIndents(-algorithmList[indexSelectedItemList].AmountNeedIndents, indexSelectedItemList + 1);      //����������� ������� ����� ������������ ��������� ���������. ����� algorithmList[indexSelectedItemList].CountIndents ���� "-" ��� ��� ������� ������� �� ������, � �� ���������
                }
                InsctructionsListManager.ApplySelectedColor(algorithmList[indexSelectedItemList + 1].GameObject);               //������ ��������� ��������� ������� ������
            }
            else
            {
                globalCountIntends -= algorithmList[indexSelectedItemList].AmountNeedIndents;                                   //���� ��������� ���������� ������ �� ����������� �������� - ������ ������ �������
            }

            GameObject itemForDelete = algorithmList[indexSelectedItemList].GameObject;                                         //��������� ������ �� ������ �������� ������ ��� ��������
            algorithmList.RemoveAt(indexSelectedItemList);                                                                      //������� ��������� ������� ������
            return itemForDelete;                                                                                               //���������� ��������� ������ ������
        }
    }

    //���������� ���������� �������� ������
    public static void SaveSelectedItemList()
    {
        selectedItemList = EventSystem.current.currentSelectedGameObject;
        Debug.Log(selectedItemList.GetComponent<Button>().colors.ToString());
        //Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text);
        //EventSystem.current.currentSelectedGameObject.
    }


    private static void EditAmountIndents(int amountNeedIndents, int startIndex)
    {
        globalCountIntends += amountNeedIndents;                                                                                        //�������� ���������� �������� ����������� �������� 
        for (int i = startIndex; i < algorithmList.Count; i++)                                                                          //�������� �� ���� ��������� ������ ����� ����������
        {
            int newAmountNeedIndents = algorithmList[i].CountIndents + amountNeedIndents;                                               //����� ���������� �������� ����� �����������
            EditAmountIntendsBeforeInstruction(algorithmList[i].GameObject, algorithmList[i].NameInstruction, newAmountNeedIndents, algorithmList[i].ExtraNameInstruction);    //�������� ���������� �������� ��������� ��������� ������ � ���� ����� ����������
            algorithmList[i].CountIndents += amountNeedIndents;                                                                         //�������� ����������� �������� ��������� ��������� ������ ����� ����������
        }
        LocalizationManager.SetLanguage(LocalizationManager.SelectedLanguage);                                                          //������������ ����� � ������ ���������
    }






    public static void LogAllListItems()
    {
        int i = 0;
        foreach (var itemlist in algorithmList)
        {
            //Debug.Log(i + ") " + itemlist.Item1 + " " + itemlist.Item2 + " " + itemlist.Item3 + " " + itemlist.Item4);
        }
    }
}
