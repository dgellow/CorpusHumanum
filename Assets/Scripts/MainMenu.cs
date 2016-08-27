using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public string gameScene;
	public GameObject settingsPanel;

	private Vector3 settingsPanelOriginalPosition;

	public void Start() {
		GameState.gameState.LoadSettings ();
		settingsPanelOriginalPosition = settingsPanel.transform.localPosition;
	}

	public void NewGame() {
		Debug.Log ("New game");
		GameState.gameState.Initialize ();
		SceneManager.LoadScene (gameScene);
	}

	public void ContinueGame() {
		Debug.Log ("Continue game");
		GameState.gameState.LoadState ();
		SceneManager.LoadScene (gameScene);	
	}

	public void Settings() {
		settingsPanel.transform.localPosition = new Vector2 (0, 0);
		Debug.Log ("Select settings");
	}

	public void CancelSettings() {
		GameState.gameState.LoadSettings ();
		Debug.Log ("Cancel settings");
		settingsPanel.transform.localPosition = settingsPanelOriginalPosition;
	}

	public void ApplySettings() {
		GameState.gameState.SaveSettings ();
		Debug.Log ("Apply settings");
		settingsPanel.transform.localPosition = settingsPanelOriginalPosition;
	}
}
