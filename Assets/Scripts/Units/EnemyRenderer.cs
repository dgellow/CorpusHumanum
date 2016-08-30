using UnityEngine;
using System.Collections;

public class EnemyRenderer : MonoBehaviour {

	public Enemy enemy;
	public float deathDuration = 2f;
	public Sprite deathSprite;
	public ParticleSystem particleSystem;
	public int deathNbParticle = 4;

	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void Update() {
		var selectedOrgan = GameController.gameState.selectedOrgan;
		if (selectedOrgan == null) {
			return;
		}

		spriteRenderer.enabled = selectedOrgan.id == enemy.target.id;
		if (enemy.status == UnitStatus.Dead) {
			StartCoroutine (PlayDeath ());	
		}
	}

	IEnumerator PlayDeath() {
		particleSystem.Emit (deathNbParticle);
		yield return new WaitForSeconds (deathDuration);
		Destroy (gameObject);
	}
}
