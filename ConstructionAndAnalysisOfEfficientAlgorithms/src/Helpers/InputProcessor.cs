using CycleBreakCalculator.Models;

namespace CycleBreakCalculator.Helpers
{
    internal static class InputProcessor
    {
        public static (Edge[] Edges, int VertexCount) ProcessInput(string text)
        {
            var numbers = GetNumbers(text);
            var (edges, vertexCount) = GetEdges(numbers);
            var edgesArray = SortEdgesByWeight(edges);
            return (edgesArray, vertexCount);
        }

        private static ICollection<int> GetNumbers(string text)
        {
            var inputNumbers = new LinkedList<int>();
            var value = 0;
            var isNumber = false;
            var isNegative = false;

            //didn't use .Split().Select(int.Parse) since those are not constant time methods
            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                if (ch >= '0' && ch <= '9')
                {
                    value = value * 10 + (ch - '0');
                    isNumber = true;
                }
                else
                {
                    if (isNumber)
                    {
                        value = isNegative ? -value : value;
                        inputNumbers.AddLast(value);
                        value = 0;
                    }

                    isNegative = ch == '-';
                    isNumber = false;
                }
            }
            if (isNumber)
            {
                value = isNegative ? -value : value;
                inputNumbers.AddLast(value);
                value = 0;
            }

            return inputNumbers;
        }

        private static (ICollection<Edge> edge, int vertexCount) GetEdges(IEnumerable<int> numbers)
        {
            var enumerator = numbers.GetEnumerator();
            enumerator.MoveNext();
            var vertexCount = enumerator.Current;

            LinkedList<Edge> edges = [];
            while (enumerator.MoveNext())
            {
                var from = enumerator.Current;
                if (!enumerator.MoveNext())
                    break;

                var to = enumerator.Current;
                if (!enumerator.MoveNext())
                    break;

                var weight = enumerator.Current;

                var edge = new Edge
                {
                    From = from,
                    To = to,
                    Weight = weight
                };
                edges.AddLast(edge);
            }

            return (edges, vertexCount);
        }

        private static Edge[] SortEdgesByWeight(ICollection<Edge> edges)
        {
            var edgeArray = new Edge[edges.Count];
            var i = 0;
            foreach (var edge in edges)
                edgeArray[i++] = edge;

            var sorter = new QuickSort<Edge>((e1, e2) =>
                (e1.Weight - e2.Weight) switch
                {
                    > 0 => 1,
                    < 0 => -1,
                    _ => 0
                }, isAscending: false);

            sorter.Sort(ref edgeArray);
            return edgeArray;
        }
    }
}
