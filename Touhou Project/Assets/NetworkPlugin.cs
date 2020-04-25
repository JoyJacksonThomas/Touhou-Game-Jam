using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;
using System.Text;

public class NetworkPlugin : MonoBehaviour
{
    public enum MessageIDs
    {
        CONNECTED_TO_SERVER = 135,
        MOVE_CURSOR,
    }

    public enum InputIDs
    {

        ESCAPE = 0,
        HORIZONTAL,
        VERTICAL,
        JUMP,
        PAUSE,
        ATTACK,
        SPECIAL,
        COUNT

    }


    public const int INPUT_IDS_COUNT = (int)InputIDs.COUNT;
    string[] inputIdentifier = { "Escape",
    "Horizontal",
    "Vertical",
    "Jump",
    "Pause",
    "Attack",
    "Special"};


    [DllImport("NetworkingPlugin")]
    private static extern int Test();

    [DllImport("NetworkingPlugin")]
    private static extern int StartupNetwork(int isServer);

    [DllImport("NetworkingPlugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern void GetPacketsFromPeer(out int size, out IntPtr connectionMessageArrays);

    [DllImport("NetworkingPlugin", CallingConvention = CallingConvention.Cdecl)]
    private static extern int SendGameMessages(IntPtr[] toSendMessages, int size);

    [DllImport("NetworkingPlugin")]
    private static extern int Connect(string ip);
    // Start is called before the first frame update

    [DllImport("NetworkingPlugin")]
    private static extern void ExitNetworking();

    [SerializeField] Text output;
    [SerializeField] GameObject startupPanel;
    [SerializeField] GameObject cursorPanelParent;
    [SerializeField] GameObject cursorPrefab;

    public bool userConnected = false;
    public int userIdentifier = -1;
    public float networkTimeInterval = 1f;
    float currentTimeInterval = 0;

    [SerializeField] Dictionary<int, GameObject> Players = new Dictionary<int, GameObject>();


    public static NetworkPlugin Instance = null;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        isServer = true;


    }

    private void OnDestroy()
    {
        Debug.Log("Deleteting networking Instances..");
        try
        {

            ExitNetworking();
        }
        catch
        {
            Debug.LogError("Delete Failed");
        }
        Debug.Log("Delete Succsesful");
    }

    void Update()
    {
        if (!Connected)
            return;

        

        int size;
        IntPtr outMessages;

        List<ConnectionMessage> outputMessages = new List<ConnectionMessage>();

        if (userConnected)
        {
            HandleInput(outputMessages);
        }


        GetPacketsFromPeer(out size, out outMessages);
        ConnectionMessage[] messagesArray = new ConnectionMessage[size];
        IntPtr current = outMessages;

        for (int i = 0; i < size; i++)
        {
            messagesArray[i] = new ConnectionMessage();
            messagesArray[i] = (ConnectionMessage)Marshal.PtrToStructure(current, typeof(ConnectionMessage));

            //Marshal.FreeCoTaskMem((IntPtr)Marshal.ReadInt32(current));   
            //Marshal.DestroyStructure(current, typeof(ConnectionMessage));

            switch (messagesArray[i].MessageID)
            {
                case 16:
                    {
                        Debug.Log("Connected to server");
                        output.text = "Connected To server";
                        if (!isServer)
                        {
                            startupPanel.SetActive(false);
                            ConnectionMessage newMessage = new ConnectionMessage();
                            newMessage.MessageID = (int)MessageIDs.CONNECTED_TO_SERVER;
                            newMessage.playerID = userIdentifier;
                            Debug.Log("User : " + newMessage.playerID);
                            newMessage.inputStates = new int[INPUT_IDS_COUNT];
                            outputMessages.Add(newMessage);
                            GameObject player2 = (GameObject)Instantiate(cursorPrefab, cursorPanelParent.transform);
                            player2.GetComponent<CursorScript>().isClient = true;

                            Players.Add(1, player2);
                            userConnected = true;

                        }
                    }
                    break;
                case 17:
                    {
                        Debug.Log("Connection Failed");
                        output.text = "Connection Failed";
                    }
                    break;
                case 19:
                    {
                        Debug.Log("Client Connected");
                        output.text = "Client Connected";
                        GameObject player2 = (GameObject)Instantiate(cursorPrefab, cursorPanelParent.transform);
                        player2.GetComponent<CursorScript>().isClient = true;

                        Players.Add(0, player2);
                    }
                    break;
                case (int)MessageIDs.CONNECTED_TO_SERVER:
                    {
                        Debug.Log("User Has Joined Game");
                        output.text = "User Has Joined Game";
                        if (!Players.ContainsKey(0))
                        {

                            GameObject player2 = (GameObject)Instantiate(cursorPrefab, cursorPanelParent.transform);
                            player2.GetComponent<CursorScript>().isClient = true;

                            Players.Add(0, player2);
                        }
                        userConnected = true;
                    }
                    break;
                case (int)MessageIDs.MOVE_CURSOR:
                    {
                        if (!Players.ContainsKey(messagesArray[i].playerID))
                        {
                            Debug.LogError("ERROR NO ID : " + messagesArray[i].playerID);

                            output.text = "ERROR NO ID : " + messagesArray[i].playerID;
                            continue;
                        }

                        GameObject pl = Players[messagesArray[i].playerID];

                        Vector2 pos = pl.transform.position;
                        //pos.x += (2 * messagesArray[i].inputStates[(int)InputIDs.HORIZONTAL]);
                        //pos.y += (2 * messagesArray[i].inputStates[(int)InputIDs.VERTICAL]);

                        pos.x = messagesArray[i].xPos;
                        pos.y = messagesArray[i].yPos;
                        //pl.GetComponent<Image>().color = Color.green;


                        if (messagesArray[i].inputStates[(int)InputIDs.ATTACK] == 2)
                        {
                            //pl.GetComponent<Image>().color = Color.red;
                            pl.GetComponent<CursorScript>().CheckOver();
                        }



                        pl.transform.position = pos;
                    }
                    break;

                default:
                    {

                        //Debug.Log("Unknown Descriptor : " + messagesArray[i].MessageID);
                    }
                    break;
            }


            current = (IntPtr)((long)current + Marshal.SizeOf(messagesArray[i]));

        }

        if (outputMessages.Count > 0)
        {
            ConnectionMessage[] messages = outputMessages.ToArray();
            int nCount = messages.Length;
            IntPtr outMessagePTR = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ConnectionMessage)) * nCount);
            IntPtr[] ptrArr = new IntPtr[nCount];
            for (int nI = 0; nI < nCount; nI++)
            {
                ptrArr[nI] = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(ConnectionMessage)));
                Marshal.StructureToPtr(messages[nI], ptrArr[nI], true);
            }


            Debug.LogError("Sending messages result : " + SendGameMessages(ptrArr, nCount));
        }




    }

    public bool isServer;
    public bool Connected;

    string textLine;
    public void EnterIP(Text input)
    {
        if (Connected)
            return;
        Debug.Log("Startup Result " + StartupNetwork(isServer ? 1 : 0));
        int res;
        if (input.text.Length < 1)
        {

            Debug.Log("Connect Result " + (res = Connect("10.0.0.1")));
        }
        else
        {

            Debug.Log("Connect Result " + (res = Connect(input.text)));
        }
        Connected = true;


        if (!isServer)
        {
            userIdentifier = 0;
        }
        else
        {
            startupPanel.SetActive(false);
            userIdentifier = 1;
        }
        var player = GameObject.FindGameObjectWithTag("Player");

        Players.Add(userIdentifier, player);

    }


    public void IsServer(Toggle toggleField)
    {
        isServer = toggleField.isOn;
    }

    public void SendMessages(List<ConnectionMessage> outputMessages)
    {
        if (outputMessages.Count > 0)
        {
            ConnectionMessage[] messages = outputMessages.ToArray();
            int nCount = messages.Length;
            IntPtr outMessagePTR = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ConnectionMessage)) * nCount);
            IntPtr[] ptrArr = new IntPtr[nCount];
            for (int nI = 0; nI < nCount; nI++)
            {
                ptrArr[nI] = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(ConnectionMessage)));
                Marshal.StructureToPtr(messages[nI], ptrArr[nI], true);
            }


            Debug.LogError("Sending messages result : " + SendGameMessages(ptrArr, nCount));
        }
    }

    void HandleInput(List<ConnectionMessage> outputMessages)
    {
        bool sendMessage = false;
        for (int i = 0; i < INPUT_IDS_COUNT; i++)
        {
            if (Mathf.Abs((int)Input.GetAxisRaw(PlayerController.prefix[0] + inputIdentifier[i])) == 1 && currentTimeInterval > networkTimeInterval)
            { sendMessage = true; break; }
            if (Input.GetButtonDown(PlayerController.prefix[0] + inputIdentifier[i]))
            { sendMessage = true; break; }
            if (Input.GetButtonUp(PlayerController.prefix[0] + inputIdentifier[i]))
            { sendMessage = true; break; }
        }
        if (sendMessage)   //(sendMessage)
        {
            Debug.Log("Send Input Message");
            ConnectionMessage positionMessage = new ConnectionMessage();
            positionMessage.MessageID = (int)MessageIDs.MOVE_CURSOR;
            positionMessage.playerID = userIdentifier;
            positionMessage.inputStates = new int[INPUT_IDS_COUNT];

            positionMessage.xPos = Players[userIdentifier].transform.position.x;
            positionMessage.yPos = Players[userIdentifier].transform.position.y;

            for (int i = 0; i < INPUT_IDS_COUNT; i++)
            {
                
                if (currentTimeInterval > networkTimeInterval)
                    positionMessage.inputStates[i] = (int)Input.GetAxisRaw(PlayerController.prefix[0] + inputIdentifier[i]);

                if (Input.GetButtonDown(PlayerController.prefix[0] + inputIdentifier[i]))
                {
                    positionMessage.inputStates[i] = 2;

                    //GameObject p1 = Players[userIdentifier];
                    //p1.GetComponent<Image>().color = Color.red;
                    //p1.GetComponent<CursorScript>().CheckOver();

                }
                if (Input.GetButtonUp(PlayerController.prefix[0] + inputIdentifier[i]))
                {
                    positionMessage.inputStates[i] = 3;
                }

            }
            outputMessages.Add(positionMessage);
        }

        if (currentTimeInterval < networkTimeInterval)
        {
            currentTimeInterval += Time.deltaTime;
        }
        else
        {
            currentTimeInterval = 0;
        }
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct ConnectionMessage
{
    public int MessageID;
    public float xPos;
    public float yPos;
    public int inputID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    public int[] inputStates;

    public int playerID;
}

