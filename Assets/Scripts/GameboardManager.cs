using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameboardManager : MonoBehaviour 
{
    private const float TILE_SIZE = .8f;
    private const float TILE_OFFSET = TILE_SIZE / 2;

    private const int NUMBER_OF_TILES_X = 25;
    private const int NUMBER_OF_TILES_Y = 25;

    private int selectionX = -1;
    private int selectionY = -1;

    void Update() 
    {
        DrawGameBoard();
    }

    private void DrawGameBoard() 
    {
        Vector3 widthLine = Vector3.right * NUMBER_OF_TILES_X;
        Vector3 heightLine = Vector3.forward * NUMBER_OF_TILES_Y;

        for (int i = 0; i <= NUMBER_OF_TILES_Y; i++) 
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= NUMBER_OF_TILES_X; j++) 
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
    }
}
