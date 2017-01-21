using UnityEngine;
using System.Collections;

public class BakeryBuilding : SpecialBuilding {

	public float slowPercent = 0.5f;

	protected override void ApplySpecialEffect (BasicEnemyUnit unit)
	{
		unit.Slow (slowPercent);
	}
}
