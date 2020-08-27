using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGeneratorCell
{
    public int x;
    public int y;

    public bool wallLeft = true;
    public bool wallBottom = true;

    public bool visited = false;

    public int distance;

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(this.x, this.y);
    }
}


public class MazeGenerator
{
    public Dificulty dificulty;

    private const int minDistance = 10;

    public Algorithms algorithm;
    public int width;
    public int height;

    public Vector2Int startCell;
    private Vector2Int _finishCell;
    private int _finishDistance;
    private List<MazeGeneratorCell> _correctPath;

    public Vector2Int FinishCell { get => _finishCell; }
    public int FinishDistance { get => _finishDistance; }
    public List<MazeGeneratorCell> CorrectPath { get => _correctPath;}

    public MazeGeneratorCell[,] Generate ()
    {


        //Initialization
        MazeGeneratorCell[,] cells = new MazeGeneratorCell[width,height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new MazeGeneratorCell() { x=x, y=y};
            }
        }

        //Maze generation
        switch (algorithm)
        {
            case Algorithms.RecursiveBacktracking:
                RecursiveBacktracker(cells);
                break;
            case Algorithms.PrimsAlgorithm:
                PrimsAlgorithm(cells);
                break;
            default:
                break;
        }


        _finishCell = SetFinish(cells);
        _correctPath = FindCorrectPath(cells);

        //Clean up
        RemoveUselessWalls(cells);


