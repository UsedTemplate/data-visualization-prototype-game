using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator
{
    public static Vector3[,] GenerateGridPositions(int rows, int columns, float spacingX, float spacingZ, float yHeight = 0f)
    {
        Vector3[,] positions = new Vector3[rows, columns];

        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                positions[z, x] = new Vector3(x * spacingX, yHeight, z * spacingZ);
            }
        }

        return positions;
    }

}
