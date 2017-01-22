using UnityEngine;
using System.Collections;

public class BakeryBuilding : SpecialBuilding {

	public float slowPercent = 0.5f;

	void Awake(){
		buildingType = SpecialBuilding.SpecialBuildingType.BAKERY;
	}

	protected override bool ApplySpecialEffect (BasicEnemyUnit unit)
	{
		unit.Slow (slowPercent);
		return false;
	}
}
