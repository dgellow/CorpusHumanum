using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Organ : MonoBehaviour {

	public string name;
	public Vector2 selectedScale;
	public Color selectedColor;
	public bool isTouched = false;

	private PolygonCollider2D collider2D;
	private SpriteRenderer spriteRenderer;
	private GameUI gameUI;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		collider2D = GetComponent<PolygonCollider2D> ();
		gameUI = FindObjectOfType<GameUI> ();
	}

	void Update () {
		// Handle touching organ
		if (Input.touchCount == 1) {
			var touch = Input.GetTouch (0);
			if (!isTouched && touch.phase == TouchPhase.Began) {
				var point = Camera.main.ScreenToWorldPoint (touch.position);
				if (collider2D.OverlapPoint (point)) {
					Debug.Log ("Touched organ: " + name);
					isTouched = true;
				}
			} else if (isTouched && touch.phase == TouchPhase.Ended) {
				isTouched = false;
				GameState.gameState.selectedOrgan = this;
				gameUI.ShowDetailView ();
			}
		}

		if (isTouched) {
			spriteRenderer.color = selectedColor;
			spriteRenderer.transform.localScale = selectedScale;
		} else {
			spriteRenderer.color = Color.white;
			spriteRenderer.transform.localScale = new Vector2 (1f, 1f);
		}
	}
}
