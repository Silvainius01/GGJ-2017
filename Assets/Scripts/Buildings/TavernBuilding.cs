using UnityEngine;
using System.Collections;

public class TavernBuilding : SpecialBuilding {

	public float slowPercent = 0.8f;
	public float drunkTime = 10.0f;
	public float enterBuildingPercentBoost = 0.2f;

	void Awake(){
		buildingType = SpecialBuilding.SpecialBuildingType.TAVERN;
	}

	protected override bool ApplySpecialEffect (BasicEnemyUnit unit)
	{
		//unit.Slow (slowPercent);
		unit.GetDrunk (slowPercent, drunkTime, enterBuildingPercentBoost);
		return false;
	}
}
