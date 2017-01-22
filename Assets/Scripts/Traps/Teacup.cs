using UnityEngine;
using System.Collections;

public class Teacup : Trap {

	public enum Direction{
		LEFT, RIGHT, UP, DOWN
	}
	public int gridSpillDistance = 3;
	private int currDistTravelled = 0;
	private Direction dir;
	public Timer nextTileTimer = new Timer(0.5f);
	public float nextTileTime = 0.5f;
	private int currSpillX, currSpillY;

	public void Init(KeyCode key, int gridX, int gridY, Direction dir){
		base.Init (key, gridX, gridY);
		this.dir = dir;
		currSpillX = gridX;
		currSpillY = gridY;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		if (activated) {
			if (nextTileTimer.timeLeft < Time.deltaTime) {
				float overflow = Time.deltaTime - nextTileTimer.timeLeft;
				AttackWithTea ();

				if (GetNextValidSpace ()) {
					++currDistTravelled;
					nextTileTimer.Activate (nextTileTime - overflow);
				}
				else
					Destroy (gameObject);

			} else {
				nextTileTimer.Update (Time.deltaTime);
			}
		}
	}

	public override void ApplyTriggerEffect (){
		AttackWithTea ();
		if (GetNextValidSpace ()) {
			++currDistTravelled;
			nextTileTimer.Activate (nextTileTime);
		}
		else
			Destroy (gameObject);
	}

	private void AttackWithTea(){
		// begin spilling the tea into the streets!!!
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
		foreach (GameObject enemy in enemies) {
			// check to see if enemy is in affected grid pos
			if(graph.IsPosInGridPos(enemy.transform.position, currSpillX, currSpillY)){
				// kill the enemy
				Destroy(enemy);
			}
		}
	}

	private bool GetNextValidSpace(){
		if (gridSpillDistance > 0 && currDistTravelled + 1 == gridSpillDistance)
			return false;

		switch (dir) {
		case Direction.DOWN:
			++currSpillY;
			break;
		case Direction.UP:
			--currSpillY;
			break;
		case Direction.LEFT:
			--currSpillX;
			break;
		case Direction.RIGHT:
			++currSpillX;
			break;
		}
		return graph.GetGridType (currSpillX, currSpillY) == GraphMaker.GRID_TYPE.NONE;
	}
}
