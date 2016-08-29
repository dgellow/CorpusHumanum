using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitStatus { Good, Weakened, Dead }

public enum UnitTier {
	Triangle,
	Circle,
	Square,
	Octogon,
	SomethingElse
}

public interface ICanAttack {
	void Hurt ();
}

public interface ICanBeAttacked {
	void ReactToBeingHurt ();
	void ReactToBeingHurt (int amount);
}

public interface ICanBehaveInCombat {
	void CombatBehaviour ();
}

public interface IHasLifespan {
	IEnumerator PlayLifespan ();
}
	
public class Enemy: ICanAttack, ICanBeAttacked {
	public UnitStatus status = UnitStatus.Good;
	public int damages = 1;
	public UnitTier tier;
	public Organ target;

	public Enemy (Organ target, UnitTier tier) {
		this.tier = tier;
		this.target = target;
	}
		
	#region ICanAttack implementation

	public void Hurt () {
		target.ReactToBeingHurt (damages);
	}

	#endregion

	#region ICanBeAttacked implementation

	public void ReactToBeingHurt () {
		if (status == UnitStatus.Weakened) {
			status = UnitStatus.Dead;
		} else if (status == UnitStatus.Good) {
			status = UnitStatus.Weakened;
		}
	}

	public void ReactToBeingHurt (int amount) {
		ReactToBeingHurt ();
	}

	#endregion
}

abstract public class Ally {
	public UnitStatus status = UnitStatus.Good;
	public List<UnitTier> strongAgainst;
	public Organ organAttachedTo;
}

class Macrophage: Ally, ICanAttack, ICanBeAttacked {

	#region ICanAttack implementation

	public void Hurt () {
		var target = GameController.gameState.organsEnemies [organAttachedTo.id] [0];
		if (target != null && target.status != UnitStatus.Dead) {
			target.ReactToBeingHurt ();	
		}
	}

	#endregion

	#region ICanBeAttacked implementation

	public void ReactToBeingHurt() {
		if (status == UnitStatus.Weakened) {
			status = UnitStatus.Dead;
		} else if (status == UnitStatus.Good) {
			status = UnitStatus.Weakened;
		}
	}

	public void ReactToBeingHurt (int amount) {
		ReactToBeingHurt ();
	}

	#endregion
}

public class Neutrophil: Ally, ICanBehaveInCombat {

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		var enemies = GameController.gameState.organsEnemies [organAttachedTo.id];
		var allies = GameController.gameState.organsAllies [organAttachedTo.id];
		foreach (var e in enemies) {
			e.ReactToBeingHurt ();
		}
		foreach (var a in allies) {
			var attackableAlly = a as ICanBeAttacked;
			if (attackableAlly != null) {
				attackableAlly.ReactToBeingHurt ();
			}
		}
		status = UnitStatus.Dead;
	}
	#endregion
}

public class Killer: Ally, ICanAttack {

	#region ICanAttack implementation
	public void Hurt () {
		var target = GameController.gameState.organsEnemies [organAttachedTo.id][0];
		while (target.status != UnitStatus.Dead) {
			target.ReactToBeingHurt ();
		}
	}
	#endregion
}

public class Helper: Ally, ICanBehaveInCombat {
	private int healDelay = 10;
	private bool canHeal = true;

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		if (canHeal) {
			organAttachedTo.Heal (10);
			GameController.gameState.StartExternalCoroutine (PlayCooldown ());
		}
	}
	#endregion

	IEnumerator PlayCooldown() {
		canHeal = false;
		for (var i = healDelay; i >= 0; i--) {
			yield return new WaitForSeconds (1);
		}
		canHeal = true;
	}
}

public class BCell: Ally, ICanBehaveInCombat {

	#region ICanBehaveInCombat implementation
	public void CombatBehaviour () {
		throw new System.NotImplementedException ();
	}
	#endregion
}