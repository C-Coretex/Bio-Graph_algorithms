namespace ProteinLocalAlignmentCalculator.Models
{
    internal class ProteinSequence(string sequence)
    {
        public string Sequence { get; } = sequence;
        public required string Name { get; init; }
        public bool IsX => Name.Contains('X');

        public override string ToString()
        {
            return $"Sequence {(IsX ? 'X' : 'Y')}:" + Environment.NewLine + Name + Environment.NewLine + Sequence;
        }

        public static ProteinSequence FromFasta(string fastaContent)
        {
            var lines = fastaContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2)
                throw new ArgumentException("Invalid FASTA content.");

            // Skip the first line (header) and concatenate the rest
            var sequence = string.Concat(lines[1..]);
            return new ProteinSequence(sequence)
            {
                Name = lines[0]
            };
        }
    }
}
