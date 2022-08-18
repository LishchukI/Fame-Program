using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BrainCloud.LitJson;
using Gravitons.UI.Modal;
using System.IO;
using MLNetDll;
using TMPro;

public class Game : MonoBehaviour
{
    public const int countRow = 8;
    public const int countColumn = 14;
    public const int numberImages = 3;
    private const int countLevels = 3;
    
    public static int x = 0;
    public static bool workingGame;
    public static string language;
    public static Image[] images;
    public static GameObject[,] field;
    public static GameObject gameModeLvlButton;
    public static GameObject gameModeSanbox; 
    public static GameObject block;
    public static GameObject textPrediction;
    

    public static Player player;
    public AlgorithmList algoritmList;
    public static NewFieldProperties[] newFieldProperties;
    public static Dictionary<string, NewFieldProperties> tagNameFieldByNewFieldProperties;
    Dictionary<string, object> stats;

    public static int CountCompletedLvls;
    public static int CurrentTime;
    public static int PredictionTime;
    public static int TimeFirstLvl;
    public static int TimeSecondLvl;
    public static int TimeThirdLvl;
    public static bool gameModeLvls = false;

    public delegate void CallSave();
    public static CallSave callSave;

    // Start is called before the first frame update
    void Start()
    {
        callSave = SaveStatisticsToBrainCloud;

        InitStat();
        ReadCloud();
        InitButtons();
        InitImages();

        gameModeLvlButton = GameObject.Find("GameModeLvls").gameObject;
        gameModeSanbox = GameObject.Find("GameModeSanbox").gameObject;

        textPrediction = GameObject.Find("PredictionTime").gameObject;
        textPrediction.SetActive(false);

        block = GameObject.Find("Block").gameObject;
        block.SetActive(false);

        workingGame = false;
        language = "Ukrainian";
        player = new Player();
        algoritmList = new AlgorithmList();
        AlgorithmList.selectedItemList = null;
        AlgorithmList.conditionTextForIf = "FieldWtihTag";
        AlgorithmList.numberRepeat = 2;
        LocalizationManager.SetLanguage(LocalizationManager.SelectedLanguage);

        newFieldProperties = new NewFieldProperties[]
        {
            new NewFieldProperties(null, "Empty", new Color (255, 255, 255, 0)),
            new NewFieldProperties(images[1].sprite, "Mark", new Color (255, 255, 255, 255)),
            new NewFieldProperties(images[2].sprite, "Barrier", new Color (255, 255, 255, 255)),
        };

        tagNameFieldByNewFieldProperties = new Dictionary<string, NewFieldProperties>(3)
        {
            { "Barrier", newFieldProperties[0] },
            { "Empty", newFieldProperties[1] },
            { "Mark", newFieldProperties[2] }
        };
    }

    private void InitStat()
    {
        stats = new Dictionary<string, object> {
            {"CountCompletedLvls", CountCompletedLvls},
            {"CurrentTime", CurrentTime},
            {"PredictionTime", PredictionTime},
            {"TimeFirstLvl", TimeFirstLvl},
            {"TimeSecondLvl", TimeSecondLvl},
            {"TimeThirdLvl", TimeThirdLvl}
            };
    }

    private void ReadCloud()
    {
        InitStat();
        App.Bc.PlayerStatisticsService.ReadAllUserStats(StatsSuccess_Callback, StatsFailure_Callback, null);
    }

    private void StatsSuccess_Callback(string responseData, object cbObject)
    {
        JsonData jsonData = JsonMapper.ToObject(responseData);
        JsonData entries = jsonData["data"]["statistics"];

        CountCompletedLvls = int.Parse(entries["CountCompletedLvls"].ToString());
        CurrentTime = int.Parse(entries["CurrentTime"].ToString());
        PredictionTime = int.Parse(entries["PredictionTime"].ToString());
        TimeFirstLvl = int.Parse(entries["TimeFirstLvl"].ToString());
        TimeSecondLvl = int.Parse(entries["TimeSecondLvl"].ToString());
        TimeThirdLvl = int.Parse(entries["TimeThirdLvl"].ToString());

        OutputPredictionTime();

        if (CountCompletedLvls >= countLevels)
        {
            gameModeLvlButton.SetActive(false);
            gameModeSanbox.SetActive(false);
        }
    }

