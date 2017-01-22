using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	enum GameState{
		BUILD,
		PLAY
	}

	public static GameManager  Instance;

	private GameState state = GameState.BUILD;
	private GraphMaker graph;

	[Header("Graph Generation")]
	public int numBuildingGenerations = 3;

	private GameObject gameMenuObj;
	private Spawner spawner;
	public Spawner.SpawnWave[] spawnWaves;
	private int currWaveIndex = 0;
	private List<BasicEnemyUnit> livingEnemies = new List<BasicEnemyUnit>();

	void Awake()
	{
		if(GameManager.Instance == null)
		{
			Instance = this;
		} 
	}

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
				if (graph.GetGraphPoint (x, y).isBlocked) {
					GameObject emptyBuilding = (Instantiate (Resources.Load ("Prefabs/Buildings/EmptyBuilding"), 
						                           graph.GetGraphPoint (x, y).pos, Quaternion.identity)) as GameObject;
					graph.SetGridType (x, y, emptyBuilding, GraphMaker.GRID_TYPE.EMPTY_BUILDING);
				} else {
					graph.SetGridType (x, y, null, GraphMaker.GRID_TYPE.NONE);
				}
			}
		}

		InitState (GameState.BUILD);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("game state: " + state);

		switch(state){
		case GameState.BUILD:

			if (Input.GetKeyDown (KeyCode.Return)) {
				Debug.Log ("playing");
				InitState (GameState.PLAY);
			}

			break;
		case GameState.PLAY:
			// update the spawner
			BasicEnemyUnit enemyUnit = spawner.UpdateSpawns (Time.deltaTime);
			if (enemyUnit != null) {
				AddLivingEnemy (enemyUnit);
			}
			break;
		}
	}

	private void InitState(GameState gameState){

		switch (gameState) {
		case GameState.BUILD:
			// show the game menu
			gameMenuObj.SetActive(true);
			break;
		case GameState.PLAY:
			// hide the game menu;
			gameMenuObj.SetActive (false);

			// clear living objects
			livingEnemies.Clear ();

			// setup spawns
			Debug.Log("initing play");
			if (currWaveIndex < spawnWaves.Length) {
				Debug.Log ("calling init wave");
				spawner.InitWave(spawnWaves[currWaveIndex++]);
			}

			break;
		}

		state = gameState;
	}

	public List<BasicEnemyUnit> GetLivingEnemies(){
		return livingEnemies;
	}

	public void AddLivingEnemy(BasicEnemyUnit enemy){
		Debug.Log ("spawn num alive: " + livingEnemies.Count);
		if(!livingEnemies.Contains(enemy)){
			livingEnemies.Add(enemy);
		}
	}

	public void LivingEnemyWasDestroyed(BasicEnemyUnit enemy){
		livingEnemies.Remove (enemy);
		Debug.Log ("death num alive: " + livingEnemies.Count);
		if (livingEnemies.Count == 0 && spawner.spawnQ.Count == 0) {
			InitState (GameState.BUILD);
		}
	}
}
