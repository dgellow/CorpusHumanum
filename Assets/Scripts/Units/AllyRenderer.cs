using UnityEngine;
using System.Collections;

public class AllyRenderer : MonoBehaviour {

	public Ally ally;
	public float deathDuration = 2f;
	public Sprite deathSprite;

	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void Update() {
		if (GameController.gameState.selectedOrgan == null) {
			return;
		}

		spriteRenderer.enabled = GameController.gameState.selectedOrgan.id == ally.organAttachedTo.id;

		if (ally.status == UnitStatus.Dead) {
			StartCoroutine (PlayDeath ());	
		}
	}

	IEnumerator PlayDeath() {
		spriteRenderer.sprite = deathSprite;
		yield return new WaitForSeconds (deathDuration);
		Destroy (gameObject);
	}
}
