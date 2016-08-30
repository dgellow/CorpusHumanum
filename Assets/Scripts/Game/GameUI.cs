using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	public ParticleSystem damageParticleSystem;
	public ParticleSystem healParticleSystem;

	public RectTransform allyList;
	public RectTransform enemyList;
	public GameObject enemyPrefab;
	public GameObject macrophagePrefab;
	public GameObject neutrophilPrefab;
	public GameObject helperPrefab;
	public GameObject killerPrefab;

	public string mainMenuScene;
	public GlobalView globalView;
	public DetailView detailView;
	public Text incomeReserveText;

	private Vector2 detailViewOriginalPosition;

	void Start() {
		detailViewOriginalPosition = detailView.transform.localPosition;
		GameController.gameState.StartGameLogic ();
	}

	void OnGUI() {
		if (GameController.gameState.selectedOrgan != null) {
			detailView.Render ();
		} else {
			globalView.Render ();
		}
	
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

	public void DrawEnemy (Enemy enemy) {
		var enemyRenderer = Instantiate<EnemyRenderer> (enemyPrefab.GetComponent<EnemyRenderer> ());
		enemyRenderer.enemy = enemy;
		enemyRenderer.transform.SetParent (enemyList.transform, true);
		var rect = enemyRenderer.GetComponent<RectTransform> ().rect;
		var halfHeight = enemyList.rect.height / 2;
		enemyRenderer.transform.localPosition = new Vector3 (
			Random.Range ((rect.width / 2), enemyList.rect.width - (rect.width / 2)), 
			Random.Range (-halfHeight + (rect.height / 2), halfHeight - (rect.height / 2)), 
			0
		);
	}

	public void DrawAlly <T> (Ally ally) where T: Ally {
		GameObject prefab = null;
		if (typeof(T) == typeof(Macrophage)) {
			prefab = macrophagePrefab;
		} else if (typeof(T) == typeof(Neutrophil)) {
			prefab = neutrophilPrefab;
		} else if (typeof(T) == typeof(Helper)) {
			prefab = helperPrefab;
		} else if (typeof(T) == typeof(Killer)) {
			prefab = killerPrefab;
		}
		var allyRenderer = Instantiate<AllyRenderer> (prefab.GetComponent<AllyRenderer> ());
		allyRenderer.ally = ally;
		allyRenderer.transform.SetParent (allyList.transform, true);
		var rect = allyRenderer.GetComponent<RectTransform> ().rect;
		var halfHeight = allyList.rect.height / 2;
		allyRenderer.transform.localPosition = new Vector3 (
			Random.Range (-allyList.rect.width + (rect.width / 2), -(rect.width / 2)), 
			Random.Range (-halfHeight + (rect.height / 2), halfHeight - (rect.height / 2)), 
			0
		);
	}
}
