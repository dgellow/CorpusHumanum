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
	public List<Ennemy> strongAgainst;
	public int damages;
}