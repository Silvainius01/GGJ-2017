using UnityEngine;
using System.Collections.Generic;

public class GraphMaker : MonoBehaviour
{
	public enum GRID_TYPE{
		NONE, 
		EMPTY_BUILDING,
		SPECIAL_BUILDING,
		TRAP,
		WALL
	}

    [System.Serializable]
    public class GraphPoint
    {
        [System.Serializable]
        public class ConnectionData
        {
            public int index;
            public float dist;

            public ConnectionData(int index, float distance)
            {
                this.index = index;
                dist = distance;
            }
        }
        [System.Serializable]
        public class NavData
        {
            public int pIndex;
            public bool wasTarget;
            public bool evaluated;
            public float tDist;

            public NavData(int index, float distance)
            {
                pIndex = index;
                tDist = distance;
                wasTarget = false;
                evaluated = false;
            }
        }

		public GameObject occupyingObj = null;
		public GRID_TYPE gridType = GRID_TYPE.NONE;
        public bool isBlocked = false;
        public Vector2 pos = Vector2.zero;
        public NavData navData = null;
        public List<ConnectionData> connections = new List<ConnectionData>();

        public bool IsConnectedTo(int index)
        {
            foreach (var link in connections)
                if (link.index == index)
                    return true;
            return false;
        }
    }

    public bool scanGraph = false;
    [Header("Graph Gen Options")]
    public float squareSideSize = 5.0f;
    public bool generateInitial = false;
    public float blockCreationChance = 0.2f;
    public bool generateBlocks = false;
    [Header("Graph Information")]
    public int rowLength = 0, colLength = 0;
    public List<GraphPoint> graphPoints = new List<GraphPoint>();
    [Header("Graph Debug NavData")]
    public bool navigate = false;
    public int startIndex = 0;
    public int finalIndex = 0;
    public List<int> navPath = new List<int>();


    Vector2 gameBoardScale;

    private void Awake()
    {
        // Collect scale of the plane we are operating on.
        gameBoardScale = new Vector2(transform.localScale.x, transform.localScale.z) * 5;
    }

    // Graph Internal Utility
    #region

    void AddPoint(Vector2 pos)
    {
        GraphPoint newPoint = new GraphPoint();
        newPoint.pos = pos;
        graphPoints.Add(newPoint);
    }

    void ConnectPoints(int indexA, int indexB)
    {
        float dist = Vector2.Distance(graphPoints[indexA].pos, graphPoints[indexB].pos);

        foreach (var link in graphPoints[indexA].connections)
            if (link.index == indexB)
                return;

        graphPoints[indexA].connections.Add(new GraphPoint.ConnectionData(indexB, dist));
        graphPoints[indexB].connections.Add(new GraphPoint.ConnectionData(indexA, dist));
    }

    void DisconnectPoints(int indexA, int indexB)
    {
        foreach (var link in graphPoints[indexA].connections)
            if (link.index == indexB)
            {
                graphPoints[indexA].connections.Remove(link);
                break;
            }
        foreach (var link in graphPoints[indexB].connections)
            if (link.index == indexA)
            {
                graphPoints[indexB].connections.Remove(link);
                break;
            }
    }

    void ClearPointNavData()
    {
        foreach (var point in graphPoints)
            point.navData = new GraphPoint.NavData(0, float.MaxValue);
    }

    public Vector2 PointPos(int index)
    {
        return graphPoints[index].pos;
    }

    #endregion

    // Graph Generation
    #region
        // index = col + (rowLength * row)
    void GeneratePoints()
    {
        bool zfirst = false;
        Vector2 startPos = (Vector2)transform.position + (-gameBoardScale + new Vector2(squareSideSize/2, squareSideSize/2)) ; // The lower right corner of the board.

        if (graphPoints == null)
            graphPoints = new List<GraphPoint>();
        graphPoints.Clear();

        rowLength = 0;
        colLength = 0;
        for (float x = startPos.x; x <= gameBoardScale.x; x += squareSideSize)
        {
            for (float y = startPos.y; y <= gameBoardScale.y; y += squareSideSize)
            {
                AddPoint(new Vector2(x, y));
                if (!zfirst) // This counts how long a row is.
                    rowLength++;
            }
            colLength++;
            zfirst = true;
        }
    }

