using System.Collections.Generic;
using UnityEngine;
using System;
using MapGen;

namespace MazeGame
{
    public class PathFinder
    {
        private List<Node> TODOList = new List<Node>();
        private List<Node> DoneList = new List<Node>();
        private Tile goalTile;

        public PathFinder()
        {
            goalTile = null;
        }

        public Queue<Tile> FindPathAStar(Tile start, Tile goal)
        {
            TODOList.Clear();
            DoneList.Clear();

            TODOList.Add(new Node(start, 0, null, 0));
            goalTile = goal;

            while (TODOList.Count > 0)
            {
                TODOList.Sort((x, y) => x.priority.CompareTo(y.priority));
                Node current = TODOList[0];
                DoneList.Add(current);
                TODOList.RemoveAt(0);

                if (current.tile == goal)
                {
                    return RetracePath(current);
                }

                foreach (Tile nextTile in current.tile.Adjacents)
                {
                    if (DoneList.Exists(node => node.tile == nextTile))
                        continue;

                    double newCostToNextTile = current.costSoFar + 10;
                    Node nextNode = TODOList.Find(node => node.tile == nextTile);

                    if (nextNode == null || newCostToNextTile < nextNode.costSoFar)
                    {
                        double heuristicCost = HeuristicsDistance(nextTile, goalTile);
                        double totalCost = newCostToNextTile + heuristicCost;

                        if (nextNode == null)
                        {
                            nextNode = new Node(nextTile, totalCost, current, newCostToNextTile);
                            TODOList.Add(nextNode);
                        }
                        else
                        {
                            nextNode.cameFrom = current;
                            nextNode.costSoFar = newCostToNextTile;
                            nextNode.priority = totalCost;
                        }
                    }
                }
            }

            return new Queue<Tile>();
        }

        private double HeuristicsDistance(Tile currentTile, Tile goalTile)
        {
            int xdist = Math.Abs(goalTile.indexX - currentTile.indexX);
            int ydist = Math.Abs(goalTile.indexY - currentTile.indexY);
            return (xdist + ydist) * 10;
        }

        private Queue<Tile> RetracePath(Node node)
        {
            List<Tile> tileList = new List<Tile>();
            while (node.cameFrom != null)
            {
                tileList.Insert(0, node.tile);
                node = node.cameFrom;
            }
            return new Queue<Tile>(tileList);
        }

        public Queue<Tile> RandomPath(Tile start, int stepNumber)
        {
            List<Tile> tileList = new List<Tile>();
            Tile currentTile = start;

            for (int i = 0; i < stepNumber; i++)
            {
                Tile nextTile = GetNextTile(currentTile, tileList);
                if (nextTile == null) break;
                tileList.Add(currentTile);
                currentTile = nextTile;
            }
            return new Queue<Tile>(tileList);
        }

        private Tile GetNextTile(Tile currentTile, List<Tile> tileList)
        {
            if (currentTile.Adjacents.Count == 0) return null;

            List<Tile> adjacentList = new List<Tile>(currentTile.Adjacents);
            ShuffleTiles(adjacentList);

            foreach (Tile tile in adjacentList)
            {
                if (tileList.Count == 0 || tile != tileList[tileList.Count - 1])
                {
                    return tile;
                }
            }
            return null;
        }

        private void ShuffleTiles<T>(List<T> list)
        {
            for (int t = 0; t < list.Count; t++)
            {
                T tmp = list[t];
                int r = UnityEngine.Random.Range(t, list.Count);
                list[t] = list[r];
                list[r] = tmp;
            }
        }
    }
}
