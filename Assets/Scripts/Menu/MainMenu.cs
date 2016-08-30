using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public string gameScene;
	public GameObject settingsPanel;

	private Vector3 settingsPanelOriginalPosition;

	public void Start() {
		GameController.gameState.LoadSettings ();
		settingsPanelOriginalPosition = settingsPanel.transform.localPosition;
	}

	public void NewGame() {
		GameController.gameState.Initialize ();
		SceneManager.LoadScene (gameScene);
	}

	public void ContinueGame() {
		GameController.gameState.LoadState ();
		SceneManager.LoadScene (gameScene);	
	}

	public void Settings() {
		settingsPanel.transform.localPosition = new Vector2 (0, 0);
		Debug.Log ("Select settings");
	}

	public void CancelSettings() {
		GameController.gameState.LoadSettings ();
		Debug.Log ("Cancel settings");
		settingsPanel.transform.localPosition = settingsPanelOriginalPosition;
	}

	public void ApplySettings() {
		GameController.gameState.SaveSettings ();
		Debug.Log ("Apply settings");
		settingsPanel.transform.localPosition = settingsPanelOriginalPosition;
	}

	void LoadLevel(Scenario scenario)  {
		GameController.gameState.selectedScenario = scenario;
		SceneManager.LoadScene (gameScene);
	}

	public void LoadLevel1() {
		LoadLevel (Scenario.Level1);
	}
		
	public void LoadLevel2() {
		LoadLevel (Scenario.Level2);
	}
		
	public void LoadLevel3() {
		LoadLevel (Scenario.Level3);
	}
}
