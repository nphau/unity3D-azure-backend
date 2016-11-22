using UnityEngine;
using System.Runtime.InteropServices;

public class GameCenterUI : MonoBehaviour {
	
	[DllImport("__Internal")]
	static extern void GameCenterUI_Init();
	[DllImport("__Internal")]
	static extern void GameCenterUI_Show();

	// Use this for initialization
	void Start () {
		Social.localUser.Authenticate((success) => {});
	}
	
	public void Init() {
		GameCenterUI_Init();	
	}
	
	public void Show() {
		if(Social.localUser.authenticated) {
			GameCenterUI_Show();
		}
	}
}