    void FinalizeConections()
    {
        // index = col + (rowLength * row)
        int index = 0;
        for (int r = 0; r < colLength; r++)
        {
            index = r * rowLength;
            for (int c = 0; c < rowLength; c++, index++)
            {
                if (index < graphPoints.Count - 1 && Vector2.Distance(PointPos(index), PointPos(index+1)) <= squareSideSize)
                    ConnectPoints(index, index + 1);
                if (r < colLength - 1)
                    ConnectPoints(index, index + rowLength);
            }
            if (index < graphPoints.Count - 1)
                DisconnectPoints(index, index - 1);
        }
    }

    void GenerateRandomBlocks()
    {
        foreach (var point in graphPoints)
            if (point.connections.Count > 2)
            {
                bool valid = true;
                
                foreach(var link in  point.connections)
                    if (graphPoints[link.index].connections.Count <= 2)
                        valid = false;

                if (valid && graphPoints.IndexOf(point) != 0 && graphPoints.IndexOf(point) != 99)
                {
                    float chance = Random.value;
                    if (Random.value <= blockCreationChance)
                    {
                        point.isBlocked = true;
                        Debug.Log("Connection count: " + point.connections.Count);
                        while (point.connections.Count > 0)
                            DisconnectPoints(graphPoints.IndexOf(point), point.connections[0].index);
                    }
                }
            }
    }
    #endregion

    // Graph Navigation.
    #region

    void NavigateBetweenAstar(int indexA, int indexB)
    {
        int nextIndex = indexB;
        var q = new List<int>();
        int cIndex = 0;
        bool foundTarget = false;
        float dist = float.MaxValue;

        startIndex = indexA;
        finalIndex = indexB;

        if (startIndex == finalIndex)
            return;

        ClearPointNavData();
        graphPoints[indexA].navData = new GraphPoint.NavData(indexA, 0.0f);
        graphPoints[indexB].navData.wasTarget = true;

        foreach (var link in graphPoints[indexA].connections)
        {
            float comp = Mathc.SqrDist(PointPos(link.index), PointPos(indexB));
            if (comp < dist)
            {
                dist = comp;
                cIndex = graphPoints[indexA].connections.IndexOf(link);
            }
            else
                graphPoints[link.index].navData.evaluated = true;
        }

        graphPoints[indexA].navData.evaluated = true;
        dist = graphPoints[indexA].connections[cIndex].dist;
        cIndex = graphPoints[indexA].connections[cIndex].index;
        graphPoints[cIndex].navData = new GraphPoint.NavData(indexA, dist);
        q.Add(cIndex);

        while (q.Count > 0)
        {
            int curIndex = q[0];

            if (curIndex == indexB)
                foundTarget = true;
            else if (graphPoints[curIndex].navData.evaluated)
            {
                q.Remove(curIndex);
                continue;
            }

           cIndex = 0;
            dist = float.MaxValue;
            foreach (var link in graphPoints[curIndex].connections)
            {
                if (graphPoints[link.index].navData.evaluated)
                    continue;

                float comp = Mathc.SqrDist(PointPos(link.index), PointPos(indexB));
                if (comp < dist)
                {
                    dist = comp;
                    cIndex = graphPoints[curIndex].connections.IndexOf(link);
                }
                else
                    graphPoints[link.index].navData.evaluated = true;
            }

            dist = graphPoints[curIndex].connections[cIndex].dist + graphPoints[curIndex].navData.tDist;
            cIndex = graphPoints[curIndex].connections[cIndex].index;

           if (dist < graphPoints[cIndex].navData.tDist)
            graphPoints[cIndex].navData = new GraphPoint.NavData(curIndex, dist);

            if (!foundTarget)
                q.Add(cIndex);

            graphPoints[curIndex].navData.evaluated = true;
            q.Remove(curIndex);
        }

        // Record path.
        q.Clear();
        q.Add(nextIndex);
        for (int a = 0; a < graphPoints.Count && nextIndex != indexA; a++)
        {
            nextIndex = graphPoints[nextIndex].navData.pIndex;
            q.Add(nextIndex);
        }
        

        navPath.Clear();
        for (int a = 0; a < q.Count; a++)
            navPath.Add(q[q.Count - (a + 1)]);
    }

