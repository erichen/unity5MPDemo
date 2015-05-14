using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;
		int buttonHeight =  (int)(screenHeight * 0.1f) ;
		int buttonWidth  =  (int)(screenWidth * 0.8f) ;
		GUI.skin.label.fontSize = (int)(buttonHeight * 0.5f); //控制Lable字體大小
		GUI.skin.textField.fontSize = (int)(buttonHeight * 0.5f); //控制Lable字體大小
		GUI.skin.button.fontSize = (int)(buttonHeight * 0.8f); //控制Button字體大小
		int width = 300;
		int height = buttonHeight * 4;//200;
		if (width < buttonWidth) 
		{
			width = buttonWidth;
		}

		if (screenHeight < height) 
		{
			height = screenHeight;
		}

		GUILayout.BeginArea(new Rect( screenWidth/2 - width/2 ,screenHeight/2 - height/2 ,width ,height));

		GUILayout.FlexibleSpace();	

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();	
		GUILayout.Label("Unity Networking Demo", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.Space(10);

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Tutorial 1 - P2P", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth))){
			Application.LoadLevel(1);
		}	
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Tutorial 1 - Join", GUILayout.Height(buttonHeight), GUILayout.Width(buttonWidth))){
			Application.LoadLevel(2);
		}	
		GUILayout.EndHorizontal();


		GUILayout.EndArea();
	}
}
