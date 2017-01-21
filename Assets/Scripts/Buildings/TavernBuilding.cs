using UnityEngine;
using System.Collections;

public class TavernBuilding : SpecialBuilding {

	public float slowPercent = 0.8f;
	public float drunkTime = 10.0f;
	public float enterBuildingPercentBoost = 0.2f;

	protected override void ApplySpecialEffect (BasicEnemyUnit unit)
	{
		//unit.Slow (slowPercent);
		unit.GetDrunk (slowPercent, drunkTime, enterBuildingPercentBoost);
	}
}
