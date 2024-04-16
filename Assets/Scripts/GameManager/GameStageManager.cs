using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageManager : MonoBehaviour
{
    // Stages in the game
    public enum Stage {
        Start, // The start of the game
        Working, // Follow the boss and leading
        Exploring, // Violate the boss's order and explore
        Ending // The end of the game
    }

    private int gameRound = 0;
    public int GameRound {
        get { return gameRound; }
        set { gameRound = value; }
    }
    private Stage currentStage = Stage.Start;
    public Stage CurrentStage {
        get { return currentStage; }
        set { currentStage = value; }
    }

    public void Next() {
        currentStage = NextStage(currentStage);
    }

    private Stage NextStage(Stage stage) {
        int nextValue = ((int)stage+1) % System.Enum.GetValues(typeof(Stage)).Length;
        if (nextValue == (int)Stage.Start) nextValue++;
        return (Stage)nextValue;
    }


}
