public class Graph<TNode>
{
    public HashSet<TNode> Nodes => nodes;
    private readonly HashSet<TNode> nodes = new HashSet<TNode>();
    private readonly Dictionary<TNode, HashSet<TNode>> edges = new Dictionary<TNode, HashSet<TNode>>();
    private readonly Dictionary<TNode, Dictionary<TNode, int>> edgeWeights = new Dictionary<TNode, Dictionary<TNode, int>>();

    public void AddNode(TNode node)
    {
        if (nodes.Contains(node))
            return;

        nodes.Add(node);
        edges[node] = new HashSet<TNode>();
    }

    public void RemoveNode(TNode node)
    {
        if (!nodes.Remove(node))
            return;

        edges.Remove(node);
        foreach (var adjacentNodes in edges.Values)
        {
            adjacentNodes.Remove(node);
        }
    }

    public HashSet<TNode>? GetAdjacentNodes(TNode node)
    {
        return edges.TryGetValue(node, out var nodeSet) ? nodeSet : null;
    }

    public void AddEdge(TNode source, TNode target, int weight = 1)
    {
        AddNode(source);
        AddNode(target);

        edges[source].Add(target);
        if (!edgeWeights.TryGetValue(source, out var weightDict))
        {
            weightDict = new Dictionary<TNode, int>();
            edgeWeights[source] = weightDict;
        }
        weightDict[target] = weight;
    }

    public void RemoveEdge(TNode source, TNode target)
    {
        if (edges.TryGetValue(source, out var nodeSet))
        {
            nodeSet.Remove(target);
        }
        if (edgeWeights.TryGetValue(source, out var weightDict))
        {
            weightDict.Remove(target);
        }
    }

    public bool HasEdge(TNode source, TNode target)
    {
        return edges.TryGetValue(source, out var nodeSet) && nodeSet.Contains(target);
    }

    public void SetEdgeWeight(TNode source, TNode target, int weight)
    {
        if (!edgeWeights.TryGetValue(source, out Dictionary<TNode, int>? value))
        {
            value = new Dictionary<TNode, int>();
            edgeWeights[source] = value;
        }

        value[target] = weight;
    }

    public int GetEdgeWeight(TNode source, TNode target)
    {
        if (edgeWeights.TryGetValue(source, out var targets) && targets.TryGetValue(target, out var weight))
        {
            return weight;
        }

        return 1;
    }
}

