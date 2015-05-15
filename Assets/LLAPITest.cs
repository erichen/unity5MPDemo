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
	void OnGUI (){
		int buttonfontSize = GUI.skin.button.fontSize;
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;
		int buttonHeight =  (int)(screenHeight * 0.1f) ;
		int buttonWidth  =  (int)(screenWidth * 0.2f) ;
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
		
		GUILayout.BeginArea(new Rect( screenWidth - buttonWidth ,0,buttonWidth,buttonHeight));		
		if(GUILayout.Button("Exit", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth))){
			Application.Quit();
		}	

		GUILayout.EndArea();

		GUI.skin.button.fontSize = buttonfontSize;
	}
}
