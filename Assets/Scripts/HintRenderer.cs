using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRenderer : MonoBehaviour
{
    public LineRenderer lr;
    public Vector2 cellSize;
    private List<Vector3> _points;
    void Start()
    {

    }

    public void Render()
    {
        lr.positionCount = _points.Count;
        lr.SetPositions(_points.ToArray());
    }

    public void SetPoints(List<MazeGeneratorCell> path)
    {
        Debug.Log($"Points(HintRenderer) - {path.Count}");
        _points = new List<Vector3>();
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 point = new Vector3(path[i].x + cellSize.x/2, path[i].y + cellSize.y/2, 0);
            _points.Add(point);
        }
    }
}
