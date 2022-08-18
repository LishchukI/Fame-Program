using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.ComponentModel;
using Gravitons.UI.Modal;
using UnityEngine.EventSystems;
using System;

public class StartWork : MonoBehaviour
{
    public static int indexAlgorithm;
    public static float speedWorkAlgorithm = 0.5f;
    EventSystem es;

    public void StartWorkClick()
    {
        Game.block.SetActive(true);
        Game.workingGame = true;

        if (Game.gameModeLvls)
            GameModeLvls.stopWatch.Stop();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        int startPositionXCoordinate = Player.PlayerCoordinate.XCoordinate;
        int startPositionYCoordinate = Player.PlayerCoordinate.YCoordinate;
        string[,] startProperties = Game.SaveGameField();
        indexAlgorithm = 0;
        for ( ; indexAlgorithm < AlgorithmList.algorithmList.Count; indexAlgorithm++)
        {
            MethodInfo nameInstructionMenthod = Game.player.GetType().GetMethod(AlgorithmList.algorithmList[indexAlgorithm].NameInstruction);
            GameObject currentSelectedObject = AlgorithmList.algorithmList[indexAlgorithm].GameObject;
            InsctructionsListManager.ApplySelectedColor(currentSelectedObject);
            nameInstructionMenthod.Invoke(Game.player, null);
            yield return new WaitForSecondsRealtime(1 / speedWorkAlgorithm);
            InsctructionsListManager.ApplyUnselectedColor(currentSelectedObject);
        }

        bool lvlCompleted = false;
        if (indexAlgorithm == AlgorithmList.algorithmList.Count)
            if (Game.gameModeLvls)
            {
                if (GameModeLvls.CheckCompletedLevel())
                {
                    TimeSpan ts = GameModeLvls.stopWatch.Elapsed;
                    Game.CurrentTime += ts.Minutes*60 + ts.Hours * 360 + ts.Seconds;
                    GameModeLvls.stopWatch.Reset();
                    Game.ChangeTimeLevel();


                    Game.CountCompletedLvls++;
                    Game.AddTimeInFile();
                    Game.GetPredictML();
                    Game.callSave();
                    GameModeLvls.CreateLevel();
                    lvlCompleted = true;
                    ModalManager.Show("Успіх!", "Рівень пройдено успішно!", new[] { new ModalButton() { Text = "Добре" } });                    
                }
                else
                    ModalManager.Show("Спробуйте ще раз!", "Ви зібрали не всі скрині на ігровому полі!", new[] { new ModalButton() { Text = "Добре" } });
            }
            else
                ModalManager.Show("Успіх!", "Виконання алгоритму завершено!", new[] { new ModalButton() { Text = "Добре" } });


        if (!lvlCompleted)
        {
            Player.ErasePlayer();
            Player.PlayerCoordinate.XCoordinate = startPositionXCoordinate;
            Player.PlayerCoordinate.YCoordinate = startPositionYCoordinate;
            Player.PaintPlayer();
            Game.LoadGameField(startProperties);
        }

        Game.workingGame = false;
        Game.block.SetActive(false);
        if (Game.gameModeLvls)
            GameModeLvls.stopWatch.Start();
    }
}