    void NavigateBetweenDijk(int indexA, int indexB)
    {
        int nextIndex = indexB;
        var q = new List<int>();
        bool foundTarget = false;
        float dist = float.MaxValue;

        startIndex = indexA;
        finalIndex = indexB;

        if (startIndex == finalIndex)
            return;

        ClearPointNavData();
        graphPoints[indexA].navData = new GraphPoint.NavData(indexA, 0.0f);
        graphPoints[indexA].navData.evaluated = true;
        graphPoints[indexB].navData.wasTarget = true;

        foreach (var link in graphPoints[indexA].connections)
        {
            graphPoints[link.index].navData = new GraphPoint.NavData(indexA, link.dist);
            q.Add(link.index);
        }

        while (q.Count > 0)
        {
            int curIndex = q[0];

            if (curIndex == indexB)
                foundTarget = true;
            else if (graphPoints[curIndex].navData.evaluated)
            {
                q.Remove(curIndex);
                continue;
            }
            
            dist = float.MaxValue;
            foreach (var link in graphPoints[curIndex].connections)
            {
                dist = link.dist + graphPoints[curIndex].navData.tDist;
<<<<<<< HEAD
                if (dist < graphPoints[link.index].navData.tDist)
=======
                 if (dist < graphPoints[link.index].navData.tDist)
>>>>>>> origin/matt
                    graphPoints[link.index].navData = new GraphPoint.NavData(curIndex, dist);
                

                if (!foundTarget && !graphPoints[link.index].navData.evaluated)
                    q.Add(link.index);
            }

            graphPoints[curIndex].navData.evaluated = true;
            q.Remove(curIndex);
        }

        // Record path.
        q.Clear();
        q.Add(nextIndex);
        for (int a = 0; a < graphPoints.Count && nextIndex != indexA; a++)
        {
            nextIndex = graphPoints[nextIndex].navData.pIndex;
            q.Add(nextIndex);
        }


        navPath.Clear();
        for (int a = 0; a < q.Count; a++)
            navPath.Add(q[q.Count - (a + 1)]);
    }

    public List<int> GetPath(Vector2 startPos, Vector2 endPos)
    {
        List<int> retval = new List<int>();

        NavigateBetweenDijk(GetClosestPointTo(startPos), GetClosestPointTo(endPos));

        for (int a = 0; a < navPath.Count; a++)
            retval.Add(navPath[a]);

        return retval;

    } 

    /// <summary> Generate a random path with the closest point to 'pos' being the start.  </summary>
    public List<int> GetRandomPathFrom(Vector2 pos)
    {
        int indexA = GetClosestPointTo(pos);
        int indexB = Random.Range(0, graphPoints.Count - 1);
        List<int> retval = new List<int>();

        while(graphPoints[indexB].isBlocked)
            indexB = Random.Range(0, graphPoints.Count - 1); ;

        NavigateBetweenAstar(indexA, indexB);

        for (int a = 0; a < navPath.Count; a++)
            retval.Add(navPath[a]);

        return retval;
    }

    #endregion

    // Misc Public Graph Data
    #region

    public int GetClosestPointTo(Vector2 pos)
    {
        int cIndex = 0;
        float dist = float.MaxValue;
        foreach (var point in graphPoints)
        {
            if (point.isBlocked)
                continue;
            float comp = Mathc.SqrDist(point.pos, pos);
            if (comp < dist)
            {
                dist = comp;
                cIndex = graphPoints.IndexOf(point);
            }
        }

        return cIndex;
    }

