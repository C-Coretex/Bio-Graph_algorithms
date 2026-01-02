using CycleBreakCalculator.Engine;
using CycleBreakCalculator.Helpers;

//cannot use any library methods that are not constant time because of the task requirements

var (edges, vertexCount) = InputProcessor.ProcessInput(await File.ReadAllTextAsync("./input.txt"));

var resultEdges = CycleBreakCalculationEngine.CalculateCycleBreaks(edges, vertexCount);

using var fileStream = new FileStream("./output.txt", FileMode.Create, FileAccess.Write);
using var writer = new StreamWriter(fileStream);

var totalWeight = 0;
foreach (var edge in resultEdges)
    totalWeight += edge.Weight;

writer.Write($"{resultEdges.Count} {totalWeight} {(char)0x0A}");
foreach (var edge in resultEdges)
{
    writer.Write($"{edge.From} {edge.To}\t");
}

Console.WriteLine("Cycle breaks calculation completed.");