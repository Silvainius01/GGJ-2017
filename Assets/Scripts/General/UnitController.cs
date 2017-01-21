using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

    public GraphMaker graph;

    void Start()
    {
        //graph = GraphMaker.

    }

  void update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //graph.N
            graph.GetClosestPointTo(Input.mousePosition);

        }
        
    }

}
