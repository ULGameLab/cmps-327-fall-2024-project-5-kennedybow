using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MapGen;

namespace Pathfinding
{
    public class Node
    {
        public Node cameFrom = null; // Parent node
        public double priority = 0; // F value
        public double costSoFar = 0; // G Value
        public Tile tile;

        public Node(Tile _tile, double _priority, Node _cameFrom, double _costSoFar)
        {
            cameFrom = _cameFrom;
            priority = _priority;
            costSoFar = _costSoFar;
            tile = _tile;
        }
    }

    public class PathFinder
    {
        List<Node> TODOList = new List<Node>();
        List<Node> DoneList = new List<Node>();
        Tile goalTile;

        // This is the constructor
        public PathFinder()
        {
            goalTile = null;
        }

        // Find the path based on A-Star Algorithm
        public Queue<Tile> FindPathAStar(Tile start, Tile goal)
        {
            TODOList = new List<Node>();
            DoneList = new List<Node>();

            TODOList.Add(new Node(start, 0, null, 0));
            goalTile = goal;

            while (TODOList.Count > 0)
            {
                TODOList.Sort((x, y) => (x.priority.CompareTo(y.priority))); // Sort TODO List based on the F cost
                Node current = TODOList[0];
                DoneList.Add(current);
                TODOList.RemoveAt(0);

                if (current.tile == goal)
                {
                    return RetracePath(current);  // Returns the path if goal is reached
                }

                foreach (Tile nextTile in current.tile.Adjacents)
                {
                    if (DoneList.Exists(n => n.tile == nextTile)) continue; // Skip if already evaluated

                    double newCost = current.costSoFar + 10; // Assuming cost to move to adjacent tile is 10
                    Node nextNode = TODOList.Find(n => n.tile == nextTile);

                    if (nextNode == null)
                    {
                        nextNode = new Node(nextTile, newCost + HeuristicsDistance(nextTile, goal), current, newCost);
                        TODOList.Add(nextNode);
                    }
                    else if (newCost < nextNode.costSoFar)
                    {
                        nextNode.costSoFar = newCost;
                        nextNode.priority = newCost + HeuristicsDistance(nextTile, goal);
                        nextNode.cameFrom = current; // Update parent
                    }
                }
            }
            return new Queue<Tile>(); // Returns an empty path if no path is found
        }

        public Queue<Tile> FindPathAStarEvadeEnemy(Tile start, Tile goal)
        {
            TODOList = new List<Node>();
            DoneList = new List<Node>();

            TODOList.Add(new Node(start, 0, null, 0));
            goalTile = goal;

            while (TODOList.Count > 0)
            {
                TODOList.Sort((x, y) => (x.priority.CompareTo(y.priority)));
                Node current = TODOList[0];
                DoneList.Add(current);
                TODOList.RemoveAt(0);

                if (current.tile == goal)
                {
                    return RetracePath(current);
                }

                foreach (Tile nextTile in current.tile.Adjacents)
                {
                    if (DoneList.Exists(n => n.tile == nextTile)) continue;

                    double newCost = current.costSoFar + 10;
                    Node nextNode = TODOList.Find(n => n.tile == nextTile);

                    if (nextTile.IsEnemyTile())
                    {
                        newCost += 30; // Increase cost for enemy tiles
                    }

                    if (nextNode == null)
                    {
                        nextNode = new Node(nextTile, newCost + HeuristicsDistance(nextTile, goal), current, newCost);
                        TODOList.Add(nextNode);
                    }
                    else if (newCost < nextNode.costSoFar)
                    {
                        nextNode.costSoFar = newCost;
                        nextNode.priority = newCost + HeuristicsDistance(nextTile, goal);
                        nextNode.cameFrom = current;
                    }
                }
            }
            return new Queue<Tile>(); // Returns an empty path if no path is found
        }

        double HeuristicsDistance(Tile currentTile, Tile goalTile)
        {
            int xdist = Math.Abs(goalTile.indexX - currentTile.indexX);
            int ydist = Math.Abs(goalTile.indexY - currentTile.indexY);
            return (xdist * 10 + ydist * 10);
        }

        Queue<Tile> RetracePath(Node node)
        {
            List<Tile> tileList = new List<Tile>();
            Node nodeIterator = node;
            while (nodeIterator.cameFrom != null)
            {
                tileList.Insert(0, nodeIterator.tile);
                nodeIterator = nodeIterator.cameFrom;
            }
            return new Queue<Tile>(tileList);
        }

        public Queue<Tile> RandomPath(Tile start, int stepNumber)
        {
            List<Tile> tileList = new List<Tile>();
            Tile currentTile = start;
            for (int i = 0; i < stepNumber; i++)
            {
                Tile nextTile;
                if (currentTile.Adjacents.Count < 0)
                {
                    break;
                }
                else if (currentTile.Adjacents.Count == 1)
                {
                    nextTile = currentTile.Adjacents[0];
                }
                else
                {
                    nextTile = null;
                    List<Tile> adjacentList = new List<Tile>(currentTile.Adjacents);
                    ShuffleTiles<Tile>(adjacentList);
                    if (tileList.Count <= 0) nextTile = adjacentList[0];
                    else
                    {
                        foreach (Tile tile in adjacentList)
                        {
                            if (tile != tileList[tileList.Count - 1])
                            {
                                nextTile = tile;
                                break;
                            }
                        }
                    }
                }
                tileList.Add(currentTile);
                currentTile = nextTile;
            }
            return new Queue<Tile>(tileList);
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