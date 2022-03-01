using System;
using System.Collections.Generic;
using Colyseus;
using LucidSightTools;
using UnityEngine;

public class ExampleManager : ColyseusManager<ExampleManager>
{
    // room states
    private string playerSide;
    private string computerSide;
    private int numOfUsedButtons;
    private bool playerMove;
    private float delay;
    private int botChosenPos;

    public delegate void OnRoomsReceived(ColyseusRoomAvailable[] rooms);

    public static OnRoomsReceived onRoomsReceived;

    [SerializeField]
    private ExampleRoomController _roomController;

    public bool autoJoinRoom = false;

    private bool isInitialized;

    /// <summary>
    ///     Returns true if there is an active room.
    /// </summary>
    public bool IsInRoom
    {
        get { return _roomController.Room != null; }
    }

    /// <summary>
    ///     Returns the synchronized time from the server in milliseconds.
    /// </summary>
    public double GetServerTime
    {
        get { return _roomController.GetServerTime; }
    }

    /// <summary>
    ///     Returns the synchronized time from the server in seconds.
    /// </summary>
    public double GetServerTimeSeconds
    {
        get { return _roomController.GetServerTimeSeconds; }
    }

    /// <summary>
    ///     The latency in milliseconds between client and server.
    /// </summary>
    public double GetRoundtripTime
    {
        get { return _roomController.GetRoundtripTime; }
    }

    public static bool IsReady
    {
        get
        {
            return Instance != null; // && Instance.client != null;
        }
    }

    private string userName;

    /// <summary>
    ///     The display name for the user
    /// </summary>
    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }

    /// <summary>
    ///     <see cref="MonoBehaviour" /> callback when a script is enabled just before any of the Update methods are called the
    ///     first time.
    /// </summary>
    protected override void Start()
    {
        Debug.Log("ExampleManager Start method");
        // For this example we're going to set the target frame rate
        // and allow the app to run in the background for continuous testing.
        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        InitializeClient();
    }

    public void Initialize(string roomName, Dictionary<string, object> roomOptions)
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;
        // Set up room controller
        _roomController = new ExampleRoomController { roomName = roomName };
        _roomController.SetRoomOptions(roomOptions);
        _roomController.SetDependencies(_colyseusSettings);
        // Set up Networked Entity Factory
    }

    /// <summary>
    /// /// Create a new <see cref="ColyseusClient"/> along with any other client initialization you may need to perform
    /// /// </summary>
    public override void InitializeClient()
    {
        Debug.Log("Client initializing...");
        base.InitializeClient();
        Debug.Log(client);
        _roomController.SetClient(client);
        if (autoJoinRoom)
        {
            _roomController.JoinOrCreateRoom();
        }
    }

    /// <summary>
    ///     Frame-rate independent message for physics calculations.
    /// </summary>
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _roomController.IncrementServerTime();
    }

    public async void GetAvailableRooms()
    {
        ColyseusRoomAvailable[] rooms = await client.GetAvailableRooms(_roomController.roomName);

        onRoomsReceived?.Invoke(rooms);
    }

    public async void JoinExistingRoom(string roomID)
    {
        await _roomController.JoinRoomId(roomID);
    }

    public async void CreateNewRoom(string roomID)
    {
        await _roomController.CreateSpecificRoom(client, _roomController.roomName, roomID);
    }

    public void CreateNewRoom(string roomID, Dictionary<string, object> roomOptions)
    {
        _roomController.SetRoomOptions(roomOptions);

        CreateNewRoom(roomID);
    }

    public async void JoinOrCreateRoom()
    {
        await _roomController.JoinOrCreateRoom();
    }

    public async void LeaveAllRooms(Action onLeave)
    {
        await _roomController.LeaveAllRooms(true, onLeave);
    }


    /// <summary>
    ///     On detection of <see cref="OnApplicationQuit" /> will disconnect
    ///     from all <see cref="rooms" />.
    /// </summary>
    private void CleanUpOnAppQuit()
    {
        if (client == null)
        {
            return;
        }

        _roomController.CleanUp();
    }

    /// <summary>
    ///     <see cref="MonoBehaviour" /> callback that gets called just before app exit.
    /// </summary>
    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        _roomController.LeaveAllRooms(true);

        CleanUpOnAppQuit();
    }

#if UNITY_EDITOR
    public void OnEditorQuit()
    {
        OnApplicationQuit();
    }
#endif

    // functions for tictactoe
    public void TestSend()
    {
        _roomController.Room.Send("type", _roomController.Room.State.mySynchronizedProperty);
    }

    public void GameStart()
    {
        _roomController.Room.Send("start");
    }

    public void GameEnd(int score)
    {
        Debug.Log(PlayerPrefs.GetString("playerID"));
        _roomController.Room.Send("end", score);
    }

    public void CallValidate()
    {
        string otp = PlayerPrefs.GetString("otp");
        string playerId = PlayerPrefs.GetString("playerId");
        string tourneyId = PlayerPrefs.GetString("tourneyId");

        var validateOptions = new {
            otp = otp,
            playerId = playerId,
            tourneyId = tourneyId
        };

        _roomController.Room.Send("validate", validateOptions);
    }

    public void MoveBot(int chosenPos)
    {
        _roomController.Room.Send("moveBot", chosenPos);
    }

    public void IncreaseNumOfUsedButtons()
    {
        _roomController.Room.Send("increaseUsedBtns");
    }

    public void SetPlayerMove()
    {
        _roomController.Room.Send("setPlayerMove");
    }

    public void SetSides(string playerSide)
    {
        _roomController.Room.Send("setSides", playerSide);
    }

    public void RestartGame()
    {
        _roomController.Room.Send("restartGame");
    }

    public bool GetPlayerMove()
    {
        return this.playerMove;
    }

    public float GetDelay()
    {
        return this.delay;
    }

    public string GetComputerSide()
    {
        return this.computerSide;
    }

    public string GetPlayerSide()
    {
        Debug.Log(this.playerSide);
        return this.playerSide;
    }

    public int GetBotChosenPos()
    {
        return this.botChosenPos;
    }

    public int GetNumOfUsedButtons()
    {
        return this.numOfUsedButtons;
    }

    public void SeeStates()
    {
        Debug.Log(_roomController.Room.State.playerMove);
    }


    public void ListenToServerEvents()
    {
        _roomController.Room.State.OnChange += (changes) =>
        {
            changes.ForEach((obj) =>
            {
                if (obj.Field == "playerSide")
                {
                    Debug.Log("playerside change");
                    Debug.Log(obj.Value.ToString());
                    this.playerSide = obj.Value.ToString();
                }
                else if (obj.Field == "computerSide")
                {
                    this.computerSide = obj.Value.ToString();
                }
                else if (obj.Field == "numOfUsedButtons")
                {
                    this.numOfUsedButtons = int.Parse(obj.Value.ToString());
                }
                else if (obj.Field == "playerMove")
                {
                    this.playerMove = (bool) obj.Value;
                }
                else if (obj.Field == "delay")
                {
                    this.delay = float.Parse(obj.Value.ToString());
                }
                else if (obj.Field == "botChosenPos")
                {
                    this.botChosenPos = int.Parse(obj.Value.ToString());
                }
            });
        };
    }
}