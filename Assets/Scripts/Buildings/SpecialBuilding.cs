using UnityEngine;
using System.Collections.Generic;

public class SpecialBuilding : Building {

	private Dictionary<BasicEnemyUnit, Timer> occupantTimers = new Dictionary<BasicEnemyUnit, Timer>();
	public float occupancyTime = 2.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
		foreach (KeyValuePair<BasicEnemyUnit, Timer> occupant in occupantTimers) {
			if (occupant.Value.Update (Time.deltaTime)) {
				// if timer went off, apply special function and call exit
				occupant.Key.enabled = true;
				ApplySpecialEffect (occupant.Key);
				occupant.Key.BuildingExited ();
			}
		}
	}

	public void EnterBuilding(BasicEnemyUnit unit){
		// set units occupancy timer
		Timer timer = null;
		if (!occupantTimers.TryGetValue (unit, out timer)) {
			timer = new Timer (occupancyTime, true);
		}
		timer.Activate (occupancyTime);

		// disable the unit
		unit.enabled = false;
	}

	protected virtual void ApplySpecialEffect(BasicEnemyUnit unit){

	}
}
