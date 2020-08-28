using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;


public class Maze : MonoBehaviour
{
    [Header("Maze settings")]
    public bool randomSettings;

    public Algorithms algorithm;
    public Dificulty dificulty;
    [Range(10, 50)]
    public int width;
    [Range(10, 50)]
    public int height;

    public bool randomStartCell;
    public Vector2Int startCell;

    [Header("Scene settings")]    
    public GameObject cellPrefab;
    public HintRenderer hintRenderer;

    [Header("Debug info")]
    public Vector2Int finishCell;
    public int finishDistance;
    public MazeGeneratorCell[,] cells;
    public List<MazeGeneratorCell> correctPath;
    public bool showCellDistance;
    public bool drawHint;

    private void Start()
    {
        Create();
    }

    public void Create()
    {
        MazeGenerator generator = new MazeGenerator();

        if (randomSettings)
        {
            this.algorithm = (Algorithms)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Algorithms)).Length);
            this.dificulty = (Dificulty)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Dificulty)).Length);
            this.width = UnityEngine.Random.Range(10, 50);
            this.height = UnityEngine.Random.Range(10, 50);
        }
        if(randomStartCell)
            this.startCell = new Vector2Int(UnityEngine.Random.Range(0, this.width - 1), UnityEngine.Random.Range(0, this.height - 1));

        generator.algorithm = this.algorithm;
        generator.width = this.width;
        generator.height = this.height;
        generator.dificulty = this.dificulty;
        generator.startCell = this.startCell;

        this.cells = generator.Generate();
        this.finishCell = generator.FinishCell;
        this.finishDistance = generator.FinishDistance;
        this.correctPath = generator.CorrectPath;
        
    }


    public void Spawn()
    {
        for (int x = 0; x < this.cells.GetLength(0); x++)
        {
            for (int y = 0; y < this.cells.GetLength(1); y++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3Int(x, y, 0), Quaternion.identity);
                cell.transform.parent = this.transform;

                #region Debug
                if(showCellDistance)
                {
                    TextMeshPro textField = cell.AddComponent<TextMeshPro>();
                    textField.fontSize = 3f;
                    textField.alignment = TextAlignmentOptions.Center;
                    textField.color = new Color(255f, 255f, 255f);
                    textField.text = cells[x, y].distance.ToString();

                    RectTransform rect = cell.GetComponent<RectTransform>();
                    rect.pivot = new Vector2(0, 0);
                    rect.sizeDelta = new Vector2(1, 1);
                }
                #endregion

                CellWalls walls = cell.GetComponent<CellWalls>();

                walls.WallLeft.SetActive(cells[x, y].wallLeft);
                walls.WallBottom.SetActive(cells[x, y].wallBottom);

                if (finishCell.x == x && finishCell.y == y)
                {
                    Finish finishScript = cell.AddComponent<Finish>();

                    if (finishCell.x == 0)
                        finishScript.side = Finish.Sides.Left;
                    else if (finishCell.y == 0)
                        finishScript.side = Finish.Sides.Bottom;
                    else if (finishCell.x == cells.GetLength(0) - 2)
                        finishScript.side = Finish.Sides.Right;
                    else if (finishCell.y == cells.GetLength(1) - 2)
                        finishScript.side = Finish.Sides.Top;
                }

            }
        }

        if (drawHint)
        {
            hintRenderer.SetPoints(correctPath);
            hintRenderer.Render();
        }

    }

    public void Despawn()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

}
