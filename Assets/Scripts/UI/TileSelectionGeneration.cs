using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSelectionGeneration : MonoBehaviour {

    public GameObject Canvas;
    public GameObject TileUiPrefab;
    public GraphMaker My_GraphMaker;

    private List<Transform> UiOptions;
    private const float uiDistanceFromScene = -3;

	// Use this for initialization
	void Start () {
	    for(int i = 0; i < My_GraphMaker.graphPoints.Count; i++)
        {
            SpawnUiElement(My_GraphMaker.graphPoints[i]);
        }
	}

    void SpawnUiElement(GraphMaker.GraphPoint _graphPoint)
    {
        Vector3 SpawnPoint = new Vector3(_graphPoint.pos.x, _graphPoint.pos.y, uiDistanceFromScene);
        GameObject _UiElement = (GameObject) Instantiate(TileUiPrefab, SpawnPoint, Quaternion.identity);
        _UiElement.transform.SetParent(Canvas.transform);

        //TODO: make new Ui element know about its graph point
    }
}
