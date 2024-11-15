using System;

public class Node
{
    public Node cameFrom; // Parent node
    public double priority; // F value
    public double costSoFar; // G value
    public Tile tile;

    public Node(Tile _tile, double _priority, Node _cameFrom, double _costSoFar)
    {
        cameFrom = _cameFrom;
        priority = _priority;
        costSoFar = _costSoFar;
        tile = _tile;
    }
}