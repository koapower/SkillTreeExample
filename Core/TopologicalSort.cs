public class TopologicalSort<TNode>
{
    HashSet<TNode> visited = new HashSet<TNode>();
    HashSet<TNode> visiting = new HashSet<TNode>();

    public List<TNode> Sort(Graph<TNode> graph)
    {
        var result = new List<TNode>();
        visited.Clear();
        visiting.Clear();

        foreach (var node in graph.Nodes)
        {
            if (!visited.Contains(node))
            {
                if (!DFS(graph, node, result))
                {
                    throw new InvalidOperationException("Graph contains a cycle, topological sort is not possible.");
                }
            }
        }

        result.Reverse();
        return result;
    }

    private bool DFS(Graph<TNode> graph, TNode node, List<TNode> result)
    {
        if (visiting.Contains(node)) return false;

        if (!visited.Contains(node))
        {
            visiting.Add(node);

            var adjacentNodes = graph.GetAdjacentNodes(node);
            if (adjacentNodes != null)
            {
                foreach (var adjacent in adjacentNodes)
                {
                    if (!DFS(graph, adjacent, result))
                    {
                        return false;
                    }
                }
            }

            visiting.Remove(node);
            visited.Add(node);
            result.Add(node);
        }

        return true;
    }

}
