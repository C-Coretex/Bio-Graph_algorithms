using CycleBreakCalculator.Helpers;
using CycleBreakCalculator.Models;

namespace CycleBreakCalculator.Engine
{
    internal static class CycleBreakCalculationEngine
    {
        public static ICollection<Edge> CalculateCycleBreaks(Edge[] edges, int vertexCount)
        {
            var cycleBreaks = new LinkedList<Edge>();
            var disjointSet = new DisjointSetUnion(vertexCount);

            foreach (var edge in edges)
            {
                if (edge.Weight < 0) //as we want to minimize weight, not cycleBreaks count - we want to add every edge with negative weight
                {
                    cycleBreaks.AddLast(edge);
                    continue;
                }

                if (!disjointSet.TryUnion(edge.From - 1, edge.To - 1))
                    cycleBreaks.AddLast(edge);
            }

            return cycleBreaks;
        }
    }
}
