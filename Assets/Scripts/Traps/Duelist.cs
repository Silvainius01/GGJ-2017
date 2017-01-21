using UnityEngine;
using System.Collections;

public class Duelist : Trap {

	public float winChance = 0.8f;
	bool firstKill = true;

	// Update is called once per frame
	void Update () {

	}

	public override void ApplyTriggerEffect (){
		
	}

	bool Duel(BasicEnemyUnit obj){
		return true;
	}
}