    private void StatsFailure_Callback(int statusCode, int reasonCode, string statusMessage, object cbObject)
    {
        Debug.Log(statusMessage);
    }


    public void SaveStatisticsToBrainCloud()
    {
        InitStat();
        App.Bc.PlayerStatisticsService.ResetAllUserStats();
        App.Bc.PlayerStatisticsService.IncrementUserStats(stats, SaveSuccess_Callback, SaveFailure_Callback, null);
    }

    private void SaveSuccess_Callback(string responseData, object cbObject)
    {
    }

    private void SaveFailure_Callback(int statusCode, int reasonCode, string statusMessage, object cbObject)
    {
        Debug.Log(statusMessage);
    }


    // Update is called once per frame
    void Update()
    {
        if (!gameModeLvls && !workingGame)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                player.MoveDown();
            if (Input.GetKeyDown(KeyCode.UpArrow))
                player.MoveUp();
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                player.MoveLeft();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                player.MoveRight();
            if (Input.GetKey("escape"))
                Application.Quit();
        }

        if (Input.GetMouseButtonDown(0))
            if (EventSystem.current.currentSelectedGameObject == null)                               //Является ли место, где нажали на мышку игровым объектом?
                InsctructionsListManager.ApplyUnselectedColor(AlgorithmList.selectedItemList);       //Ставим, что выбранного объекта нет
            else if (EventSystem.current.currentSelectedGameObject.tag != "EditList")                //Выбранный объкт не относится к кнопкам, которые отвечают за редактирование списка?
                InsctructionsListManager.ApplyUnselectedColor(AlgorithmList.selectedItemList);       //Ставим, что выбранного объекта нет
    }

    //Initialization
    private void InitButtons()
    {
        field = new GameObject[countRow, countColumn];
        for (int i = 0; i < countRow; i++)
            for (int ii = 0; ii < countColumn; ii++)
                field[i, ii] = GameObject.Find($"Button ({i}, {ii})").gameObject;
    }

    private void InitImages()
    {
        images = new Image[numberImages];
        images[0] = GameObject.Find("Player").GetComponent<Image>();
        images[1] = GameObject.Find("Tag").GetComponent<Image>();
        images[2] = GameObject.Find("Stop").GetComponent<Image>();
    }

    //Click methods
    public void FieldClick()
    {
        //int xCoordinate, yCoordinate;
        //GetCoordinateField(EventSystem.current.currentSelectedGameObject.name, out xCoordinate, out yCoordinate);
        //Debug.Log(EventSystem.current.currentSelectedGameObject.tag + xCoordinate + yCoordinate);
        if (!gameModeLvls)
        {
            GameObject currentField = EventSystem.current.currentSelectedGameObject;
            string tagNameField = currentField.tag;
            if (tagNameField == "Mark" && currentField.transform.GetChild(0).transform.GetChild(0).tag == "Player")
                ChangeFieldProperties(currentField, tagNameFieldByNewFieldProperties["Barrier"]);
            else
                ChangeFieldProperties(currentField, tagNameFieldByNewFieldProperties[tagNameField]);

            GetCoordinateField(EventSystem.current.currentSelectedGameObject.gameObject.name);
        }
        else
            ModalManager.Show("Помилка!", "В ігровому режимі проходження рівнів не можна ставити перешкоди та мітки!", new[] { new ModalButton() { Text = "Добре" } });
    }

    public static void ChangeFieldProperties(GameObject currentField, NewFieldProperties newFieldProperties)
    {
        currentField.transform.GetChild(0).GetComponentInChildren<Image>().sprite = newFieldProperties.sprite;
        currentField.tag = newFieldProperties.tag;
        currentField.transform.GetChild(0).GetComponentInChildren<Image>().color = newFieldProperties.color;
    }

    private void GetCoordinateField(string name)
    {
        int firstNumber, secondNumber;
        Regex regex = new Regex("\\((\\d+), (\\d+)\\)");
        Match match = regex.Match(name);
        if (!match.Success)
            throw new Exception("Regex error!");
        firstNumber = Convert.ToInt32(match.Groups[1].Value);
        secondNumber = Convert.ToInt32(match.Groups[2].Value);
        Debug.Log(firstNumber + " " + secondNumber);
    }


    public static string[,] SaveGameField()
    {
        string[,] tags = new string[countRow, countColumn];
        for (int i = 0; i < countRow; i++)
            for (int ii = 0; ii < countColumn; ii++)
                tags[i, ii]= field[i, ii].tag;
        return tags;
    }

    public static void LoadGameField(string[,] tags)
    {
        for (int i = 0; i < countRow; i++)
            for (int ii = 0; ii < countColumn; ii++)
            {
                if (tags[i,ii] == "Mark")
                {
                    field[i, ii].transform.GetChild(0).GetComponentInChildren<Image>().sprite = Game.images[1].sprite;
                    field[i, ii].tag = "Mark";
                    field[i, ii].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255, 255, 255, 255);
                }
                else if(tags[i, ii] == "Barrier")
                {
                    field[i, ii].transform.GetChild(0).GetComponentInChildren<Image>().sprite = Game.images[2].sprite;
                    field[i, ii].tag = "Barrier";
                    field[i, ii].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255, 255, 255, 255);
                }
                else if(tags[i, ii] == "Empty")
                {
                    field[i, ii].transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                    field[i, ii].tag = "Empty";
                    field[i, ii].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255, 255, 255, 0);
                }
            }
    }

    public static void ClearGameField()
    {
        for (int i = 0; i < countRow; i++)
            for (int ii = 0; ii < countColumn; ii++)
                ChangeFieldProperties(field[i, ii], tagNameFieldByNewFieldProperties["Barrier"]);
    }
    

    public static void ChangeTimeLevel()
    {
        switch (CountCompletedLvls)
        {
            case 0:
                TimeFirstLvl += CurrentTime;
                CurrentTime = 0;
                break;
            case 1:
                TimeSecondLvl += CurrentTime;
                CurrentTime = 0;
                break;
            case 2:
                TimeThirdLvl += CurrentTime;
                CurrentTime = 0;
                break;
        }
    }



    public static void AddTimeInFile()
    {
        string fileForML = Application.dataPath + "/Resources/CompletedLevelsStatistic.csv";
        string newStringInFile;

        switch (CountCompletedLvls)
        {
            case 2:
                newStringInFile = $"2, 13, 2, 3, {TimeFirstLvl}, {TimeSecondLvl}";
                File.AppendAllText(fileForML, newStringInFile);
                break;
            case 3:
                newStringInFile = $"3, 26, 4, 4, {TimeSecondLvl}, {TimeSecondLvl}";
                File.WriteAllText(fileForML, newStringInFile);
                break;
        }
    }

    public static void GetPredictML()
    {
        int predictionTime;
        switch (CountCompletedLvls)
        {
            case 1:
                predictionTime = Convert.ToInt32(MLNET.StartPrediction(2, 13, 2, 3, TimeFirstLvl));
                PredictionTime = predictionTime;
                OutputPredictionTime();

                break;
            case 2:
                predictionTime = Convert.ToInt32(MLNET.StartPrediction(3, 26, 4, 4, TimeSecondLvl));
                PredictionTime = predictionTime;
                OutputPredictionTime();
                break;
            case 3:
                textPrediction.SetActive(false);
                break;
        }
    }

    public static void OutputPredictionTime()
    {
        if (CountCompletedLvls < 3 && PredictionTime != 0 && gameModeLvls == true)
        {
            textPrediction.SetActive(true);
            textPrediction.gameObject.GetComponent<TextMeshProUGUI>().text = $"Приблизний час виконання рівня: {PredictionTime}";
        }
    }
}
