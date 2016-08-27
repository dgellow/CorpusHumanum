using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {
	public string mainMenuScene;

	public void SaveAndBackToMenu() {
		Debug.Log ("Save state");
		GameState.gameState.SaveState ();
		Debug.Log ("Back to main menu");
		SceneManager.LoadScene (mainMenuScene);
	}
}
