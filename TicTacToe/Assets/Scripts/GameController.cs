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
    private string playerSide;
    private string computerSide;
    public GameObject gameOverPanel;
    public Text gameOverText;
    private int isNotInteractable;
    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public GameObject startInfo;
    public bool playerMove;
    public float delay;
    private int value;



    // tournament params
    private string playerId;
    private string otp;
    private string tourneyId;
    private string wallet_address;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetGameControllerReferenceOnButtons();
        isNotInteractable = 0;
        playerMove = true;

        // PlayerPrefs.SetString("test", "Hi this is a test pref");

        SetTourneyOptions();
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

        ExampleManager.Instance.Initialize("TictactoeRoom", roomOptions);

        while (ExampleManager.Instance.IsInRoom == false)
        {
            yield return new WaitForEndOfFrame();
        }

        ExampleManager.Instance.ListenToServerEvents();

        ExampleManager.Instance.GameStart();

        // call validate from server
        ExampleManager.Instance.CallValidate();
    }

    void Update()
    {
        if (playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 100)
            {
                value = Random.Range(0, 8);
                if (buttonList[value].GetComponentInParent<Button>().interactable == true)
                {
                    buttonList[value].text = GetComputerSide();
                    buttonList[value].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                }
            }
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
        playerSide = startingSide;
        if (playerSide == "X")
        {
            computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            computerSide = "X";
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
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

    public void EndTurn()
    {
        isNotInteractable += 1;

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

        else if (isNotInteractable >= 9)
        {
            GameOver("draw");
        }

        else
        {
            ChangeSides();
            delay = 60;
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

        if (ExampleManager.Instance.IsInRoom == true)
        {
            ExampleManager.Instance.GameEnd(100);
        }
    }

    void ChangeSides()
    {
        // playerSide = (playerSide == "X") ? "O" : "X";
        playerMove = (playerMove == true) ? false : true;

        // if (playerSide == "X")
        if (playerMove == true)
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
        isNotInteractable = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        startInfo.SetActive(true);
        playerMove = true;
        delay = 60;

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

    void SetTourneyOptions()
    {
        PlayerPrefs.SetString("testotp", "hi im otp");
        try
        {
            // get required creds from arcadia
            string launchParams = Application.absoluteURL.Split("?"[0])[1];
            string[] parameters = launchParams.Split(char.Parse("&"));
            foreach (string param in parameters)
            {
                string[] temp = param.Split('=');

                if (temp[0].Equals("playerid"))
                {
                    playerId = temp[1];
                }
                else if (temp[0].Equals("otp"))
                {
                    otp = temp[1];
                }
                else if (temp[0].Equals("tourneyid"))
                {
                    tourneyId = temp[1];
                }
                else if (temp[0].Equals("wallet_address"))
                {
                    wallet_address = temp[1];
                }
            }

            PlayerPrefs.SetString("playerId", playerId);
            PlayerPrefs.SetString("otp", otp);
            PlayerPrefs.SetString("tourneyId", tourneyId);
            PlayerPrefs.SetString("wallet_address", wallet_address);
        }
        catch
        {
            Debug.Log("Not in tourney");
        }
    }
}