    /// <summary>  Returns the closest point to "pos" that is connected to "index"   </summary>
    int GetClosestConnectedPoint(int index, Vector2 pos)
    {
        int cIndex = 0;
        float dist = 0;
        foreach(var link in graphPoints[index].connections)
        {
            if (graphPoints[link.index].isBlocked)
                continue;
            float comp = Mathc.SqrDist(PointPos(link.index), pos);
            if(comp < dist)
            {
                dist = comp;
                cIndex = link.index;
            }
        }

        return cIndex;
    }

    public bool ScanGraphForBlocks()
    {
        bool foundBlock = false;
        foreach (var point in graphPoints)
            if (point.isBlocked)
            {
                while (point.connections.Count > 0)
                    DisconnectPoints(graphPoints.IndexOf(point), point.connections[0].index);
                foundBlock = true;
            }
        ClearPointNavData();
        return foundBlock;
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (generateInitial)
        {
            Awake();
            GeneratePoints();
            FinalizeConections();
            ClearPointNavData();
            generateInitial = false;
        }

        if(generateBlocks)
        {
            scanGraph = true;
            generateBlocks = false;
            GenerateRandomBlocks();
        }

        if (scanGraph)
        {
            FinalizeConections();
            ScanGraphForBlocks();
            navPath.Clear();
            scanGraph = false;
        }

        if(navigate)
        {
            NavigateBetweenDijk(startIndex, finalIndex);

            if (!graphPoints[navPath[navPath.Count - 1]].IsConnectedTo(finalIndex))
                //NavigateBetween(finalIndex, startIndex);

            navigate = false;
        }

        if (graphPoints != null)
        {
            foreach (var point in graphPoints)
            {
                if (point.isBlocked)
                    Gizmos.color = Color.black;
                else if (point.navData.evaluated)
                    Gizmos.color = Color.cyan;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawSphere(point.pos, 1.0f);
            }

            foreach (var point in graphPoints)
                if (point.connections != null)
                    foreach (var link in point.connections)
                    {
                        if (link.index < graphPoints.IndexOf(point))
                        {
                            if (graphPoints[link.index].navData.evaluated)
                                Gizmos.color = Color.cyan;
                            else
                                Gizmos.color = Color.magenta;
                            Gizmos.DrawLine(graphPoints[link.index].pos, point.pos);
                        }
                    }

            if (navPath.Count > 0)
            {
                Gizmos.color = Color.green;
                for (int a = 0; a < navPath.Count - 1; a++)
                    Gizmos.DrawLine(PointPos(navPath[a]), PointPos(navPath[a + 1]));
                foreach (var index in navPath)
                {
                    Gizmos.color = Color.green;
                    if (index == startIndex)
                        Gizmos.color = Color.blue;
                    else if (index == finalIndex)
                        Gizmos.color = Color.yellow;

                    Gizmos.DrawSphere(PointPos(index), 1.0f);
                }
            }
        }
    }
		
	public void SetGridType(int x, int y, GameObject obj, GRID_TYPE type){
		GraphPoint graphPoint = GetGraphPoint (x, y);
		graphPoint.occupyingObj = obj;
		graphPoint.gridType = type;
	}

    public void SetGridType(int index, GameObject obj, GRID_TYPE type)
    {
        GraphPoint graphPoint = graphPoints[index];
        graphPoint.occupyingObj = obj;
        graphPoint.gridType = type;
    }

    public GRID_TYPE GetGridType(int x, int y){
		return GetGraphPoint (x, y).gridType;
	}

	// col + rowlength * row
	public GraphPoint GetGraphPoint(int x, int y){
		return graphPoints [y + rowLength * x];
	}

	public bool IsPosInGridPos(Vector2 pos, int gridX, int gridY){
		return pos.x >= gridX * squareSideSize && pos.x <= gridX * (squareSideSize + 1)
			&& pos.y >= gridY * squareSideSize && pos.y <= gridY * (squareSideSize + 1);
	}
}
