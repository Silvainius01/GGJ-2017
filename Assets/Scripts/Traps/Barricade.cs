using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector2 _myPosition = this.transform.position;
        int _index = GraphMaker.Instance.GetClosestPointTo(_myPosition);
        GraphMaker.GraphPoint _point = GraphMaker.Instance.graphPoints [_index];
        // just toggling is blocked doesnt allow for the AI to ind this piece and break it down... this should be changed
        _point.isBlocked = true;
        GraphMaker.Instance.Init();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
