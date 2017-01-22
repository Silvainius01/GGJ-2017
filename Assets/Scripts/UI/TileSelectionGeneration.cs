using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSelectionGeneration : MonoBehaviour {

    public GameObject Canvas;
    public GameObject TileUiPrefab;

    private List<Transform> UiOptions;
    private const float uiDistanceFromScene = -3;

	// Use this for initialization
	void Start () {
	    for(int i = 0; i < GraphMaker.Instance.graphPoints.Count; i++)
        {
            SpawnUiElement(GraphMaker.Instance.graphPoints[i]);
        }
	}

    void SpawnUiElement(GraphMaker.GraphPoint _graphPoint)
    {
        Vector3 SpawnPoint = new Vector3(_graphPoint.pos.x, _graphPoint.pos.y, uiDistanceFromScene);
        GameObject _UiElement = (GameObject) Instantiate(TileUiPrefab, SpawnPoint, Quaternion.identity);
        _UiElement.transform.SetParent(Canvas.transform);
		_UiElement.GetComponent<ButtonPassActionForward>().Point = _graphPoint;

        //TODO: make new Ui element know about its graph point
    }
}
