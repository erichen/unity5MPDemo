using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {
	public float speed = 10;
	public GameObject particle;

	[Command]
	void CmdMoveTo(Vector3 pos, Vector3 moveDir){
		transform.position = pos;
		transform.Translate(speed * moveDir * Time.deltaTime );
		RpcSyncPos (transform.position);
		RpcMoveTo (moveDir);
	}
	[ClientRpc]
	void RpcSyncPos(Vector3 pos){
		if (isLocalPlayer) {
			return;
		}
		transform.position = pos;
	}
	[ClientRpc]
	void RpcMoveTo(Vector3 moveDir){
		if (isLocalPlayer) {
			return;
		}
		transform.Translate(speed * moveDir * Time.deltaTime );
	}

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) 
		{
			return;
		}
		Input.simulateMouseWithTouches = true;
	}
	// 

	// Update is called once per frame
	void Update () 	{
		if (!isLocalPlayer) 
		{
			return;
		}
//		print (" localPlayer:" + isLocalPlayer + " isServer:" + isServer + " isClient:" + isClient);

		checkMouse();
		checkTouch ();
		checkKeyboard ();
	}
	void checkKeyboard(){
		Vector3 moveDir = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		if (moveDir == Vector3.zero){
			return;
		}
		transform.Translate(speed * moveDir * Time.deltaTime );
		CmdMoveTo (transform.position, moveDir);
	}
	void checkMouse(){
		if (Input.GetMouseButtonDown (0)) {//left button
			Vector3 pos = Input.mousePosition;
			MoveTo(pos.x, pos.y);
		}
	}
	void checkTouch(){
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				Vector2 pos = touch.position;
				MoveTo(pos.x, pos.y);
			}
		}
	}
	void MoveTo(float x, float y){
		Vector3 moveDir = GetMoveDir(x, y);
		transform.Translate(speed * moveDir * Time.deltaTime );
		CmdMoveTo (transform.position, moveDir);
	}
	Vector3 GetMoveDir(float px, float py){
		float x = -1;
		float y = -1;
		float jump = 0;
		if (px > Screen.width * 0.25){
			x = 0; 
		}
		if (px > Screen.width * 0.75){
			x = 1; 
		}
		if (py > Screen.height * 0.25){
			y = 0; 
		}
		if (py > Screen.height * 0.75){
			y = 1; 
		}
		if (x == 0 && y == 0){
			jump = 1;
		}
		
		Vector3 moveDir = new Vector3(x, jump * 10, y);
		return moveDir;
	}
	
}