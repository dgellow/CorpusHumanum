using UnityEngine;
using System.Collections;

public class TierUnitRenderer : MonoBehaviour {

	public SpriteRenderer tierLogo;
	public Sprite triangleLogo;
	public Sprite squareLogo;
	public Sprite circleLogo;
	public Sprite octogonLogo;

	private EnemyRenderer enemyRenderer;
	private AllyRenderer allyRenderer;

	// Use this for initialization
	void Start () {
		enemyRenderer = GetComponent<EnemyRenderer> ();
		if (enemyRenderer != null) {
			tierLogo.sprite = TierToLogo (enemyRenderer.enemy.tier);
		}

		allyRenderer = GetComponent<AllyRenderer> ();
		if (allyRenderer != null) {
			tierLogo.sprite = TierToLogo (allyRenderer.ally.strongAgainst[0]);
		}
	}

	void Update() {
		var selectedOrgan = GameController.gameState.selectedOrgan;
		if (enemyRenderer != null) {
			if (selectedOrgan != null && selectedOrgan.id == enemyRenderer.enemy.target.id) {
				tierLogo.enabled = selectedOrgan.isBeingCollected;
			} else {
				tierLogo.enabled = false;
			}
		}

		if (allyRenderer != null) {
			if (selectedOrgan != null && selectedOrgan.id == allyRenderer.ally.organAttachedTo.id) {
				tierLogo.enabled = true;
			} else {
				tierLogo.enabled = false;
			}
		}
	}

	Sprite TierToLogo (UnitTier tier) {
		switch (tier) {
		case UnitTier.Triangle:
			return triangleLogo;
		case UnitTier.Circle:
			return circleLogo;
		case UnitTier.Square:
			return squareLogo;
		case UnitTier.Octogon:
			return octogonLogo;
		default:
			return null;
		}
	}
}
