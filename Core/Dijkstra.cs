public class Dijkstra<TNode>
{
    public int FindShortestUnlockPath(
        Graph<TNode> graph,
        TNode start,
        TNode target,
        IEqualityComparer<TNode> comparer)
    {
        var distances = new Dictionary<TNode, int>();
        var priorityQueue = new PriorityQueue<(TNode, int), int>();
        var unlockedSkills = new HashSet<TNode>();

        foreach (var node in graph.Nodes)
            distances[node] = int.MaxValue;

        distances[start] = 0;
        priorityQueue.Enqueue((start, 0), 0);

        while (priorityQueue.Count > 0)
        {
            var (currentSkill, currentDistance) = priorityQueue.Dequeue();

            if (unlockedSkills.Contains(currentSkill))
                continue;
            unlockedSkills.Add(currentSkill);

            if (comparer.Equals(currentSkill, target)) //return when target node is reached
                return currentDistance;

            var adjacentNodes = graph.GetAdjacentNodes(currentSkill);
            if (adjacentNodes == null)
                continue;

            foreach (var neighborNode in adjacentNodes)
            {
                int newDist = currentDistance + graph.GetEdgeWeight(currentSkill, neighborNode);
                if (newDist < distances[neighborNode])
                {
                    distances[neighborNode] = newDist;
                    priorityQueue.Enqueue((neighborNode, newDist), newDist);
                }
            }
        }

        return -1; //cannot reach target
    }
}
