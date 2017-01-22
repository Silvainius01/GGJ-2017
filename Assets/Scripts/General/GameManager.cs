using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	enum GameState{
		BUILD,
		PLAY
	}

	private GameState state = GameState.BUILD;
	private GraphMaker graph;

	[Header("Graph Generation")]
	public int numBuildingGenerations = 3;

	private GameObject gameMenuObj;
	private Spawner spawner;

	// Use this for initialization
	void Start () {
		// get the game menu
		gameMenuObj = GameObject.FindGameObjectWithTag("BuildMenu");
		gameMenuObj.SetActive (true);

		// get the enemy spawner
		spawner = GetComponent<Spawner>();

		// create a random game board
		graph = GameObject.FindGameObjectWithTag ("GameBoard").GetComponent<GraphMaker> ();
		graph.Init ();
		for (int i = 0; i < numBuildingGenerations; ++i) {
			graph.GenerateRandomBlocks();
		}
		graph.ScanGraphForBlocks ();

		for (int y = 0; y < graph.rowLength; ++y) {
			for (int x = 0; x < graph.colLength; ++x) {
				if (graph.GetGraphPoint(x,y).isBlocked) {
					GameObject emptyBuilding = (Instantiate (Resources.Load("Prefabs/Buildings/EmptyBuilding"), 
						graph.GetGraphPoint (x, y).pos, Quaternion.identity)) as GameObject;
					graph.SetGridType (x, y, emptyBuilding, GraphMaker.GRID_TYPE.EMPTY_BUILDING);
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		switch(state){
		case GameState.BUILD:
			break;
		case GameState.PLAY:
			break;
		}
	}

	List<GameObject> GetLivingEnemies(){
		return null;
	}
}
