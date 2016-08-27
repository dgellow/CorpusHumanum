using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Organ : MonoBehaviour {

	public string name;
	public Vector2 selectedScale;
	public Color selectedColor;
	public bool isSelecting = false;
	public SpriteRenderer spriteRenderer;
	public int nbDetectedTumor;
	public int nbDetectedForeignBody;
	public int nbDetectedVirus;

	private PolygonCollider2D collider2D;
	private GameUI gameUI;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		collider2D = GetComponent<PolygonCollider2D> ();
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
			spriteRenderer.color = selectedColor;
			spriteRenderer.transform.localScale = selectedScale;
		} else {
			spriteRenderer.color = Color.white;
			spriteRenderer.transform.localScale = new Vector2 (1f, 1f);
		}
	}
}
