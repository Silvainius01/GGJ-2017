using UnityEngine;
using System.Collections;

public class Duelist : Trap {

	public float winChance = 0.8f;
	private bool firstKill = true;
	public Timer dualTimer = new Timer(1.0f);
	private BasicEnemyUnit target;

	public void Init(float winChance){
		this.winChance = winChance;
	}

	// Update is called once per frame
	public virtual void Update () {
		if (dualTimer.isActive ()) {
			if (dualTimer.Update (Time.deltaTime)) {
				Duel (target);
			}
		} else {
			target = FindTargetToDuel ();
			if (target != null) {
				// begin duel timer with enemy
				dualTimer.Activate();
			}
		}
	}

	private BasicEnemyUnit FindTargetToDuel(){
		// begin spilling the tea into the streets!!!
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
		foreach (GameObject enemy in enemies) {
			// check to see if enemy is in affected grid pos
			if(graph.IsPosInGridPos(enemy.transform.position, gridX, gridY)){
				return enemy.GetComponent<BasicEnemyUnit> ();
			}
		}
		return null;
	}

	bool Duel(BasicEnemyUnit obj){
		if (UnityEngine.Random.Range (0.0f, 1.0f) >= winChance || firstKill) {
			firstKill = false;
			Destroy (obj);
		} else {
			Destroy (gameObject);
		}
		target = null;
	}
}
