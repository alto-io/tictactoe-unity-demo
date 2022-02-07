using System;
using System.Collections.Generic;
using Colyseus;
using LucidSightTools;
using UnityEngine;

public class ExampleManager : ColyseusManager<ExampleManager>
{
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
        _roomController = new ExampleRoomController {roomName = roomName};
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

#region Remote Function Call

    /// <summary>
    ///     Send a Remote Function Call
    /// </summary>
    /// <param name="entity">The entity we want to send the RFC</param>
    /// <param name="function">The name of the function to call</param>
    /// <param name="param">The parameters of the function to call</param>
    /// <param name="target">Who should receive this RFC</param>
    public static void RFC(ColyseusNetworkedEntityView entity, string function, object[] param,
        ExampleRFCTargets target = ExampleRFCTargets.ALL)
    {
        RFC(entity.Id, function, param, target);
    }

    /// <summary>
    ///     Send a Remote Function Call
    /// </summary>
    /// <param name="entityId">The ID of the entity we want to send the RFC</param>
    /// <param name="function">The name of the function to call</param>
    /// <param name="param">The parameters of the function to call</param>
    /// <param name="target">Who should receive this RFC</param>
    public static void RFC(string entityId, string function, object[] param,
        ExampleRFCTargets target = ExampleRFCTargets.ALL)
    {
        NetSend("remoteFunctionCall",
            new ExampleRFCMessage {entityId = entityId, function = function, param = param, target = target});
    }

    public static void CustomServerMethod(string methodName, object[] param)
    {
        NetSend("customMethod", new ExampleCustomMethodMessage {method = methodName, param = param});
    }

    /// <summary>
    ///     Send an action and message object to the room.
    /// </summary>
    /// <param name="action">The action to take</param>
    /// <param name="message">The message object to pass along to the room</param>
    public static void NetSend(string action, object message = null)
    {
        if (Instance._roomController.Room == null)
        {
            LSLog.LogError($"Error: Not in room for action {action} msg {message}");
            return;
        }

        _ = message == null
            ? Instance._roomController.Room.Send(action)
            : Instance._roomController.Room.Send(action, message);
    }

    /// <summary>
    ///     Send an action and message object to the room.
    /// </summary>
    /// <param name="actionByte">The action to take</param>
    /// <param name="message">The message object to pass along to the room</param>
    public static void NetSend(byte actionByte, object message = null)
    {
        if (Instance._roomController.Room == null)
        {
            LSLog.LogError(
                $"Error: Not in room for action bytes msg {(message != null ? message.ToString() : "No Message")}");
            return;
        }

        _ = message == null
            ? Instance._roomController.Room.Send(actionByte)
            : Instance._roomController.Room.Send(actionByte, message);
    }

#endregion Networked Entity Creation
}