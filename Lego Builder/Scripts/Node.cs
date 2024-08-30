using UnityEngine;
using System.Collections.Generic;

public class Node
{
    public GameObject GameObject { get; }
    public List<Node> Neighbors { get; }

    public Node(GameObject gameObject)
    {
        GameObject = gameObject;
        Neighbors = new List<Node>();
    }

    public void AddNeighbor(Node neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
        }
    }
}
