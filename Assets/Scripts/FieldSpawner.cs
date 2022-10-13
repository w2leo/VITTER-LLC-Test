using System;
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

    public int MaxPlayerCubes { get => maxPlayerCubes; }

    public int SpawnedPlayerCubes { get => spawnedCubes; }

    public int RemainPlayerCubes { get => maxPlayerCubes - SpawnedPlayerCubes; }

    public int RemainCubesToField { get => CountCubesInField() - maxPlayerCubes; }


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

    private void Update()
    {
        if (fieldIninialized && RemainPlayerCubes > 0 && CheckPocketCellsEmpty())
        {
            PocketCell pocketCell = FindFirstEmptyPocketCell();
            GameCube newCube = DrawCube(pocketCell.transform.position);
            newCube.CanMove = true;
            pocketCell.SetCube(newCube);
            spawnedCubes++;
        }
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
    private bool SetCubeToField(int x, int y)
    {
        //Check cell is empty
        if (playField[x, y] != 0)
        {
            return false;
        }
        //Set cube to cell
        playField[x, y] = 1;
        DrawCube(x, y);
        fieldCells[x, y].SetCube();
        return true;
    }

    public bool SetCubeToField(int x, int y, GameCube cube)
    {
        //Check cell is empty
        if (playField[x, y] != 0)
        {
            return false;
        }
        //Set cube to cell
        playField[x, y] = 1;

        float baseX = firstSpawnPoint.position.x;
        float baseY = firstSpawnPoint.position.y;
        Vector2 cellPosition = new Vector2(baseX + x, baseY - y);
        cube.transform.position = cellPosition;
        cube.CanMove = false;
        fieldCells[x, y].SetCube();
        return true;
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

    private void Awake()
    {
        fieldIninialized = false;
        fieldCells = new FieldCell[fieldSize, fieldSize];
        CreateLevel();
        fieldIninialized = true;
    }

    private void CreateLevel()
    {
        BasicInitLevel();
        DrawCells();
        SpawnBasicCubes();
        spawnedCubes = CountSpawnedCubes();
    }

    private void BasicInitLevel() // Only for this task
    {
        maxPlayerCubes = 5; // 3!!
        playField = new int[fieldSize, fieldSize] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        answerField = new int[fieldSize, fieldSize] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } }; // {0,1,0}
        basicCubes = new int[fieldSize, fieldSize] { { 0, 1, 0 }, { 0, 0, 0 }, { 0, 1, 0 } };

        if (!CheckFieldInitialization(answerField))
        {
            throw new Exception("WRONG_INIT_EXCEPTION");
        }
    }

    private int CountSpawnedCubes()
    {
        int countCubes = 0;
        foreach (var e in playField)
        {
            if (e == 1)
            {
                countCubes++;
            }
        }
        return countCubes;
    }

    private bool CheckFieldInitialization(int[,] fieldToCheclk)
    {
        int count = 0;
        foreach (var e in fieldToCheclk)
        {
            if (e == 1)
            {
                count++;
            }
        }
        return count == MaxPlayerCubes;
    }

    private void SpawnBasicCubes()
    {
        for (int y = 0; y < fieldSize; y++)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                if (basicCubes[x, y] == 1)
                {
                    if (!SetCubeToField(x, y))
                    {
                        throw new Exception("WRONG_INIT_EXCEPTION");
                    }
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

    private void DrawCube(int x, int y)
    {
        float baseX = firstSpawnPoint.position.x;
        float baseY = firstSpawnPoint.position.y;
        float baseZ = -1.0f;
        Vector3 cellPosition = new Vector3(baseX + x, baseY - y, baseZ);
        GameCube newCube = Instantiate(gameCubePrefab);
        newCube.transform.SetParent(playerCubes);
        newCube.transform.position = cellPosition;
    }

    private GameCube DrawCube(Vector3 position)
    {
        position = new Vector3(position.x, position.y, -1.0f);
        GameCube newCube = Instantiate(gameCubePrefab);
        newCube.transform.SetParent(playerCubes);
        newCube.transform.position = position;
        return newCube;
    }
}
