using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class CreateBoard : MonoBehaviour
{
    public GameObject[] tilePrefab;
    public GameObject housePrefab;
    public Text score;

    public GameObject treePrefab;

    private GameObject[] tiles;


    // Bitboard of dirts (long 64 bits)
    long dirtBB = 0; 
    long treeBB = 0;
    long desertBB = 0;
    long playerBB = 0; 

    int nRows = 8;
    int nColums = 8;

    void Start()
    {

        tiles = new GameObject[nRows * nColums];

        for (int r=0; r< nRows; r++)
        {
            for (int c = 0;c < nColums; c++)
            {
                int randomTile = UnityEngine.Random.Range(0, tilePrefab.Length);

                Vector3 pos = new Vector3(c, 0, r);

                GameObject tile = Instantiate(tilePrefab[randomTile], pos, Quaternion.identity);
                tile.name = tile.tag + "_" + r + "_" + c;

                // Store each tile
                tiles[r * nRows + c] = tile;

                if (tile.tag == "Dirt")
                {
                    dirtBB = SetCellState(dirtBB, r, c);
                }

                if (tile.tag == "Desert")
                {
                    desertBB = SetCellState(desertBB, r, c);
                }
            }

            PrintBB("Dirt", dirtBB);

            Debug.Log("<color=cyan>" + "Dirt cells = " + CellCount(dirtBB) + "</color>");
        }

        InvokeRepeating("PlantTree", 1, 0.3f);
        
    }

    void PlantTree()
    {
        int randomRow = UnityEngine.Random.Range(0, nRows);
        int randomCol = UnityEngine.Random.Range(0, nColums);


        // Get cell state or dirtBB and if it's 1, we can plant a tree
        //if (GetCellState(dirtBB, randomRow, randomCol))

        // Modification, subtract the dirt tiles where the player has put a house
        // To get rid of the positions in dirtBB that the player has put a house, we use the and and negate the other bitboard
        // Example
        // dirtBB   ==  01001100
        // playerBB ==  00101010   -> ~playerBB  => 11010101 (Convert 1 to 0 and 0 to 1)
        //
        // dirtBB &   01001100
        // ~playerB   11010101
        // Result     01000100
        if (GetCellState(dirtBB & ~playerBB, randomRow, randomCol))
        {
            GameObject tree = Instantiate(treePrefab);
            tree.transform.parent = tiles[randomRow * nRows + randomCol].transform;
            tree.transform.localPosition = Vector3.zero;

            treeBB = SetCellState(treeBB, randomRow, randomCol);
        }

       
    }

    void PrintBB(string name, long BB)
    {
        Debug.Log("<color=cyan>" + name + ": " + Convert.ToString(BB, 2).PadLeft(64, '0') + "</color>");
    }

    long SetCellState(long bitboard, int row, int col)
    {
        long newBit = 1L << (row * nRows + col); // Set the bit in the correct position for the bitboard

        return (bitboard |= newBit);
    }

    bool GetCellState(long bitboard, int row, int col)
    {
        // Mask to get the position of the bitboard for that row or col
        long mask = 1L << (row * nRows + col);

        // Add the mask to the bitboard to retrieve 0 or 1
        return ((mask & bitboard) != 0); 
    }

    int CellCount(long bitboard)
    {
        int count = 0;
        long bb = bitboard;
        while (bb != 0)
        {
            bb &= bb - 1; // Remove 1 to get to 0 at some point
            count++;
        }

        return count;
    }

    public void CalculateScore()
    {
        int dirtTileCount = CellCount(playerBB & dirtBB);
        int deserTileCount = CellCount(playerBB & desertBB);
        
        int total = (CellCount(playerBB & desertBB) * 2) + (CellCount(playerBB & dirtBB) * 10);

        score.text = "Score:  " + total + " ( Desert: " + deserTileCount + " - Dirt: " + dirtTileCount + " )";
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // each tile is 1 size long, so the tile hit tile is in an int position
                int rowTile = (int)hit.collider.gameObject.transform.position.z;
                int colTile = (int)hit.collider.gameObject.transform.position.x;


                bool isDirtRow = GetCellState(dirtBB, rowTile, colTile);

                bool thereIsTree = GetCellState(treeBB, rowTile, colTile);

                Debug.Log("<color=cyan>" + "Tile " + rowTile + " " + colTile + " isDirt? " + isDirtRow + " Tree? " + thereIsTree + "</color>");
                

                // Limit where the player can put a house, only on dirt or where there is no trees
                // if it's a cell dirt and no tree or desertBB
                if (GetCellState((dirtBB & ~treeBB) | desertBB, rowTile, colTile))
                {

                    GameObject house = Instantiate(housePrefab);
                    house.transform.parent = hit.collider.gameObject.transform;
                    house.transform.localPosition = Vector3.zero;
                    playerBB = SetCellState(playerBB, rowTile, colTile);

                    CalculateScore();
                }


            }
        }
    }
}
