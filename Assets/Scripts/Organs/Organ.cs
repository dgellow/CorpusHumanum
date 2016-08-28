using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Organ : MonoBehaviour {

	public string name;
	public Vector3 selectedScale;
	public Color selectedColor;
	public bool isSelecting = false;
	public Image image;
	public int healthPoints = 100;
	public bool isBeingScanned = false;
	public int countDownScan;

	private int maxHealthPoints;
	private int minHealthPoints = 0;
	private Collider2D collider2D;
	private GameUI gameUI;

	public Dictionary<Enemy, int> enemies;
	public List<Ally> allies;

	void Start () {
		enemies = new Dictionary<Enemy,  int> ();
		allies = new List<Ally> ();
		maxHealthPoints = healthPoints;
		image = GetComponent<Image> ();
		collider2D = GetComponent<Collider2D> ();
		gameUI = FindObjectOfType<GameUI> ();
	}

	public int countEnemies() {
		var enemyCount = 0;
		if (enemies != null) {
			foreach (KeyValuePair<Enemy, int> entry in enemies) {
				enemyCount += entry.Value;
			}
		}
		return enemyCount;
	}

	void Update () {
		// Handle organ selection
		if (Input.touchCount == 1) {
			var touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began || 
				touch.phase == TouchPhase.Moved) {
				var point = Camera.main.ScreenToWorldPoint (touch.position);
				if (collider2D.OverlapPoint (point)) {
					foreach (var organ in GameController.gameState.organs) {
						organ.isSelecting = false;
					}
					isSelecting = true;
				}
			} else if (isSelecting && touch.phase == TouchPhase.Ended) {
				GameController.gameState.selectedOrgan = this;
				gameUI.ShowDetailView ();
			}
		}
			
		if (isSelecting) {
			image.color = selectedColor;
			image.transform.localScale = selectedScale;
		} else {
			image.color = Color.white;
			image.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
	}

	void Hurt(int amount) {
		healthPoints -= amount;
		if (healthPoints <= minHealthPoints) {
			healthPoints = minHealthPoints;
			Die ();
		}
	}

	void Heal(int amount) {
		healthPoints += amount;
		if (healthPoints > maxHealthPoints) {
			healthPoints = maxHealthPoints;
		}
	}

	void Die() {
		throw new System.NotImplementedException ();
	}

	public IEnumerator PlayScan() {
		isBeingScanned = true;
		for (var i = GameController.gameState.scanDelay; i >= 0; i--) {
			yield return new WaitForSeconds (1);
			countDownScan = i;
		}
		isBeingScanned = false;
	}
}
