using UnityEngine;
using System.Collections;

public abstract class Trap : MonoBehaviour {

	protected KeyCode triggerKey;
	protected Vector2 gridPosition;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (triggerKey)) {
			ApplyTriggerEffect ();
		}
	}

	public void DisplayTriggerKey(){
		// TODO print text over the position showing the hotkey
	}

	public void Init(KeyCode key, Vector2 gridPosition){
		triggerKey = key;
		this.gridPosition = gridPosition;
	}

	public abstract void ApplyTriggerEffect ();
}
