using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata {

    public int generationSteps = 4;
    public int deathLimit = 5;
    public int birthLimit = 4;

    const int defaultGenerationSteps = 4;
    const int defaultdeathLimit = 5;
    const int defaultbirthLimit = 4;
    int mapLengthX = 0;
    int mapLengthY = 0;

    public void SetDefaultValues()
    {
        generationSteps = defaultGenerationSteps;
        deathLimit = defaultdeathLimit;
        birthLimit = defaultbirthLimit;
    }

    public void GenerateMap(ref int[,] map)
    {
        mapLengthX = map.GetUpperBound(0);
        mapLengthY = map.GetUpperBound(1);

        int[,] generatedMap = map;
        for (int i = 0; i < generationSteps; i++)
        {
            generatedMap = CellularAutomataIteration(generatedMap);
        }

        for (int x = 0; x < mapLengthX; x++)
        {
            for (int y = 0; y < mapLengthY; y++)
            {
                map[x, y] = generatedMap[x, y];
            }
        }
    }

    int[,] CellularAutomataIteration(int[,] map)
    {
        int[,] newMap = new int[mapLengthX, mapLengthY];
        for (int x = 0; x < mapLengthX; x++)
        {
            for (int y = 0; y < mapLengthY; y++)
            {
                int neighboursAmount = NeighboursAmount(map, x, y);

                if(map[x,y] != 0)
                    newMap[x, y] = (neighboursAmount < deathLimit) ? 0 : newMap[x, y];
                else
                    newMap[x, y] = (neighboursAmount > birthLimit) ? 1 : 0;
            }
        }
        return newMap;
    }

    int NeighboursAmount(int[,] map, int x, int y)
    {
        int amount = 0;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int neighbourX = x + i;
                int neighboutY = y + j;

                if (i == 0 && j == 0)
                    continue;
                else if (neighbourX < 0 || neighboutY < 0 || neighbourX >= mapLengthX || neighboutY >= mapLengthY)
                    amount++;
                else if (map[neighbourX, neighboutY] > 0)
                    amount++;
            }
        }
        return amount;
    }
}
