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
    public AlgorithmList() => algorithmList = new List<ItemList>();                                            //Конструктор чистого спика
    public AlgorithmList(List<ItemList> savedAlgorithmList) => algorithmList = savedAlgorithmList;             //Конструктор списка с сэйва

    private static Dictionary<string, int> nameInstructionByAmountNeedIndents = new Dictionary<string, int>         //Словарь команда - нужность добавить отступ 
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

    public static GameObject selectedItemList;          //Выбранный элемент списка
    public static List<ItemList> algorithmList;         //Список инструкций
    public static string conditionTextForIf;
    public static int numberRepeat;
    private static int globalCountIntends = 0;          //Колличество оступов
    public class ItemList
    {
        public GameObject GameObject { get; set; }          //Объект элемента списка в игре
        public string NameInstruction { get; set; }         //Название инструкции в игре
        public int AmountNeedIndents { get; set; }          //Колличество нужных отступов после инструкции
        public int CountIndents { get; set; }               //Колличество отступов для текущей инструкции
        public string ExtraNameInstruction { get; set; }    //Продолжения имени инструкции (для условий/циклов)
    }

    public static void AddInstructionInList(GameObject copyItemTemplate)
    {
        string nameInstruction = copyItemTemplate.GetComponent<Button>().name;      //Имя инструкции
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

        if (selectedItemList == null)                                                                                   //Не выбран ли элемент в списке в данный момент? 
        {
            algorithmList.Add(itemList);                                                                                //Добавление элемента в конец списка
            EditAmountIntendsBeforeInstruction(itemList.GameObject, itemList.NameInstruction, itemList.CountIndents, itemList.ExtraNameInstruction);      //Добавляем нужное колличество отступов
            globalCountIntends += nameInstructionByAmountNeedIndents[nameInstruction];                                  //Изменяем глобальное колличество отступов на -1, или 0, или +1
        }
        else
        {
            int itemIndex = algorithmList.FindIndex(x => x.GameObject == selectedItemList);                             //Определяем индекс выбранного элемента
            itemList.CountIndents = algorithmList[itemIndex].CountIndents;                                              //Определяем количество отступов выбранного элемента перед инструкцией
            
            algorithmList.Insert(itemIndex, itemList);                                                                  //Вставляем новый элемент в список
            copyItemTemplate.transform.SetSiblingIndex(itemIndex);                                                      //Вставляем новый элемент в список В ИГРЕ
            EditAmountIntendsBeforeInstruction(itemList.GameObject, itemList.NameInstruction, itemList.CountIndents, itemList.ExtraNameInstruction);   //Вставляем нужное количество отступов перед инструкцией В ИГРЕ

            if (itemList.AmountNeedIndents != 0)                                                                        //Нужно ли другое количество отступов после данной инструкции?
                EditAmountIndents(itemList.AmountNeedIndents, itemIndex + 1);                                           //Изменяем количество отступов в следующих инструкциях
        }
    }

    //Добавление отступов в инструкцию
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

    //Удаление инструкции в списке
    public static GameObject DeleteInstructionInList()
    {
        if (selectedItemList == null)
        {
            if(algorithmList.Count != 0)
            {
                int lastItemListIndex = algorithmList.Count - 1;                                    
                globalCountIntends -= algorithmList[lastItemListIndex].AmountNeedIndents;           //Если выбранная инструкция влияла на колличество отступов - убрать данное влияние
                GameObject itemForDelete = algorithmList[lastItemListIndex].GameObject;             //Сохраняем ссылку на объект элемента списка для удаления
                algorithmList.RemoveAt(lastItemListIndex);                                          //Удаляем выбранный элемент списка
                return itemForDelete;
            }
            else
                ModalManager.Show("Помилка!", "Немає команд для видалення!", new[] { new ModalButton() { Text = "Добре" } });
            return null;
        }
        else
        {
            int indexSelectedItemList = algorithmList.FindIndex(x => x.GameObject == selectedItemList);                         //Индекс выбранного объекта
            if (indexSelectedItemList + 1 != algorithmList.Count)
            {
                if (algorithmList[indexSelectedItemList].AmountNeedIndents != 0)                                                //Если есть разница в отступах - нужно форматировать отступы следующих элементов списка
                {
                    EditAmountIndents(-algorithmList[indexSelectedItemList].AmountNeedIndents, indexSelectedItemList + 1);      //Форматируем отступы перед инструкциями следующих элементов. Перед algorithmList[indexSelectedItemList].CountIndents знак "-" так как удаляем элемент со списка, а не добавляем
                }
                InsctructionsListManager.ApplySelectedColor(algorithmList[indexSelectedItemList + 1].GameObject);               //Делаем выбранным следующий элемент списка
            }
            else
            {
                globalCountIntends -= algorithmList[indexSelectedItemList].AmountNeedIndents;                                   //Если выбранная инструкция влияла на колличество отступов - убрать данное влияние
            }

            GameObject itemForDelete = algorithmList[indexSelectedItemList].GameObject;                                         //Сохраняем ссылку на объект элемента списка для удаления
            algorithmList.RemoveAt(indexSelectedItemList);                                                                      //Удаляем выбранный элемент списка
            return itemForDelete;                                                                                               //Возвращаем выбранный объект списка
        }
    }

    //Сохранение выбранного элемента списка
    public static void SaveSelectedItemList()
    {
        selectedItemList = EventSystem.current.currentSelectedGameObject;
        Debug.Log(selectedItemList.GetComponent<Button>().colors.ToString());
        //Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text);
        //EventSystem.current.currentSelectedGameObject.
    }


    private static void EditAmountIndents(int amountNeedIndents, int startIndex)
    {
        globalCountIntends += amountNeedIndents;                                                                                        //Изменяем глобальное значение колличества отступов 
        for (int i = startIndex; i < algorithmList.Count; i++)                                                                          //Проходим по всем элементам списка после выбранного
        {
            int newAmountNeedIndents = algorithmList[i].CountIndents + amountNeedIndents;                                               //Новое количество отступов перед инструкцией
            EditAmountIntendsBeforeInstruction(algorithmList[i].GameObject, algorithmList[i].NameInstruction, newAmountNeedIndents, algorithmList[i].ExtraNameInstruction);    //Изменяем количество отступов следующих элементов списка В ИГРЕ после выбранного
            algorithmList[i].CountIndents += amountNeedIndents;                                                                         //Изменяем колличество отступов следующих элементов списка после выбранного
        }
        LocalizationManager.SetLanguage(LocalizationManager.SelectedLanguage);                                                          //Локализируем текст с новыми отступами
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
