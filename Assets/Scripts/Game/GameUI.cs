using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	
	public string mainMenuScene;
	public GlobalView globalView;
	public DetailView detailView;
	public Text incomeReserveText;

	private Vector2 detailViewOriginalPosition;

	void Start() {
		detailViewOriginalPosition = detailView.transform.localPosition;
		GameController.gameState.Initialize ();
	}

	void OnGUI() {
		detailView.Render ();
		globalView.Render ();
	
		RenderGUI (); 
	}

	void RenderGUI() {
		incomeReserveText.text = GameController.gameState.incomeReserve.ToString ();
	}

	public void SaveAndBackToMenu() {
		Debug.Log ("Save state");
		GameController.gameState.SaveState ();
		Debug.Log ("Back to main menu");
		SceneManager.LoadScene (mainMenuScene);
	}

	public void ShowDetailView() {
		var rectTransform = detailView.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = new Vector2 (0, 0);
	}

	public void HideDetailView() {
		detailView.transform.localPosition = detailViewOriginalPosition;
		GameController.gameState.selectedOrgan.isSelecting = false;
		GameController.gameState.selectedOrgan = null;
	}
}
