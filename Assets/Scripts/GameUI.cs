using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {
	public string mainMenuScene;
	public GameObject globalView;
	public DetailView detailView;

	private Vector2 detailViewOriginalPosition;

	void Start() {
		detailViewOriginalPosition = detailView.transform.localPosition;
	}

	void OnGUI() {
		detailView.Render ();
	}

	public void SaveAndBackToMenu() {
		Debug.Log ("Save state");
		GameState.gameState.SaveState ();
		Debug.Log ("Back to main menu");
		SceneManager.LoadScene (mainMenuScene);
	}

	public void ShowDetailView() {
		detailView.transform.position = new Vector2 (0, 0);
	}

	public void HideDetailView() {
		detailView.transform.localPosition = detailViewOriginalPosition;
		GameState.gameState.selectedOrgan = null;
	}
}
