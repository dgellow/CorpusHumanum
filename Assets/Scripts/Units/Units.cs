using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitStatus { Good, Weakened, Dead }

public struct Ennemy {
	public UnitStatus status;
	public int damages;
}

public struct Ally {
	public UnitStatus status;
	public List<EnemyType> strongAgainst;
	public int damages;
	public Ally(int damages, List<EnemyType> strengths) {
		status = UnitStatus.Good;
		strongAgainst = strengths;
		this.damages = damages;
	}
}