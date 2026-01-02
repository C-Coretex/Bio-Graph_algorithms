namespace CycleBreakCalculator.Models
{
    internal record Edge
    {
        public int From { get; init; }
        public int To { get; init; }
        public int Weight { get; init; }
    }
}
