using UnityEngine;
using System.Collections;

public class Rat : Unit
{
    [Header("Rat Options")]
    bool moveToNewSquare = false;
    public float roamRange = 2.5f;
    public float roamDistToStop = 0.1f;
    public Timer squareMoveTimer = new Timer(2.0f, true);
    

	// Use this for initialization
	void Start () {
        navigateGraph = false;
        hasCompletedPath = true;
        destroyOnPathCompletion = false;
        roamRange = GraphMaker.Instance.squareSideSize / 2;
	}

    // Update is called once per frame
    public override void Update()
    {
        if (squareMoveTimer.isActive)
        {
            squareMoveTimer.Update(Time.deltaTime);
            if (squareMoveTimer.hasFired)
                moveToNewSquare = true;
        }

        if (moveToNewSquare)
        {
            navigateGraph = true;
            moveToNewSquare = false;
            path = GraphMaker.Instance.GetRandomPathFrom(transform.position);
        }
        else if (hasCompletedPath)
        {
            if (navigateGraph)
            {
                navigateGraph = false;
                squareMoveTimer.Activate();
            }

            hasCompletedPath = false;
            movePos = (Random.insideUnitCircle * roamRange) + GraphMaker.Instance.GetGraphPoint(transform.position).pos;
        }

        base.Update();
    }
}
