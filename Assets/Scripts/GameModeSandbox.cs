using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameModeSandbox : MonoBehaviour
{
    public void GameModeSandboxClick()
    {
        TimeSpan ts = GameModeLvls.stopWatch.Elapsed;
        Game.CurrentTime += ts.Minutes * 60 + ts.Hours * 360 + ts.Seconds;
        GameModeLvls.stopWatch.Reset();
        Game.ChangeTimeLevel();
        Game.callSave();

        Game.gameModeLvlButton.SetActive(true);
        Game.ClearGameField();
        Game.gameModeLvls = false;

    }
    
}
