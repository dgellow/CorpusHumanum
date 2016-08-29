using UnityEngine;
using System.Collections;

public class TierUnitRenderer : MonoBehaviour {

	public SpriteRenderer tierLogo;
	public Sprite triangleLogo;
	public Sprite squareLogo;
	public Sprite circleLogo;
	public Sprite octogonLogo;

	// Use this for initialization
	void Start () {
		var enemyRenderer = GetComponent<EnemyRenderer> ();
		if (enemyRenderer != null) {
			tierLogo.sprite = TierToLogo (enemyRenderer.enemy.tier);
		}

		var allyRenderer = GetComponent<AllyRenderer> ();
		if (allyRenderer != null) {
			tierLogo.sprite = TierToLogo (allyRenderer.ally.strongAgainst[0]);
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