        return cells;
    }

    private void RecursiveBacktracker(MazeGeneratorCell[,] cells)
    {
        MazeGeneratorCell current = cells[startCell.x, startCell.y];
        current.visited = true;
        current.distance = 0;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>();

        do
        {
            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>();

            int x = current.x;
            int y = current.y;

            if (x > 0 && !cells[x - 1, y].visited)
                unvisitedNeighbours.Add(cells[x - 1, y]);
            if (y > 0 && !cells[x, y - 1].visited)
                unvisitedNeighbours.Add(cells[x, y - 1]);
            if (x < width - 2 && !cells[x + 1, y].visited)
                unvisitedNeighbours.Add(cells[x + 1, y]);
            if (y < height - 2 && !cells[x, y + 1].visited)
                unvisitedNeighbours.Add(cells[x, y + 1]);

            if( unvisitedNeighbours.Count > 0)
            {
                MazeGeneratorCell next = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                next.visited = true;
                next.distance = current.distance + 1;
                RemoveWalls(current, next);

                stack.Push(next);
                current = next;

            }
            else
            {
                current = stack.Pop();
            }
        } while (stack.Count > 0);

    }

    private void PrimsAlgorithm(MazeGeneratorCell[,] cells)
    {
        List<MazeGeneratorCell> unvisitedCells = new List<MazeGeneratorCell>();
        for (int x = 0; x < cells.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < cells.GetLength(1) - 1; y++)
            {
                unvisitedCells.Add(cells[x,y]);
            }
        }

        cells[startCell.x, startCell.y].visited = true;
        cells[startCell.x, startCell.y].distance = 0;
        unvisitedCells.Remove(cells[startCell.x, startCell.y]);

        do
        {
            MazeGeneratorCell current = unvisitedCells[UnityEngine.Random.Range(0, unvisitedCells.Count)];

            MazeGeneratorCell visitedNeigbour = getVisitedNeighbour(cells, current);

            if (visitedNeigbour != null)
            {
                current.visited = true;
                current.distance = visitedNeigbour.distance + 1;
                RemoveWalls(current, visitedNeigbour);
                unvisitedCells.Remove(current);
            }

        } while (unvisitedCells.Count > 0);



    }

    private List<MazeGeneratorCell> FindCorrectPath(MazeGeneratorCell[,] cells)
    {
        List<MazeGeneratorCell> path = new List<MazeGeneratorCell>();

        MazeGeneratorCell current = cells[_finishCell.x, _finishCell.y];
        path.Add(current);

        while (current.distance > 0)
        {
            if(current.x > 0 && IsConnected(current, cells[current.x - 1, current.y]) && cells[current.x - 1, current.y].distance < current.distance)
            {
                current = cells[current.x - 1, current.y];
                path.Add(current);
                continue;
            }
            if (current.y > 0 && IsConnected(current, cells[current.x , current.y - 1]) && cells[current.x, current.y - 1].distance < current.distance)
            {
                current = cells[current.x, current.y - 1];
                path.Add(current);
                continue;
            }
            if (current.x < cells.GetLength(0) - 2 && IsConnected(current, cells[current.x + 1, current.y]) && cells[current.x + 1, current.y].distance < current.distance)
            {
                current = cells[current.x + 1, current.y];
                path.Add(current);
                continue;
            }
            if (current.y < cells.GetLength(1) - 2 && IsConnected(current, cells[current.x, current.y + 1]) && cells[current.x, current.y + 1].distance < current.distance)
            {
                current = cells[current.x, current.y + 1];
                path.Add(current);
                continue;
            }
        }

        return path;
    }
    private MazeGeneratorCell getVisitedNeighbour(MazeGeneratorCell[,] cells, MazeGeneratorCell currentCell)
    {
        List<MazeGeneratorCell> list = new List<MazeGeneratorCell>();

        if (currentCell.x > 0 && cells[currentCell.x - 1, currentCell.y].visited)
            list.Add(cells[currentCell.x - 1, currentCell.y]);
        if (currentCell.x < cells.GetLength(0) - 2 && cells[currentCell.x + 1, currentCell.y].visited)
            list.Add(cells[currentCell.x + 1, currentCell.y]);
        if (currentCell.y > 0 && cells[currentCell.x, currentCell.y - 1].visited)
            list.Add(cells[currentCell.x, currentCell.y - 1]);
        if (currentCell.y < cells.GetLength(1) - 2 && cells[currentCell.x, currentCell.y + 1].visited)
            list.Add(cells[currentCell.x, currentCell.y + 1]);

        if (list.Count > 0)
            return list[UnityEngine.Random.Range(0, list.Count)];
        else
            return null;
    }

    private bool IsConnected(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        bool connected = true;

        if (a.x == b.x)
        {
            if (a.y > b.y)
            {
                if (a.wallBottom)
                    connected = false;
            }
            else
            {
                if (b.wallBottom)
                    connected = false;
            }
        }
        else
        {
            if (a.x > b.x)
            {
                if (a.wallLeft)
                    connected = false;
            }
            else
            { 
                if (b.wallLeft)
                    connected = false;
            }
        }

        return connected;
    }

    private void RemoveWalls(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if (a.x == b.x)
        {
            if(a.y > b.y)
                a.wallBottom = false;
            else
                b.wallBottom = false;
        }
        else
        {
            if (a.x > b.x)
                a.wallLeft = false;
            else
                b.wallLeft = false;
        }
    }

    private Vector2Int SetFinish(MazeGeneratorCell[,] cells)
    {
        List<MazeGeneratorCell> list = new List<MazeGeneratorCell>();

        for (int x = 0; x < width - 2; x++)
        {
            if(cells[x, 0].distance > minDistance)
                list.Add(cells[x, 0]);
            if (cells[x, height - 2].distance > minDistance)
                list.Add(cells[x, height - 2]);
        }

        for (int y = 0; y < height - 2; y++)
        {
            if (cells[0, y].distance > minDistance)
                list.Add(cells[0, y]);
            if (cells[width - 2, y].distance > minDistance)
                list.Add(cells[width - 2, y]);
        }

        list = list.OrderBy(x => x.distance).ToList();

        switch (dificulty)
        {
            case Dificulty.Min:
                {
                    MazeGeneratorCell cell = list.First();
                    this._finishDistance = cell.distance;
                    Debug.Log("Min finish distance - " + cell.distance);
                    return cell.ToVector2Int();
                }
                
            case Dificulty.Avg:
                {
                    MazeGeneratorCell cell = list[UnityEngine.Random.Range(0, list.Count)];
                    this._finishDistance = cell.distance;
                    Debug.Log("Random finish distance - " + cell.distance);
                    return cell.ToVector2Int();
                }
                
            case Dificulty.Max:
                {
                    MazeGeneratorCell cell = list.Last();
                    this._finishDistance = cell.distance;
                    Debug.Log("Max finish distance - " + cell.distance);
                    return cell.ToVector2Int();
                } 
            default:
                {
                    MazeGeneratorCell cell = cells[0, 0];
                    this._finishDistance = cell.distance;
                    Debug.LogWarning("Default finish distance - " + cell.distance);
                    return cell.ToVector2Int();
                }
        }
    }

    private void RemoveUselessWalls(MazeGeneratorCell[,] cells)
    {
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            cells[x, cells.GetLength(1) - 1].wallLeft = false;
        }
        for (int y = 0; y < cells.GetLength(1); y++)
        {
            cells[cells.GetLength(0) - 1, y].wallBottom = false;
        }

        if (FinishCell.x == 0)
            cells[FinishCell.x, FinishCell.y].wallLeft = false;
        else if (FinishCell.y == 0)
            cells[FinishCell.x, FinishCell.y].wallBottom = false;
        else if (FinishCell.x == cells.GetLength(0) - 2)
            cells[FinishCell.x + 1, FinishCell.y].wallLeft = false;
        else if (FinishCell.y == cells.GetLength(1) - 2)
            cells[FinishCell.x, FinishCell.y + 1].wallBottom = false;
    }
}
