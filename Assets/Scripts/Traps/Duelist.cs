using UnityEngine;
using System.Collections;

public class Duelist : MonoBehaviour {

	public float winChance = 0.8f;
	private bool firstKill = true;
	public Timer dualTimer = new Timer(1.0f);
	private BasicEnemyUnit target;
	private GraphMaker graph;
	private int x, y;

	public void Init(float winChance, GraphMaker graph, int x, int y){
		this.winChance = winChance;
		this.graph = graph;
		this.x = x;
		this.y = y;
	}

	// Update is called once per frame
	public virtual void Update () {
		if (dualTimer.isActive) {
			if (dualTimer.Update (Time.deltaTime)) {
				Duel (target);
			}
		} else {
			target = FindTargetToDuel ();
			if (target != null) {
				// begin duel timer with enemy
				target.DuelStarted();
				dualTimer.Activate();
			}
		}
	}

	private BasicEnemyUnit FindTargetToDuel(){
		// begin spilling the tea into the streets!!!
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
		foreach (GameObject enemy in enemies) {
			// check to see if enemy is in affected grid pos
			if(graph.IsPosInGridPos(enemy.transform.position, x, y)){
				return enemy.GetComponent<BasicEnemyUnit> ();
			}
		}
		return null;
	}

	void Duel(BasicEnemyUnit obj){
		if (UnityEngine.Random.Range (0.0f, 1.0f) >= winChance || firstKill) {
			firstKill = false;
			Destroy (obj);
		} else {
			target.DuelEnded ();
			Destroy (gameObject);
		}
		target = null;
	}
}
