public class SkillTree
{
    private Dictionary<string, SkillNode> skills = new Dictionary<string, SkillNode>();

    public void AddSkill(SkillNode skill)
    {
        skills[skill.Id] = skill;
    }

    public bool UnlockSkill(string skillId)
    {
        if (!skills.TryGetValue(skillId, out var skill))
            return false;

        foreach (var prereq in skill.Prerequisites)
        {
            if (!skills.TryGetValue(prereq, out var prereqSkill) || !prereqSkill.IsUnlocked)
            {
                return false;
            }
        }

        skill.IsUnlocked = true;
        return true;
    }

    public Graph<SkillNode> GetDependencyGraph()
    {
        var graph = new Graph<SkillNode>();
        foreach (var skill in skills.Values)
        {
            graph.AddNode(skill);
            foreach (var preSkill in skill.Prerequisites)
            {
                graph.AddEdge(skills[preSkill], skill, skill.Cost);
            }
        }
        return graph;
    }

    public List<SkillNode> GetTopologicalOrder()
    {
        var sorter = new TopologicalSort<SkillNode>();
        var dependencyGraph = GetDependencyGraph();
        var unlockOrder = sorter.Sort(dependencyGraph);

        return unlockOrder;
    }

    public int CalculateShortestUnlockCost(string startSkillId, string targetSkillId, bool includeStartSkillCost)
    {
        var startSkillPathCost = CalculateNodePathCost(startSkillId);
        var targetSkillPathCost = CalculateNodePathCost(targetSkillId);
        var result = targetSkillPathCost - startSkillPathCost;
        if (includeStartSkillCost)
            result += skills[startSkillId].Cost;

        return result;
    }

    private int CalculateNodePathCost(string nodeId)
    {
        var traversedList = new HashSet<string>();
        return CalculateCost(nodeId);

        //sub function
        int CalculateCost(string current)
        {
            if (!skills.TryGetValue(current, out var node))
                throw new ArgumentException($"Node {current} not found.");

            if (traversedList.Contains(current))
                return 0;
            traversedList.Add(current);

            if (node.Prerequisites.Count == 0)
                return node.Cost;

            //recursive
            var prerequisitesCost = node.Prerequisites.Sum(prereqIds => CalculateCost(prereqIds));
            return prerequisitesCost + node.Cost;
        }
    }

    /// <summary>
    /// This does not consider prerequisites
    /// </summary>
    /// <param name="sourceNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int GetShortestUnlockPath(string sourceNode, string targetNode)
    {
        if (!skills.TryGetValue(sourceNode, out var sourceSkill))
            throw new Exception($"Cannot find node ID: {sourceNode} in skill tree.");
        if (!skills.TryGetValue(targetNode, out var targetSkill))
            throw new Exception($"Cannot find node ID: {targetNode} in skill tree.");

        var dijkstra = new Dijkstra<SkillNode>();
        var dependencyGraph = GetDependencyGraph();
        var comparer = EqualityComparer<SkillNode>.Default;
        return dijkstra.FindShortestUnlockPath(dependencyGraph, sourceSkill, targetSkill, comparer);
    }
}
