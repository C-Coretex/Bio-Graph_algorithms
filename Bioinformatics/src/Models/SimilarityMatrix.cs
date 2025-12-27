namespace ProteinLocalAlignmentCalculator.Models
{
    internal class SimilarityMatrix(int[][] matrix)
    {
        public const string AminoAcids = "ARNDCQEGHILKMFPSTWYV";

        public required string FileName { get; init; }

        public int GetScore(char a, char b)
        {
            int row = CharToIndex(a);
            int col = CharToIndex(b);

            return matrix[row][col];
        }

        private static int CharToIndex(char c)
        {
            int index = AminoAcids.IndexOf(c);
            if (index == -1)
                throw new ArgumentException($"Invalid character: {c}");

            return index;
        }

        public static SimilarityMatrix FromText(string[] lines, string fileName)
        {
            lines = [.. lines.Where(line => !string.IsNullOrWhiteSpace(line) && AminoAcids.Contains(line[0]))];
            var matrix = new int[20][];
            for (int i = 0; i < 20; i++)
                matrix[i] = [.. lines[i].Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1).Take(20).Select(int.Parse)];

            return new SimilarityMatrix(matrix)
            {
                FileName = fileName
            };
        }
    }
}
