using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;

public class myNetworkManagerHUD : MonoBehaviour {
	public bool isAtStartup = true;
	private bool isConnecting = false;
	private bool isError = false;
	public bool m_RunInBackground = true;

	public Transform spawnPos;
	public string connectToIp = "localhost";
	public int connectPort = 7777;
	NetworkClient myClient;
	public GameObject playerPrefab;

	public bool runInBackground
	{
		get
		{
			return this.m_RunInBackground;
		}
		set
		{
			this.m_RunInBackground = value;
		}
	}


	void Awake()
	{
		spawnPos.position.Set(0, 10, 0);
//		NetworkManager.RegisterStartPosition (spawnPos);
		if (m_RunInBackground)
			Application.runInBackground = true;
	}
	// Use this for initialization
	void Start () {
		ClientScene.RegisterPrefab (playerPrefab);
		print ("playerPrefab:localPlayerAuthority:"+playerPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() 
	{
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;
		int buttonHeight =  (int)(screenHeight * 0.1f) ;
		int buttonWidth  =  (int)(screenWidth * 0.3f) ;
		GUI.skin.textField.fontSize = (int)(buttonHeight * 0.6f); //控制Lable字體大小
		GUI.skin.button.fontSize = (int)(buttonHeight * 0.6f); //控制Button字體大小
		int width = 300;
		//		int height = 200;
		if (buttonWidth < 100 ) 
		{
			buttonWidth = 100;
		}
		if (width < buttonWidth) 
		{
			width = buttonWidth;
		}
		
		
		if(GUILayout.Button("MainMenu", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth))){
			Disconnect();
			Application.LoadLevel(0);
		}
		if (isError) {
			GUILayout.Label("Error to Connected to server:");
			if(GUILayout.Button("Return", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth))){
				isError = false;
			}
			return;
		}

		if (isAtStartup)
		{
			if (GUILayout.Button("LanHost", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
			{
				SetupServer();
				SetupLocalClient();
			}
			if (GUILayout.Button("LanClient", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
			{
				SetupClient();
			}
			if (GUILayout.Button("LanServerOnly", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
			{
				SetupServer();
			}
			connectToIp = GUILayout.TextField(connectToIp, GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth));
			connectPort = Convert.ToInt32(GUILayout.TextField(connectPort.ToString(), GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)));
			return;
		}
		if (isConnecting){
			GUILayout.Label("Connect Status: Connecting");
			return;
		}

		if(myClient != null)
		{
			GUILayout.Label("Connection Status:Client");
//			GUILayout.Label("Ping to Server:" + Network.GetAveragePing(Network.connections[0]));
			if (!ClientScene.ready){
				if (GUILayout.Button("Set Ready", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
				{
//					ClientScene.Ready(myClient.connection);
					ClientScene.AddPlayer(myClient.connection, 0);
				}
			}
		}
		if (NetworkServer.active)
		{
			GUILayout.Label("Connection Status:Server");
			GUILayout.Label("Connections:"+NetworkServer.connections.Count);
//			if (Network.connections.Length >= 1)
//				GUILayout.Label("Ping to Server:" + Network.GetAveragePing(Network.connections[0]));
		}
			
		if(GUILayout.Button("Disconnect", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth)))
			Disconnect();

		GUILayout.Label("IP Address:" + connectToIp + ":" + connectPort, GUILayout.Width(buttonWidth*2));

	}

	public void RequestServerSpawn(NetworkConnection conn, short playerControllerId){
		GameObject player = (GameObject)Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation);
		NetworkServer.Spawn (player);
	}

	public void ClientSpawn(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public void Disconnect(){
		isAtStartup = true;
		isConnecting = false;
		if(myClient != null){
			ClientScene.DestroyAllClientObjects();
			myClient.Disconnect();
		}
		if (NetworkServer.active){
			NetworkServer.Reset();
		}
	}
	
	// Create a server and listen on a port
	public void SetupServer()
	{
		NetworkServer.Listen(connectPort);
		NetworkServer.RegisterHandler (MsgType.Ready, OnReady);
		NetworkServer.RegisterHandler (MsgType.AddPlayer, OnAddPlayer);
		NetworkServer.RegisterHandler (MsgType.Disconnect, OnDisconnected);
		isAtStartup = false;
	}
	
	// Create a client and connect to the server port
	public void SetupClient()
	{
		myClient = new NetworkClient();
		myClient.Connect(connectToIp, connectPort);
		RegisterClientHandlers (ref myClient);
		isAtStartup = false;
		isConnecting = true;
	}
	
	// Create a local client and connect to the local server
	public void SetupLocalClient()
	{
		myClient = ClientScene.ConnectLocalServer();
		RegisterClientHandlers (ref myClient);
		isAtStartup = false;
		isConnecting = true;
	}
	private void RegisterClientHandlers(ref NetworkClient myClient){
		myClient.RegisterHandler (MsgType.Connect, OnClientConnected);
		myClient.RegisterHandler (MsgType.Error, OnClientError);
	}
	// client function
	public void OnClientConnected(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server:"+ myClient.GetType()+" RTT:"+ myClient.GetRTT() + " hostid:" + netMsg.conn.hostId);
		isConnecting = false;
	}
	public void OnClientError(NetworkMessage netMsg){
		Debug.Log("Error to Connected to server:");
		isError = true;
		isAtStartup = true;
		ClientScene.DestroyAllClientObjects();
	}
	public void OnReady(NetworkMessage netMsg){
		Debug.Log("OnReady"+ myClient.GetType()+" RTT:"+ myClient.GetRTT() + " hostid:" + netMsg.conn.hostId);
	}
	public void OnAddPlayer(NetworkMessage netMsg){
		AddPlayerMessage msg = netMsg.ReadMessage<AddPlayerMessage>();
		Debug.Log("OnAddPlayer");
		ClientSpawn (netMsg.conn, msg.playerControllerId);
	}
	public void OnDisconnected(NetworkMessage netMsg){
		NetworkServer.DestroyPlayersForConnection (netMsg.conn);
	}

}