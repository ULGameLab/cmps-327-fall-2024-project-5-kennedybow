using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinder
{
    public enum MapState { GENERATED, DESTROYED }

    public class GenerateMap : MonoBehaviour
    {
        public int width = 20;
        public int height = 20;
        public bool perlinGenerator = false;
        public float perlinConstraint = 2.0f;
        public float primeConstraint = 0.5f;

        public GameObject passableTilePrefab;  
        public GameObject nonPassableTilePrefab; 
        public Material startTileMaterial;     
        public Material goalTileMaterial;      
        public MapState state = MapState.DESTROYED;

        Tile[,] TileList;
        Tile[,] tileList;
        public Tile start;
        public Tile goal;

        GameObject playerGameObject;

        void Start()
        {
            playerGameObject = GameObject.FindWithTag("Player");
        }

        void Update()
        {
            switch (state)
            {
                case MapState.DESTROYED:
                    transform.localScale = new Vector3(1, 1, 1);
                    GenerateNewMap();
                    transform.localScale = new Vector3(20f / width, 1, 20f / height);
                    state = MapState.GENERATED;
                    break;
                case MapState.GENERATED:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        foreach (Transform child in transform)
                        {
                            GameObject.Destroy(child.gameObject);
                        }
                        state = MapState.DESTROYED;
                    }
                    break;
                default:
                    break;
            }
        }

        public void GenerateNewMap()
        {
            TileList = generateTilesList();
            tileList = new Tile[width, height];

            foreach (Tile mapTile in mapTileList)
            {
                Tile t = CreateTile(Tile);
                tileList[Tile.X, Tile.Y] = t;
            }

            foreach (Tile tile in tileList)
            {
                FillAdjacentsForaTile(tile);
            }

            CreateWalls();
            ResetEnemies();
        }

        Tile[,] generateTilesList()
        {
            Tile[,] tiles;
            if (perlinGenerator)
            {
                PerlinGenerator perlinGen = new PerlinGenerator();
                tiles = perlinGen.MapGen(width, height, perlinConstraint);
            }
            else
            {
                PrimGenerator primGen = new PrimGenerator();
                tiles = primGen.MapGen(width, height, primeConstraint);
            }
            return tiles;
        }

        private Tile CreateTile(Tile Tile)
        {
            GameObject tileGO;
            if (Tile.Walkable) 
            {
                tileGO = Instantiate(passableTilePrefab, transform);
            }
            else 
            {
                tileGO = Instantiate(nonPassableTilePrefab, transform);
            }

            tileGO.name += tileGO.transform.GetSiblingIndex().ToString();
            tileGO.transform.position = new Vector3(-width / 2, 0, -height / 2) + new Vector3(mapTile.X, 0, mapTile.Y);
            Tile tile = tileGO.GetComponent<Tile>();
            tile.FillFromTile(Tile);

            if (tile.isStart)
            {
                tileGO.GetComponent<MeshRenderer>().material = startTileMaterial;
                start = tile;
                if (playerGameObject != null)
                {
                    playerGameObject.transform.position = tileGO.transform.position;
                    playerGameObject.GetComponent<Player>().Reset(tileGO.GetComponent<Tile>());
                }
            }
            else if (tile.isGoal)
            {
                tileGO.GetComponent<MeshRenderer>().material = goalTileMaterial;
                goal = tile;
            }

            return tile;
        }

        void FillAdjacentsForaTile(Tile tile)
        {
            if (tile.indexX - 1 >= 0) tile.AddAdjacentTile(tileList[tile.indexX - 1, tile.indexY]); 
            if (tile.indexX + 1 < width) tile.AddAdjacentTile(tileList[tile.indexX + 1, tile.indexY]); 
            if (tile.indexY - 1 >= 0) tile.AddAdjacentTile(tileList[tile.indexX, tile.indexY - 1]); 
            if (tile.indexY + 1 < height) tile.AddAdjacentTile(tileList[tile.indexX, tile.indexY + 1]);

            if (tile.indexX - 1 >= 0 && tile.indexY - 1 >= 0 && tileList[tile.indexX - 1, tile.indexY].isPassable && tileList[tile.indexX, tile.indexY - 1].isPassable)
            {
                tile.AddAdjacentTile(tileList[tile.indexX - 1, tile.indexY - 1]);
            }
            if (tile.indexX - 1 >= 0 && tile.indexY + 1 < height && tileList[tile.indexX - 1, tile.indexY].isPassable && tileList[tile.indexX, tile.indexY + 1].isPassable)
            {
                tile.AddAdjacentTile(tileList[tile.indexX - 1, tile.indexY + 1]);
            }
            if (tile.indexX + 1 < width && tile.indexY - 1 >= 0 && tileList[tile.indexX, tile.indexY - 1].isPassable && tileList[tile.indexX + 1, tile.indexY].isPassable)
            {
                tile.AddAdjacentTile(tileList[tile.indexX + 1, tile.indexY - 1]);
            }
            if (tile.indexX + 1 < width && tile.indexY + 1 < height && tileList[tile.indexX, tile.indexY + 1].isPassable && tileList[tile.indexX + 1, tile.indexY].isPassable)
            {
                tile.AddAdjacentTile(tileList[tile.indexX + 1, tile.indexY + 1]);
            }
        }

        void CreateWalls()
        {
            for (int i = 0; i < width + 2; i++)
            {
                GameObject upperWallGO = Instantiate(nonPassableTilePrefab, transform);
                upperWallGO.transform.position = new Vector3(-width / 2, 0, -height / 2) + new Vector3(i - 1, 0, -1);
                upperWallGO.GetComponent<Tile>().isPassable = false;

                GameObject lowerWallGO = Instantiate(nonPassableTilePrefab, transform);
                lowerWallGO.transform.position = new Vector3(-width / 2, 0, -height / 2) + new Vector3(i - 1, 0, height);
                lowerWallGO.GetComponent<Tile>().isPassable = false;
            }

            for (int j = 0; j < height + 2; j++)
            {
                GameObject leftWallGO = Instantiate(nonPassableTilePrefab, transform);
                leftWallGO.transform.position = new Vector3(-width / 2, 0, -height / 2) + new Vector3(-1, 0, j - 1);
                leftWallGO.GetComponent<Tile>().isPassable = false;

                GameObject rightWallGO = Instantiate(nonPassableTilePrefab, transform);
                rightWallGO.transform.position = new Vector3(-width / 2, 0, -height / 2) + new Vector3(width, 0, j - 1);
                rightWallGO.GetComponent<Tile>().isPassable = false;
            }
        }

        private void ResetEnemies()
        {
            Enemy[] enemyList = (Enemy[])GameObject.FindObjectsByType(typeof(Enemy), FindObjectsSortMode.None);
            foreach (Enemy enemy in enemyList)
            {
                enemy.Reset();
            }
        }
    }
}