using UnityEngine;
using System.Collections.Generic;

public class SpecialBuilding : Building {

	public enum SpecialBuildingType{
		TAVERN,
		BAKERY,
		AVIATION
	}

	private Dictionary<BasicEnemyUnit, Timer> occupantTimers = new Dictionary<BasicEnemyUnit, Timer>();
	public float occupancyTime = 2.0f;
	public SpecialBuildingType buildingType;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
		List<BasicEnemyUnit> removal = new List<BasicEnemyUnit> ();
		foreach (KeyValuePair<BasicEnemyUnit, Timer> occupant in occupantTimers) {
			if (occupant.Value.Update (Time.deltaTime)) {
				// if timer went off, apply special function and call exit
				occupant.Key.gameObject.SetActive(true);
				if (!ApplySpecialEffect (occupant.Key)) {
					occupant.Key.BuildingExited ();
				}
				removal.Add (occupant.Key);
			}
		}

		foreach (BasicEnemyUnit unit in removal) {
			occupantTimers.Remove (unit);
		}
	}

	public void EnterBuilding(BasicEnemyUnit unit){
		// set units occupancy timer
		Timer timer = null;
		if (!occupantTimers.TryGetValue (unit, out timer)) {
			timer = new Timer (occupancyTime, true);
			occupantTimers.Add (unit, timer);
		}
		timer.Activate (occupancyTime);

		// disable the unit
		unit.gameObject.SetActive(false);
	}

	protected virtual bool ApplySpecialEffect(BasicEnemyUnit unit){
		return false;
	}
}
