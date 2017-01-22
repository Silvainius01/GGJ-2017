using UnityEngine;
using System.Collections;

public class DuelistTrap : Trap {

	public float winChance = 0.8f;

	public override void ApplyTriggerEffect (){
		// spawn a dualist
		GameObject obj = Instantiate(Resources.Load("Prefabs/Traps/Duelist"), transform.position, Quaternion.identity) as GameObject;
		obj.GetComponent<Duelist> ().Init (winChance, graph, gridX, gridY);
		Destroy (gameObject);
	}
}
