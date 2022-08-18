using Gravitons.UI.Modal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeLvls : MonoBehaviour
{
    private class Coordinates
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    Game game = new Game();
    public static Stopwatch stopWatch;

    const int countMarksFirstLevel = 7;
    static Coordinates[] marksCoordinateFirstLevel = new Coordinates[countMarksFirstLevel];

    const int countMarksSecondLevel = 6;
    const int countEmptySecondLevel = 8;
    static Coordinates[] marksCoordinateSecondLevel = new Coordinates[countMarksSecondLevel];
    static Coordinates[] emptyCoordinateSecondLevel = new Coordinates[countEmptySecondLevel];

    const int countMarksThirdLevel = 10;
    static Coordinates[] marksCoordinateThirdLevel = new Coordinates[countMarksThirdLevel];

    public void GameModeLvlsClick()
    {
        Game.gameModeLvls = true;
        EventSystem.current.currentSelectedGameObject.gameObject.SetActive(false);
        Game.OutputPredictionTime();
        CreateLevel();
    }

    public static void CreateLevel()
    {
        switch (Game.CountCompletedLvls)
        {
            case 0:
                Game.ClearGameField();
                StartFirstLevel();
                break;
            case 1:
                Game.ClearGameField();
                StartSecondLevel();
                break;
            case 2:
                Game.ClearGameField();
                StartThirdLevel();
                break;
            default:
                if (Game.gameModeLvls)
                {
                    Game.ClearGameField();
                    Game.gameModeLvls = false;
                    GameObject.Find("GameModeSanbox").gameObject.SetActive(false);
                    ModalManager.Show("Вітаємо!!!", "Ви успішно пройшли всі рівні! \nБажаємо успіхів у власних задач, на які вистачить тільки фантазії :)", new[] { new ModalButton() { Text = "Добре" } });
                }
                else
                    ModalManager.Show("Помикла!", "Ви вже пройшли всі рівні, які розроблені на даний момент :(\n Чекайте оновлень!", new[] { new ModalButton() { Text = "Добре" } });
                break;
        }
    }

    public static bool CheckCompletedLevel()
    {
        switch (Game.CountCompletedLvls)
        {
            case 0:
                return CheckCompletedFirstLevel();
            case 1:
                return CheckCompletedSecondLevel();
            case 2:
                return CheckCompletedThirdLevel();
            default:
                return false;
        }
    }

    private static void StartFirstLevel()
    {
        ModalManager.Show("Завдання першого рівня:", "Зберіть всі скрині на ігровому полі", new[] { new ModalButton() { Text = "Добре" } });


        Player.ErasePlayer();
        Player.PlayerCoordinate.XCoordinate = 3;
        Player.PlayerCoordinate.YCoordinate = 2;
        Player.PaintPlayer();


        const int countBarriers = 32;
        Coordinates[] barriersCoordinate = new Coordinates[countBarriers];
        barriersCoordinate[0] = new Coordinates { x=3, y=1 };
        barriersCoordinate[1] = new Coordinates { x = 2, y = 1 };
        barriersCoordinate[2] = new Coordinates { x = 4, y = 1 };
        barriersCoordinate[3] = new Coordinates { x = 4, y = 2 };
        barriersCoordinate[4] = new Coordinates { x = 4, y = 3 };
        barriersCoordinate[5] = new Coordinates { x = 5, y = 3 };
        barriersCoordinate[6] = new Coordinates { x = 5, y = 4 };
        barriersCoordinate[7] = new Coordinates { x = 5, y = 5 };
        barriersCoordinate[8] = new Coordinates { x = 4, y = 5 };
        barriersCoordinate[9] = new Coordinates { x = 4, y = 6 };
        barriersCoordinate[10] = new Coordinates { x = 5, y = 6 };
        barriersCoordinate[11] = new Coordinates { x = 5, y = 7 };
        barriersCoordinate[12] = new Coordinates { x = 5, y = 8 };
        barriersCoordinate[13] = new Coordinates { x = 5, y = 9 };
        barriersCoordinate[14] = new Coordinates { x = 4, y = 9 };
        barriersCoordinate[15] = new Coordinates { x = 4, y = 10 };
        barriersCoordinate[16] = new Coordinates { x = 4, y = 11 };
        barriersCoordinate[17] = new Coordinates { x = 4, y = 12 };
        barriersCoordinate[18] = new Coordinates { x = 4, y = 13 };
        barriersCoordinate[19] = new Coordinates { x = 3, y = 13 };
        barriersCoordinate[20] = new Coordinates { x = 2, y = 13 };
        barriersCoordinate[21] = new Coordinates { x = 2, y = 12 };
        barriersCoordinate[22] = new Coordinates { x = 2, y = 11 };
        barriersCoordinate[23] = new Coordinates { x = 2, y = 10 };
        barriersCoordinate[24] = new Coordinates { x = 2, y = 9 };
        barriersCoordinate[25] = new Coordinates { x = 2, y = 8 };
        barriersCoordinate[26] = new Coordinates { x = 2, y = 7 };
        barriersCoordinate[27] = new Coordinates { x = 2, y = 6 };
        barriersCoordinate[28] = new Coordinates { x = 2, y = 5 };
        barriersCoordinate[29] = new Coordinates { x = 2, y = 4 };
        barriersCoordinate[30] = new Coordinates { x = 2, y = 3 };
        barriersCoordinate[31] = new Coordinates { x = 2, y = 2 };

        marksCoordinateFirstLevel[0] = new Coordinates { x = 3, y = 3 };
        marksCoordinateFirstLevel[1] = new Coordinates { x = 4, y = 4 };
        marksCoordinateFirstLevel[2] = new Coordinates { x = 3, y = 5 };
        marksCoordinateFirstLevel[3] = new Coordinates { x = 3, y = 7 };
        marksCoordinateFirstLevel[4] = new Coordinates { x = 4, y = 7 };
        marksCoordinateFirstLevel[5] = new Coordinates { x = 4, y = 8 };
        marksCoordinateFirstLevel[6] = new Coordinates { x = 3, y = 10 };

        for (int i = 0; i < countBarriers; i++)
            ChangePropertiesField(barriersCoordinate[i].x, barriersCoordinate[i].y, "Barrier");
        for (int i = 0; i < countMarksFirstLevel; i++)
            ChangePropertiesField(marksCoordinateFirstLevel[i].x, marksCoordinateFirstLevel[i].y, "Mark");

        stopWatch = new Stopwatch();
        stopWatch.Start();
    }

    public static bool CheckCompletedFirstLevel()
    {
        for (int i = 0; i < countMarksFirstLevel; i++)
            if (Game.field[marksCoordinateFirstLevel[i].x, marksCoordinateFirstLevel[i].y].tag != "Empty")
                return false;
        return true;
    }

    private static void StartSecondLevel()
    {
        ModalManager.Show("Завдання другого рівня:", "Зберіть всі скрині по дорозі, а де їх немає - поставте!", new[] { new ModalButton() { Text = "Добре" } });


        Player.ErasePlayer();
        Player.PlayerCoordinate.XCoordinate = 2;
        Player.PlayerCoordinate.YCoordinate = 2;
        Player.PaintPlayer();


        const int countBarriers = 34;
        Coordinates[] barriersCoordinate = new Coordinates[countBarriers];
        barriersCoordinate[0] = new Coordinates { x = 1, y = 1 };
        barriersCoordinate[1] = new Coordinates { x = 1, y = 2 };
        barriersCoordinate[2] = new Coordinates { x = 1, y = 3 };
        barriersCoordinate[3] = new Coordinates { x = 1, y = 4 };
        barriersCoordinate[4] = new Coordinates { x = 1, y = 5 };
        barriersCoordinate[5] = new Coordinates { x = 1, y = 6 };
        barriersCoordinate[6] = new Coordinates { x = 1, y = 7 };
        barriersCoordinate[7] = new Coordinates { x = 1, y = 8 };
        barriersCoordinate[8] = new Coordinates { x = 1, y = 9 };
        barriersCoordinate[9] = new Coordinates { x = 1, y = 10 };
        barriersCoordinate[10] = new Coordinates { x = 1, y = 11 };
        barriersCoordinate[11] = new Coordinates { x = 1, y = 12 };
        barriersCoordinate[12] = new Coordinates { x = 3, y = 1 };
        barriersCoordinate[13] = new Coordinates { x = 3, y = 2 };
        barriersCoordinate[14] = new Coordinates { x = 3, y = 3 };
        barriersCoordinate[15] = new Coordinates { x = 3, y = 4 };
        barriersCoordinate[16] = new Coordinates { x = 3, y = 5 };
        barriersCoordinate[17] = new Coordinates { x = 3, y = 6 };
        barriersCoordinate[18] = new Coordinates { x = 3, y = 7 };
        barriersCoordinate[19] = new Coordinates { x = 3, y = 8 };
        barriersCoordinate[20] = new Coordinates { x = 3, y = 9 };
        barriersCoordinate[21] = new Coordinates { x = 3, y = 10 };
        barriersCoordinate[22] = new Coordinates { x = 4, y = 10 };
        barriersCoordinate[23] = new Coordinates { x = 5, y = 10 };
        barriersCoordinate[24] = new Coordinates { x = 6, y = 10 };
        barriersCoordinate[25] = new Coordinates { x = 7, y = 10 };
        barriersCoordinate[26] = new Coordinates { x = 7, y = 11 };
        barriersCoordinate[27] = new Coordinates { x = 7, y = 12 };
        barriersCoordinate[28] = new Coordinates { x = 6, y = 12 };
        barriersCoordinate[29] = new Coordinates { x = 5, y = 12 };
        barriersCoordinate[30] = new Coordinates { x = 4, y = 12 };
        barriersCoordinate[31] = new Coordinates { x = 3, y = 12 };
        barriersCoordinate[32] = new Coordinates { x = 2, y = 12 };
        barriersCoordinate[33] = new Coordinates { x = 2, y = 1 };

        marksCoordinateSecondLevel[0] = new Coordinates { x = 2, y = 2 };
        marksCoordinateSecondLevel[1] = new Coordinates { x = 2, y = 4 };
        marksCoordinateSecondLevel[2] = new Coordinates { x = 2, y = 7 };
        marksCoordinateSecondLevel[3] = new Coordinates { x = 2, y = 10 };
        marksCoordinateSecondLevel[4] = new Coordinates { x = 2, y = 11 };
        marksCoordinateSecondLevel[5] = new Coordinates { x = 4, y = 11 };

        emptyCoordinateSecondLevel[0] = new Coordinates { x = 2, y = 3 };
        emptyCoordinateSecondLevel[1] = new Coordinates { x = 2, y = 5 };
        emptyCoordinateSecondLevel[2] = new Coordinates { x = 2, y = 6 };
        emptyCoordinateSecondLevel[3] = new Coordinates { x = 2, y = 8 };
        emptyCoordinateSecondLevel[4] = new Coordinates { x = 2, y = 9 };
        emptyCoordinateSecondLevel[5] = new Coordinates { x = 3, y = 11 };
        emptyCoordinateSecondLevel[6] = new Coordinates { x = 5, y = 11 };
        emptyCoordinateSecondLevel[7] = new Coordinates { x = 6, y = 11 };


        for (int i = 0; i < countBarriers; i++)
            ChangePropertiesField(barriersCoordinate[i].x, barriersCoordinate[i].y, "Barrier");
        for (int i = 0; i < countMarksSecondLevel; i++)
            ChangePropertiesField(marksCoordinateSecondLevel[i].x, marksCoordinateSecondLevel[i].y, "Mark");

        stopWatch = new Stopwatch();
        stopWatch.Start();
    }

    public static bool CheckCompletedSecondLevel()
    {
        for (int i = 0; i < countMarksSecondLevel; i++)
            if (Game.field[marksCoordinateSecondLevel[i].x, marksCoordinateSecondLevel[i].y].tag != "Empty")
                return false;
        for (int i = 0; i < countEmptySecondLevel; i++)
            if (Game.field[emptyCoordinateSecondLevel[i].x, emptyCoordinateSecondLevel[i].y].tag != "Mark")
                return false;

        return true;
    }

    private static void StartThirdLevel()
    {
        ModalManager.Show("Завдання третього рівня:", "Зберіть всі скрині!", new[] { new ModalButton() { Text = "Добре" } });


        Player.ErasePlayer();
        Player.PlayerCoordinate.XCoordinate = 2;
        Player.PlayerCoordinate.YCoordinate = 3;
        Player.PaintPlayer();


        const int countBarriers = 58;
        Coordinates[] barriersCoordinate = new Coordinates[countBarriers];
        barriersCoordinate[0] = new Coordinates { x = 3, y = 1 };
        barriersCoordinate[1] = new Coordinates { x = 1, y = 1 };
        barriersCoordinate[2] = new Coordinates { x = 1, y = 2 };
        barriersCoordinate[3] = new Coordinates { x = 1, y = 3 };
        barriersCoordinate[4] = new Coordinates { x = 1, y = 4 };
        barriersCoordinate[5] = new Coordinates { x = 0, y = 4 };
        barriersCoordinate[6] = new Coordinates { x = 0, y = 5 };
        barriersCoordinate[7] = new Coordinates { x = 0, y = 6 };
        barriersCoordinate[8] = new Coordinates { x = 1, y = 6 };
        barriersCoordinate[9] = new Coordinates { x = 0, y = 7 };
        barriersCoordinate[10] = new Coordinates { x = 0, y = 8 };
        barriersCoordinate[11] = new Coordinates { x = 1, y = 8 };
        barriersCoordinate[12] = new Coordinates { x = 1, y = 9 };
        barriersCoordinate[13] = new Coordinates { x = 1, y = 10 };
        barriersCoordinate[14] = new Coordinates { x = 1, y = 11 };
        barriersCoordinate[15] = new Coordinates { x = 1, y = 12 };
        barriersCoordinate[16] = new Coordinates { x = 2, y = 12 };
        barriersCoordinate[17] = new Coordinates { x = 3, y = 12 };
        barriersCoordinate[18] = new Coordinates { x = 3, y = 11 };
        barriersCoordinate[19] = new Coordinates { x = 4, y = 12 };
        barriersCoordinate[20] = new Coordinates { x = 5, y = 12 };
        barriersCoordinate[21] = new Coordinates { x = 5, y = 11 };
        barriersCoordinate[22] = new Coordinates { x = 6, y = 11 };
        barriersCoordinate[23] = new Coordinates { x = 6, y = 10 };
        barriersCoordinate[24] = new Coordinates { x = 6, y = 9 };
        barriersCoordinate[25] = new Coordinates { x = 6, y = 8 };
        barriersCoordinate[26] = new Coordinates { x = 7, y = 8 };
        barriersCoordinate[27] = new Coordinates { x = 7, y = 7 };
        barriersCoordinate[28] = new Coordinates { x = 7, y = 6 };
        barriersCoordinate[29] = new Coordinates { x = 7, y = 5 };
        barriersCoordinate[30] = new Coordinates { x = 6, y = 5 };
        barriersCoordinate[31] = new Coordinates { x = 7, y = 4 };
        barriersCoordinate[32] = new Coordinates { x = 7, y = 3 };
        barriersCoordinate[33] = new Coordinates { x = 6, y = 3 };
        barriersCoordinate[34] = new Coordinates { x = 7, y = 2 };
        barriersCoordinate[35] = new Coordinates { x = 7, y = 1 };
        barriersCoordinate[36] = new Coordinates { x = 6, y = 1 };
        barriersCoordinate[37] = new Coordinates { x = 6, y = 0 };
        barriersCoordinate[38] = new Coordinates { x = 5, y = 0 };
        barriersCoordinate[39] = new Coordinates { x = 4, y = 1 };
        barriersCoordinate[40] = new Coordinates { x = 4, y = 0 };
        barriersCoordinate[41] = new Coordinates { x = 3, y = 0 };
        barriersCoordinate[42] = new Coordinates { x = 2, y = 0 };
        barriersCoordinate[43] = new Coordinates { x = 2, y = 1 };
        barriersCoordinate[44] = new Coordinates { x = 3, y = 3 };
        barriersCoordinate[45] = new Coordinates { x = 3, y = 4 };
        barriersCoordinate[46] = new Coordinates { x = 3, y = 5 };
        barriersCoordinate[47] = new Coordinates { x = 3, y = 6 };
        barriersCoordinate[48] = new Coordinates { x = 3, y = 7 };
        barriersCoordinate[49] = new Coordinates { x = 3, y = 8 };
        barriersCoordinate[50] = new Coordinates { x = 3, y = 9 };
        barriersCoordinate[51] = new Coordinates { x = 4, y = 9 };
        barriersCoordinate[52] = new Coordinates { x = 4, y = 8 };
        barriersCoordinate[53] = new Coordinates { x = 4, y = 7 };
        barriersCoordinate[54] = new Coordinates { x = 4, y = 6 };
        barriersCoordinate[55] = new Coordinates { x = 4, y = 5 };
        barriersCoordinate[56] = new Coordinates { x = 4, y = 4 };
        barriersCoordinate[57] = new Coordinates { x = 4, y = 3 };

        marksCoordinateThirdLevel[0] = new Coordinates { x = 1, y = 5 };
        marksCoordinateThirdLevel[1] = new Coordinates { x = 1, y = 7 };
        marksCoordinateThirdLevel[2] = new Coordinates { x = 2, y = 11 };
        marksCoordinateThirdLevel[3] = new Coordinates { x = 4, y = 11 };
        marksCoordinateThirdLevel[4] = new Coordinates { x = 6, y = 7 };
        marksCoordinateThirdLevel[5] = new Coordinates { x = 6, y = 6 };
        marksCoordinateThirdLevel[6] = new Coordinates { x = 6, y = 4 };
        marksCoordinateThirdLevel[7] = new Coordinates { x = 6, y = 2 };
        marksCoordinateThirdLevel[8] = new Coordinates { x = 5, y = 1 };
        marksCoordinateThirdLevel[9] = new Coordinates { x = 3, y = 1 };


        for (int i = 0; i < countBarriers; i++)
            ChangePropertiesField(barriersCoordinate[i].x, barriersCoordinate[i].y, "Barrier");
        for (int i = 0; i < countMarksThirdLevel; i++)
            ChangePropertiesField(marksCoordinateThirdLevel[i].x, marksCoordinateThirdLevel[i].y, "Mark");

        stopWatch = new Stopwatch();
        stopWatch.Start();
    }

    public static bool CheckCompletedThirdLevel()
    {
        for (int i = 0; i < countMarksThirdLevel; i++)
            if (Game.field[marksCoordinateThirdLevel[i].x, marksCoordinateThirdLevel[i].y].tag != "Empty")
                return false;
        return true;
    }
    


    private static void ChangePropertiesField(int xCoordinateField, int yCoordinateField, string tag)
    {
        if(tag == "Mark")
        {
            Game.field[xCoordinateField, yCoordinateField].transform.GetChild(0).GetComponentInChildren<Image>().sprite = Game.images[1].sprite;
            Game.field[xCoordinateField, yCoordinateField].tag = "Mark";
            Game.field[xCoordinateField, yCoordinateField].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255, 255, 255, 255);
        }
        else
        {
            Game.field[xCoordinateField, yCoordinateField].transform.GetChild(0).GetComponentInChildren<Image>().sprite = Game.images[2].sprite;
            Game.field[xCoordinateField, yCoordinateField].tag = "Barrier";
            Game.field[xCoordinateField, yCoordinateField].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(255, 255, 255, 255);
        }
    }
}
