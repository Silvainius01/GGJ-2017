using UnityEngine;
using System.Collections;

public class AviatorSchoolBuilding : SpecialBuilding {

	private int defaultFlightRange = 2;
	public int testTargetX = 0, testTargetY = 0;

	void Awake(){
		GameObject gyro = Instantiate (Resources.Load ("Prefabs/Effects/Gyrocoptor"), transform.position, Quaternion.identity) as GameObject;
		gyro.GetComponent<Gyrocoptor> ().Init (testTargetX, testTargetY);
	}

	protected override void ApplySpecialEffect (BasicEnemyUnit unit)
	{
		// if fat from backery, enemy may leave
		if (false) {

		}
		// otherwise, bye bye
		else {
			// if drunk pick a target and fly as long as you need to to crash into him

			// if not drunk, find target within default distance and crash into him
		}
	}
}
