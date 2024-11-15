using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int indexX;
    public int indexY;
    public bool isPassable = true; // Assuming whether the tile is walkable or not
    public bool isStart;
    public bool isGoal;
    public List<Tile> Adjacents;
    private bool isEnemy; // This should be set based on your game logic

    public void FillFromTile(Tile Tile)
    {
        // Code to initialize tile from MapTile
    }

    public void AddAdjacentTile(Tile tile)
    {
        if (Adjacents == null) Adjacents = new List<Tile>();
        Adjacents.Add(tile);
    }

    // Check if the tile is an enemy tile
    public bool IsEnemyTile()
    {
        return isEnemy; // Modify this logic as necessary (maybe based on game initialization)
    }
}