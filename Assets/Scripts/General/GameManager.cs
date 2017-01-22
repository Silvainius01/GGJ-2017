using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	enum GameState{
		BUILD,
		PLAY
	}

	private GameState state = GameState.BUILD;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
