using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Colyseus;
using System.Threading.Tasks;

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    private int numOfUsedButtons;
    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;

    private string playerSide;
    private string computerSide;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetGameControllerReferenceOnButtons();
    }

    private IEnumerator Start()
    {
        Debug.Log("GameController Start method");
        while (!ExampleManager.IsReady)
        {
            yield return new WaitForEndOfFrame();
        }

        // Colyseus Room options
        Dictionary<string, object> roomOptions = new Dictionary<string, object>
        {
            ["playerId"] = "playerId1", //The name of our custom logic file
            ["tourneyId"] = "tourneyId1",
            ["token"] = "myToken",
            ["finalScore"] = "100"
        };

        ExampleManager.Instance.Initialize("my_room", roomOptions);

        while (ExampleManager.Instance.IsInRoom == false)
        {
            yield return new WaitForEndOfFrame();
        }

        ExampleManager.Instance.ListenToServerEvents();
    }

    void Update()
    {
        if (ExampleManager.Instance.GetPlayerMove() == false)
        {
            Debug.Log("Player is false");
            int botChosenPos = ExampleManager.Instance.GetBotChosenPos();

            if (buttonList[botChosenPos].GetComponentInParent<Button>().interactable == true)
            {
                buttonList[botChosenPos].text = ExampleManager.Instance.GetComputerSide();
                buttonList[botChosenPos].GetComponentInParent<Button>().interactable = false;
                EndTurn();
            }

            ExampleManager.Instance.MoveBot(Random.Range(0, 8));

        }
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        ExampleManager.Instance.SetSides(startingSide);

        if (startingSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }

        StartGame();
    }

    void StartGame()
    {
        if (ExampleManager.Instance.IsInRoom == true)
        {
            SetBoardState(true);
            SetPlayerButtons(false);
            startInfo.SetActive(false);
            ExampleManager.Instance.CallAwake();

            this.playerSide = ExampleManager.Instance.GetPlayerSide();
            this.computerSide = ExampleManager.Instance.GetComputerSide();
        }
    }

    public string GetPlayerSide()
    {
        return ExampleManager.Instance.GetPlayerSide();
    }

    public string GetComputerSide()
    {
        return ExampleManager.Instance.GetComputerSide();
    }

    public void EndTurn()
    {
        // numOfUsedButtons += 1;
        ExampleManager.Instance.IncreaseNumOfUsedButtons();
        Debug.Log(playerSide);

        if (
            buttonList[0].text == playerSide &&
            buttonList[1].text == playerSide &&
            buttonList[2].text == playerSide
        )
        {
            GameOver(playerSide);
        }

        else if (
            buttonList[3].text == playerSide &&
            buttonList[4].text == playerSide &&
            buttonList[5].text == playerSide
        )
        {
            GameOver(playerSide);
        }

        else if (
            buttonList[6].text == playerSide &&
            buttonList[7].text == playerSide &&
            buttonList[8].text == playerSide
        )
        {
            GameOver(playerSide);
        }

        else if (
            buttonList[0].text == playerSide &&
            buttonList[3].text == playerSide &&
            buttonList[6].text == playerSide
        )
        {
            GameOver(playerSide);
        }

        else if (
            buttonList[1].text == playerSide &&
            buttonList[4].text == playerSide &&
            buttonList[7].text == playerSide
        )
        {
            GameOver(playerSide);
        }

        else if (
            buttonList[2].text == playerSide &&
            buttonList[5].text == playerSide &&
            buttonList[8].text == playerSide
        )
        {
            GameOver(playerSide);
        }

        else if (
            buttonList[0].text == playerSide &&
            buttonList[4].text == playerSide &&
            buttonList[8].text == playerSide
        )
        {
            GameOver(playerSide);
        }

        else if (
            buttonList[2].text == playerSide &&
            buttonList[4].text == playerSide &&
            buttonList[6].text == playerSide
        )
        {
            GameOver(playerSide);
        }

        //

        else if (
            buttonList[0].text == computerSide &&
            buttonList[1].text == computerSide &&
            buttonList[2].text == computerSide
        )
        {
            GameOver(computerSide);
        }

        else if (
            buttonList[3].text == computerSide &&
            buttonList[4].text == computerSide &&
            buttonList[5].text == computerSide
        )
        {
            GameOver(computerSide);
        }

        else if (
            buttonList[6].text == computerSide &&
            buttonList[7].text == computerSide &&
            buttonList[8].text == computerSide
        )
        {
            GameOver(computerSide);
        }

        else if (
            buttonList[0].text == computerSide &&
            buttonList[3].text == computerSide &&
            buttonList[6].text == computerSide
        )
        {
            GameOver(computerSide);
        }

        else if (
            buttonList[1].text == computerSide &&
            buttonList[4].text == computerSide &&
            buttonList[7].text == computerSide
        )
        {
            GameOver(computerSide);
        }

        else if (
            buttonList[2].text == computerSide &&
            buttonList[5].text == computerSide &&
            buttonList[8].text == computerSide
        )
        {
            GameOver(computerSide);
        }

        else if (
            buttonList[0].text == computerSide &&
            buttonList[4].text == computerSide &&
            buttonList[8].text == computerSide
        )
        {
            GameOver(computerSide);
        }

        else if (
            buttonList[2].text == computerSide &&
            buttonList[4].text == computerSide &&
            buttonList[6].text == computerSide
        )
        {
            GameOver(computerSide);
        }

        else if (ExampleManager.Instance.GetNumOfUsedButtons() >= 9)
        {
            GameOver("draw");
        }

        else
        {
            ChangeSides();
        }
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

    void GameOver(string winner)
    {
        gameOverPanel.SetActive(true);
        restartButton.SetActive(true);
        ShowTurnIndicators(false);

        if (winner == "draw")
        {
            gameOverText.text = "It's a draw!";
            SetPlayerColorsInactive();
        }
        else
        {
            gameOverText.text = winner + " Wins!";
            SetBoardState(false);
        }
    }

    void ChangeSides()
    {
        // playerSide = (playerSide == "X") ? "O" : "X";
        // playerMove = (playerMove == true) ? false : true;
        ExampleManager.Instance.SetPlayerMove();

        // if (playerSide == "X")
        if (ExampleManager.Instance.GetPlayerMove() == true)
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    public void RestartGame()
    {
        ShowTurnIndicators(true);
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);

        ExampleManager.Instance.RestartGame();

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
    }

    void SetBoardState(bool state)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = state;
            if (state == true)
            {
                buttonList[i].text = "";
            }
        }
    }

    void ShowTurnIndicators(bool state)
    {
        playerX.panel.enabled = state;
        playerO.panel.enabled = state;

        if (!state)
        {
            playerX.text.text = "";
            playerO.text.text = "";
        }
        else
        {
            playerX.text.text = "X";
            playerO.text.text = "O";
        }
    }

    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.text.color = inactivePlayerColor.textColor;
        playerO.panel.color = inactivePlayerColor.panelColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }
}
