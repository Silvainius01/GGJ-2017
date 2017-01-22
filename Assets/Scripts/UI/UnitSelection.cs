using UnityEngine;
using System.Collections;

public class UnitSelection : MonoBehaviour
{
    bool first = false;
    Vector2 sPos;
    Vector2 fPos;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    { 
	    if(Input.GetMouseButtonDown(0))
        {
            fPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(first)
            {
                sPos = fPos;
                first = false;
            }

            
        }
        else if(!first)
        {
            var unitsOnMap = FindObjectsOfType<Unit>();

            foreach(var unit in unitsOnMap)
                if (Mathc.VectorIsBetween(unit.transform.position, sPos, fPos))
                    if (unit.ownedByPlayer)
                        unit.isSelected = true;
        }
	}
}
