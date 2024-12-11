internal class Program
{
    static void Main(string[] args)
    {
        //       A
        //      / \
        //     B   C    E
        //      \  /    /
        //        D   F
        //         \ /
        //          G
        var nodeList = new List<SkillNode>()
        {
            new SkillNode("A", "Arcane Blast") { Cost = 1,
                //uncomment this will cause exception because it makes a cycle 
                //Prerequisites = new List<string>() { "D" }
            },
            new SkillNode("B", "Burning Slash") { Cost = 2, Prerequisites = new List<string>() { "A" } },
            new SkillNode("C", "Crystal Shield") { Cost = 3, Prerequisites = new List<string>() { "A" } },
            new SkillNode("D", "Demon Meteor") { Cost = 5, Prerequisites = new List<string>() { "B", "C" } },
            new SkillNode("E", "Extra Punch") { Cost = 1 },
            new SkillNode("F", "Fierce Punch") { Cost = 2, Prerequisites = new List<string>() { "E" } },
            new SkillNode("G", "Gas Bomb") { Cost = 7, Prerequisites = new List<string>() { "D", "F" } },
        };

        var skillTree = new SkillTree();
        foreach (var node in nodeList)
        {
            skillTree.AddSkill(node);
        }

        try
        {
            // Topological Order
            var unlockOrder = skillTree.GetTopologicalOrder();
            Console.WriteLine("[Topological Unlock Order]");
            foreach (var skill in unlockOrder)
            {
                Console.WriteLine($"({skill.Id}) {skill.Name} (Cost: {skill.Cost})");
            }
            var from = "C";
            var to = "G";
            var cost = skillTree.CalculateShortestUnlockCost(from, to, false);
            Console.WriteLine($"\n[Unlock Cost] Unlock cost from {from} to {to} is {cost}");

            // I wanted to showcase Dijkstra as well but it is not natural for Dijkstra to handle skill prerequisites problems
            var lowestCost = skillTree.GetShortestUnlockPath(from, to);
            Console.WriteLine($"\n[Dijkstra] Lowest cost from {from} to {to} (without prerequisites) is {lowestCost}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}


