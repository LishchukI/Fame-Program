using Gravitons.UI.Modal;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public Player(int xCoordinate = 0, int yCoordinate = 0)
    {
        PlayerCoordinate.XCoordinate = xCoordinate;
        PlayerCoordinate.YCoordinate = yCoordinate;
        PaintPlayer();
    }

    public class PlayerCoordinate
    {
        public static int XCoordinate;
        public static int YCoordinate;
    }



    //Methods
    public static void PaintPlayer()
    {
        Game.field[PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate].transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Image>().sprite = Game.images[0].sprite;
        Game.field[PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate].transform.GetChild(0).transform.GetChild(0).tag = "Player";
        Game.field[PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate].transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255); ;
    }

    public static void ErasePlayer()
    {
        Game.field[PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate].transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
        Game.field[PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate].transform.GetChild(0).transform.GetChild(0).tag = "Untagged";
        Game.field[PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate].transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 0);
    }

    public void CheckEndGame()
    {
        if (Game.workingGame)
            StartWork.indexAlgorithm = AlgorithmList.algorithmList.Count + 2;
    }

    public void MoveDown()
    {
        if (PlayerCoordinate.XCoordinate + 1 != Game.countRow)
        {
            if (!CheckObstacle(PlayerCoordinate.XCoordinate + 1, PlayerCoordinate.YCoordinate))
            {
                ErasePlayer();
                PlayerCoordinate.XCoordinate++;
                PaintPlayer();
            }
            else
            {
                ModalManager.Show("Помилка!", "Персонаж не може стати на клітку з перешкодою!", new[] { new ModalButton() { Text = "Добре" } });
                CheckEndGame();
            }
        } 
        else
        {
            ModalManager.Show("Помилка!", "Персонаж не може вийти за межі ігрового поля!", new[] { new ModalButton() { Text = "Добре" } });
            CheckEndGame();
        }
    }

    public void MoveUp()
    {
        if (PlayerCoordinate.XCoordinate - 1 >= 0)
        {
            if (!CheckObstacle(PlayerCoordinate.XCoordinate - 1, PlayerCoordinate.YCoordinate))
            {
                ErasePlayer();
                PlayerCoordinate.XCoordinate--;
                PaintPlayer();   
            }
            else
            {
                ModalManager.Show("Помилка!", "Персонаж не може стати на клітку з перешкодою!", new[] { new ModalButton() { Text = "Добре" } });
                CheckEndGame();
            }
        }
        else
        {
            ModalManager.Show("Помилка!", "Персонаж не може вийти за межі ігрового поля!", new[] { new ModalButton() { Text = "Добре" } });
            CheckEndGame();
        }
}

    public void MoveRight()
    {
        if (PlayerCoordinate.YCoordinate + 1 != Game.countColumn)
        {
            if (!CheckObstacle(PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate + 1))
            {
                ErasePlayer();
                PlayerCoordinate.YCoordinate++;
                PaintPlayer();
            }
            else
            {
                ModalManager.Show("Помилка!", "Персонаж не може стати на клітку з перешкодою!", new[] { new ModalButton() { Text = "Добре" } });
                CheckEndGame();
            }
        }
        else
        {
            ModalManager.Show("Помилка!", "Персонаж не може вийти за межі ігрового поля!", new[] { new ModalButton() { Text = "Добре" } });
            CheckEndGame();
        }
}

    public void MoveLeft()
    {
        if (PlayerCoordinate.YCoordinate - 1 >= 0)
        {
            if (!CheckObstacle(PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate - 1))
            {
                ErasePlayer();
                PlayerCoordinate.YCoordinate--;
                PaintPlayer();
            }
            else
            {
                ModalManager.Show("Помилка!", "Персонаж не може стати на клітку з перешкодою!", new[] { new ModalButton() { Text = "Добре" } });
                CheckEndGame();
            }
        }
        else
        { 
            ModalManager.Show("Помилка!", "Персонаж не може вийти за межі ігрового поля!", new[] { new ModalButton() { Text = "Добре" } });
            CheckEndGame();
        }
    }

    private bool CheckObstacle(int xCoordinate, int yCoordinate)
    {
        if (Game.field[xCoordinate, yCoordinate].transform.GetChild(0).GetComponentInChildren<Image>().sprite == Game.images[2].sprite)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckTag(int xCoordinate, int yCoordinate)
    {
        if (Game.field[xCoordinate, yCoordinate].transform.GetChild(0).GetComponentInChildren<Image>().sprite == Game.images[1].sprite)
            return true;
        else
            return false;
    }


    public void If()
    {
        MethodInfo nameInstructionMenthodIf = Game.player.GetType().GetMethod(AlgorithmList.algorithmList[StartWork.indexAlgorithm].ExtraNameInstruction);
        nameInstructionMenthodIf.Invoke(Game.player, null);
    }

    public void CheckIfWithouInstruction()
    {
        if(AlgorithmList.algorithmList[StartWork.indexAlgorithm + 1].NameInstruction == "Else")
        {
            StartWork.indexAlgorithm++;
            ExitBodyCondition();
        }
    }

    public void ObstacleRight()
    {
        if (PlayerCoordinate.YCoordinate + 1 != Game.countColumn)
        {
            if (!CheckObstacle(PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate + 1))
            {
                ExitBodyCondition();
            }
            else
            {
                CheckIfWithouInstruction();
            }
        }
        else
        {
            ExitBodyCondition();
        }
    }

    public void ObstacleLeft()
    {
        if (PlayerCoordinate.YCoordinate + 1 != Game.countColumn)
        {
            if (!CheckObstacle(PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate - 1))
            {
                ExitBodyCondition();
            }
            else
            {
                CheckIfWithouInstruction();
            }
        }
        else
        {
            ExitBodyCondition();
        }
    }

    public void ObstacleDown()
    {
        if (PlayerCoordinate.XCoordinate + 1 != Game.countRow)
        {
            if (!CheckObstacle(PlayerCoordinate.XCoordinate + 1, PlayerCoordinate.YCoordinate))
            {
                ExitBodyCondition();
            }
            else
            {
                CheckIfWithouInstruction();
            }
        }
        else
        {
            ExitBodyCondition();
        }
    }

    public void ObstacleUp()
    {
        if (PlayerCoordinate.YCoordinate - 1 != Game.countColumn)
        {
            if (!CheckObstacle(PlayerCoordinate.XCoordinate - 1, PlayerCoordinate.YCoordinate))
            {
                ExitBodyCondition();
            }
            else
            {
                CheckIfWithouInstruction();
            }
        }
        else
        {
            ExitBodyCondition();
        }
    }

    public void ObstacleNotRight()
    {
        if (PlayerCoordinate.YCoordinate + 1 != Game.countColumn)
        {
            if (CheckObstacle(PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate + 1))
            {
                ExitBodyCondition();
            }
            else
            {
                CheckIfWithouInstruction();
            }
        }
        else
        {
            CheckIfWithouInstruction();
        }
    }

    public void ObstacleNotLeft()
    {
        if (PlayerCoordinate.YCoordinate + 1 != Game.countColumn)
        {
            if (CheckObstacle(PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate - 1))
            {
                ExitBodyCondition();
            }
            else
            {
                CheckIfWithouInstruction();
            }
        }
        else
        {
            CheckIfWithouInstruction();
        }
    }

    public void ObstacleNotDown()
    {
        if (PlayerCoordinate.XCoordinate + 1 != Game.countRow)
        {
            if (CheckObstacle(PlayerCoordinate.XCoordinate + 1, PlayerCoordinate.YCoordinate))
            {
                ExitBodyCondition();
            }
            else
            {
                CheckIfWithouInstruction();
            }
        }
        else
        {
            CheckIfWithouInstruction();
        }
    }

    public void ObstacleNotUp()
    {
        if (PlayerCoordinate.YCoordinate - 1 != Game.countColumn)
        {
            if (CheckObstacle(PlayerCoordinate.XCoordinate - 1, PlayerCoordinate.YCoordinate))
            {
                ExitBodyCondition();
            }
            else
            {
                CheckIfWithouInstruction();
            }
        }
        else
        {
            CheckIfWithouInstruction();
        }
    }

    public void FieldWtihTag()
    {
        if(!CheckTag(PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate))
        {
            ExitBodyCondition();
        }
        else
        {
            CheckIfWithouInstruction();
        }
    }

    public void FieldEmpty()
    {
        if (CheckTag(PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate))
        {
            ExitBodyCondition();
        }
        else
        {
            CheckIfWithouInstruction();
        }
    }

    public void Else()
    {
        ExitBodyCondition();
    }

    public void EndCondition()
    {

    }

    public void RepeatTimes()
    {
        if (Convert.ToInt32(AlgorithmList.algorithmList[StartWork.indexAlgorithm].ExtraNameInstruction) == 0)
            ExitBodyLoop();
        else
        {
            int currentTimeLoop = Convert.ToInt32(AlgorithmList.algorithmList[StartWork.indexAlgorithm].ExtraNameInstruction) - 1;
            AlgorithmList.algorithmList[StartWork.indexAlgorithm].ExtraNameInstruction = Convert.ToString(currentTimeLoop);
            LocalizationManager.SetLanguage(LocalizationManager.SelectedLanguage);
            AlgorithmList.algorithmList[StartWork.indexAlgorithm].GameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text += " " + AlgorithmList.algorithmList[StartWork.indexAlgorithm].ExtraNameInstruction;
        }
    }

    public void EndLoop()
    {
        GoToStartLoop();
    }

    public void PutChest()
    {
        Game.ChangeFieldProperties(Game.field[PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate], Game.tagNameFieldByNewFieldProperties["Empty"]);
    }

    public void PickUpChest()
    {
        Game.ChangeFieldProperties(Game.field[PlayerCoordinate.XCoordinate, PlayerCoordinate.YCoordinate], Game.tagNameFieldByNewFieldProperties["Barrier"]);
    }
    

    private void ExitBodyCondition()
    {
        for (int i = StartWork.indexAlgorithm + 1; i < AlgorithmList.algorithmList.Count; i++)
        {
            if (AlgorithmList.algorithmList[i].NameInstruction == "EndCondition" || AlgorithmList.algorithmList[i].NameInstruction == "Else")
            {
                StartWork.indexAlgorithm = i;
                return;
            }
        }
    }

    private void ExitBodyLoop()
    {
        for (int i = StartWork.indexAlgorithm + 1; i < AlgorithmList.algorithmList.Count; i++)
        {
            if (AlgorithmList.algorithmList[i].NameInstruction == "EndLoop")
            {
                StartWork.indexAlgorithm = i;
                return;
            }
        }
    }

    private void GoToStartLoop()
    {
        for (int i = StartWork.indexAlgorithm - 1; i >= 0; i--)
        {
            if (AlgorithmList.algorithmList[i].NameInstruction == "RepeatTimes")
            {
                StartWork.indexAlgorithm = i - 1;
                return;
            }
        }
    }

    /*
    private void TransferToElse(int index)
    {
        if (index + 1 != AlgorithmList.algorithmList.Count)
        {
            if (AlgorithmList.algorithmList[index + 1].NameInstruction == "Else")
            {
                StartWork.indexAlgorithm = index + 2;
            }
        }
        else
            StartWork.indexAlgorithm = index;
    }
    */
}
