using System;
using System.Buffers.Text;
using System.Collections.Generic;
using UnityEngine;

public class FieldSpawner : MonoBehaviour
{
    [SerializeField] Transform firstSpawnPoint;
    [SerializeField] FieldCell fieldCellPrefab;
    [SerializeField] GameCube gameCubePrefab;
    [SerializeField] Transform playerCubes;
    [SerializeField] List<PocketCell> pocketCells;

    private FieldCell[,] fieldCells;
    private int[,] playField;
    private int[,] answerField;
    private int[,] basicCubes;
    private int maxPlayerCubes;
    private int spawnedCubes;
    private const int fieldSize = 3;
    private bool fieldIninialized;

    public int RemainPlayerCubes { get => maxPlayerCubes - spawnedCubes; }

    public int RemainCubesInField { get => CountCubesInField() - maxPlayerCubes; }

    public bool SetCubeToField(int x, int y, GameCube cube)
    {
        if (CheckCellIsEmpty(x, y))
        {
            playField[x, y] = 1;
            cube.SetPosition(GetNewCubePosition(x, y));
            cube.ChangeMoveState(false);
            return true;
        }
        return false;
    }

    public bool CheckAnswer()
    {
        for (int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                if (answerField[x, y] != playField[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void Update()
    {
        if (fieldIninialized && RemainPlayerCubes > 0 && CheckPocketCellsEmpty())
        {
            SpawnNewCubeInPocket();
        }
    }

    private int CountCubesInField()
    {
        int result = 0;
        foreach (var e in playField)
        {
            if (e == 1)
            {
                result++;
            }
        }
        return result;
    }

    private bool CheckPocketCellsEmpty()
    {
        bool result = false;
        foreach (var pocketCell in pocketCells)
        {
            result |= pocketCell.IsEmpty;
        }
        return result;
    }

    private PocketCell FindFirstEmptyPocketCell()
    {
        foreach (var pocketCell in pocketCells)
        {
            if (pocketCell.IsEmpty)
            {
                return pocketCell;
            }
        }
        return null;
    }

    private void Awake()
    {
        fieldIninialized = false;
        fieldCells = new FieldCell[fieldSize, fieldSize];
        CreateLevel();
        fieldIninialized = true;
    }

    private void CreateLevel()
    {
        InitNewLevel();
        DrawCells();
        SpawnBasicCubes();
    }

    private void InitNewLevel() // Only for this task
    {
        maxPlayerCubes = 3; 
        playField = new int[fieldSize, fieldSize] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        answerField = new int[fieldSize, fieldSize] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
        basicCubes = new int[fieldSize, fieldSize] { { 0, 1, 0 }, { 0, 0, 0 }, { 0, 1, 0 } };
        maxPlayerCubes = CountMaxCubes();
    }

    private int CountMaxCubes()
    {
        int count = 0;
        foreach (var e in answerField)
        {
            if (e == 1)
            {
                count++;
            }
        }
        return count;
    }

    private void SpawnBasicCubes()
    {
        for (int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                if (basicCubes[x, y] == 1)
                {
                    GameCube newCube = DrawNewCube(GetNewCubePosition(x, y), moveState: false);
                    SetCubeToField(x, y, newCube);
                    spawnedCubes++;
                }
            }
        }
    }

    private void DrawCells()
    {
        Vector2 cellPosition;
        int cellNumber = 1;
        for (int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                cellPosition = new Vector2(x, -y);
                fieldCells[x, y] = Instantiate(fieldCellPrefab).GetComponent<FieldCell>();
                fieldCells[x, y].SetInitData(firstSpawnPoint, cellPosition, $"FieldCell_{cellNumber}", x, y);
                cellNumber++;
            }
        }
    }

    private GameCube DrawNewCube(Vector2 position, bool moveState)
    {
        GameCube newCube = Instantiate(gameCubePrefab);
        newCube.transform.SetParent(playerCubes);
        newCube.SetPosition(position);
        newCube.ChangeMoveState(moveState);
        return newCube;
    }

    private Vector2 GetNewCubePosition(int x, int y)
    {
        float baseX = firstSpawnPoint.position.x;
        float baseY = firstSpawnPoint.position.y;
        return new Vector2(baseX + x, baseY - y);
    }

    private bool CheckCellIsEmpty(int x, int y)
    {
        return playField[x, y] == 0;
    }

    private void SpawnNewCubeInPocket()
    {
        PocketCell pocketCell = FindFirstEmptyPocketCell();
        GameCube newCube = DrawNewCube(pocketCell.transform.position, moveState: true);
        pocketCell.SetCube(newCube);
        spawnedCubes++;
    }
}
