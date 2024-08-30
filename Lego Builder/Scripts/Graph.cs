using System.Collections.Generic;

public class Graph
{
    private List<Node> nodes;

    public Graph()
    {
        nodes = new List<Node>();
    }

    public void AddNode(Node node)
    {
        if (!nodes.Contains(node))
        {
            nodes.Add(node);
        }
    }

    public void AddDirectedEdge(Node from, Node to)
    {
        from.AddNeighbor(to);
    }

    public List<Node> GetNodes()
    {
        return nodes;
    }
}
