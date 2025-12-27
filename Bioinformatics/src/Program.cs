using ProteinLocalAlignmentCalculator.Engine;
using ProteinLocalAlignmentCalculator.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        //input
        var data = LoadInputFiles();

        if(data.seq1.IsX && data.seq2.IsX)
        {
            Console.WriteLine("Two sequences cannot be the same (X and X or Y and Y).");
            return;
        }

        int? gapOpenPenalty = null;
        if(args.ElementAtOrDefault(0) is { } o)
            gapOpenPenalty = int.Parse(o);
        int? gapExtendPenalty = null;
        if (args.ElementAtOrDefault(1) is { } e)
            gapExtendPenalty = int.Parse(e);

        //calculation
        ProteinSequence[] sequences = [data.seq1, data.seq2];

        var calculator = new LocalAlignmentCalculator(
            sequences.First(s => s.IsX),
            sequences.First(s => !s.IsX),
            data.similatiryMatrix,
            gapOpenPenalty,
            gapExtendPenalty
        );
        calculator.CalculateOptimalAlignment();

        //output
        Console.WriteLine(sequences.First(s => s.IsX));
        Console.WriteLine();
        Console.WriteLine(sequences.First(s => !s.IsX));

        Console.WriteLine();
        Console.WriteLine("Similarity matrix:");
        Console.WriteLine(data.similatiryMatrix.FileName);

        Console.WriteLine();
        Console.WriteLine("Gap penalties:");
        Console.WriteLine($"h: {gapOpenPenalty?.ToString() ?? "-"}");
        Console.WriteLine($"g: {gapExtendPenalty?.ToString() ?? "-"}");

        Console.WriteLine();
        Console.WriteLine($"SW score: {calculator.Score}");

        Console.WriteLine();
        Console.WriteLine("Alignment:");
        Console.WriteLine(calculator.GetAlignmentText());

        Console.ReadKey();
    }

    private static (ProteinSequence seq1, ProteinSequence seq2, SimilarityMatrix similatiryMatrix) LoadInputFiles()
    {
        var di = new DirectoryInfo(".");

        var sequenceFiles = di.GetFiles("*.fa");

        if (sequenceFiles.Length != 2)
        {
            Console.WriteLine("Please ensure there are exactly 2 .fa files in the directory.");
            throw new InvalidOperationException();
        }


        var similarityMatrixFiles = di.GetFiles("*.mat");

        if (similarityMatrixFiles.Length != 1)
        {
            Console.WriteLine("Please ensure there is exactly 1 .mat file in the directory.");
            throw new InvalidOperationException();
        }


        var similarityMatrix = SimilarityMatrix.FromText(
                File.ReadAllLines(similarityMatrixFiles[0].FullName), 
                similarityMatrixFiles[0].Name
            );

        return (
            ProteinSequence.FromFasta(File.ReadAllText(sequenceFiles[0].FullName)),
            ProteinSequence.FromFasta(File.ReadAllText(sequenceFiles[1].FullName)),
            similarityMatrix
        );
    }
}