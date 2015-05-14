using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class LLAPITest : MonoBehaviour  {
	public Transform StartPosition;

	void Awake(){
		NetworkManager.RegisterStartPosition (StartPosition);
	}

	void Start () 
	{
		Utility.SetAppID ((AppID)215);
		//NetworkMatch.SetProgramAppID((AppID)XXX);        // AppID you get when you've registered the game
	}
}
