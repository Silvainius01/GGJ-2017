using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssignmentUiManager : MonoBehaviour {

    public static AssignmentUiManager Instance;

    public List<GameObject> BuildingButtons = new List<GameObject>();
    public List<GameObject> TrapButtons = new List<GameObject>();
    public GraphMaker My_GraphMaker;
    public GameObject FocusGroup;
    public GameObject CanvasToToggleOff;
    public float UiRadius = 1f;

    GraphMaker.GRID_TYPE gridType = GraphMaker.GRID_TYPE.NONE;
    GraphMaker.GraphPoint curTile;
    List<GameObject> curSpawned = new List<GameObject>();
    const float uiDistanceFromZero = -5;
    bool Active = false;
    
    void Awake()
    {
        if(AssignmentUiManager.Instance == null)
        {
            Instance = this;
        } 
    }

    public void SetConstruct (GameObject _construction)
    {
		// if creating a building, remove empty building obj
		if (gridType == GraphMaker.GRID_TYPE.SPECIAL_BUILDING) {
			GraphMaker.GraphPoint oldGraphPoint = My_GraphMaker.GetGraphPoint (curTile.pos);
			Destroy (oldGraphPoint.occupyingObj);
		}
			
		// create obj and place it on the grid
		GameObject _spawn = Instantiate (_construction, curTile.pos, Quaternion.identity) as GameObject;
        My_GraphMaker.SetGridType(My_GraphMaker.GetClosestPointTo(curTile.pos), _spawn, gridType);

        foreach(GameObject _obj in curSpawned)
        {
            Destroy(_obj);
        }
        curSpawned.Clear();
        CanvasToToggleOff.SetActive(true);
        FocusGroup.SetActive(false);
    }

    public void ActivateUI (GraphMaker.GraphPoint _gridPoint)
    {
        curTile = _gridPoint;
        Active = true;
        FocusGroup.SetActive(true);
        CanvasToToggleOff.SetActive(false);

        if(curTile.gridType == GraphMaker.GRID_TYPE.EMPTY_BUILDING)
        {
			gridType = GraphMaker.GRID_TYPE.EMPTY_BUILDING;
            PopulateUiRing(BuildingButtons);
        } else if (curTile.gridType == GraphMaker.GRID_TYPE.NONE)
        {
            gridType = GraphMaker.GRID_TYPE.NONE;
            PopulateUiRing(TrapButtons);
        } else
        {
            Debug.Log("This space type cannot be populated");
        }
    }

    void PopulateUiRing(List<GameObject> _toSpawn)
    {
        float _theta = Mathc.TWO_PI / _toSpawn.Count ;
        for (int i = 0; i < _toSpawn.Count; i++)
        {
            float _degreeItteration = _theta * i + Mathc.HALF_PI;
            Vector2 _2dPoint = new Vector2(Mathf.Cos(_degreeItteration), Mathf.Sin(_degreeItteration)) * UiRadius;
            Vector3 _spawnPoint = new Vector3(_2dPoint.x, _2dPoint.y, uiDistanceFromZero);
            GameObject _uiElement = (GameObject)Instantiate(_toSpawn[i], _spawnPoint, Quaternion.identity);
            _uiElement.transform.SetParent(FocusGroup.transform);
            curSpawned.Add(_uiElement);
        }
    }
}
