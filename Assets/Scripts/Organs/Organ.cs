using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Organ : MonoBehaviour, ICanBeAttacked {

	public int id;
	public string name;
	public Vector3 selectedScale;
	public Color selectedColor;
	public bool isSelecting = false;
	public Image image;
	public Sprite backgroundImage;
	public int healthPoints = 100;
	public bool isBeingScanned = false;
	public bool isBeingCollected = false;
	public int countDownScan;
	public int countDownCollect;
	public RectTransform totalHealthMiniBar;
	public RectTransform healthMiniBar;
	public Image healthMiniBarImage;

	public int maxHealthPoints;
	private int minHealthPoints = 0;
	private Collider2D collider2D;
	private GameUI gameUI;

	void Start () {
		maxHealthPoints = healthPoints;
		healthMiniBarImage = healthMiniBar.GetComponent<Image> ();
		image = GetComponent<Image> ();
		collider2D = GetComponent<Collider2D> ();
		gameUI = FindObjectOfType<GameUI> ();
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

	#region ICanBeAttacked implementation

	public void ReactToBeingHurt() {
		throw new System.NotImplementedException ();
	}

	public void ReactToBeingHurt (int amount) {
		gameUI.damageParticleSystem.Emit (1);
		healthPoints -= amount;
		if (healthPoints <= minHealthPoints) {
			healthPoints = minHealthPoints;
			Die ();
		}
	}

	#endregion

	public void Heal(int amount) {
		healthPoints += amount;
		if (healthPoints > maxHealthPoints) {
			healthPoints = maxHealthPoints;
		}
		gameUI.healParticleSystem.Emit (1);
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

	public IEnumerator PlayCollect() {
		isBeingCollected = true;
		for (var i = GameController.gameState.collectDelay; i >= 0; i--) {
			yield return new WaitForSeconds (1);
			countDownCollect = i;
		}
		isBeingCollected = false;
	}
}
