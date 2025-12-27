using ProteinLocalAlignmentCalculator.Models;
using System;
using System.Data;
using System.Text;

namespace ProteinLocalAlignmentCalculator.Engine
{
    /// <summary>
    /// Smith-Waterman algorithm
    /// </summary>
    internal class LocalAlignmentCalculator
    {
        private readonly ProteinSequence _seqX;
        private readonly ProteinSequence _seqY;
        private readonly SimilarityMatrix _similarityMatrix;
        private readonly int _gapOpenPenalty;
        private readonly int _gapExtendPenalty;

        private List<Rule>[,] _ruleMatrix;
        private int[,] _substitutionScore;
        private int[,] _gapXScore;
        private int[,] _gapYScore;

        public LocalAlignmentCalculator(
            ProteinSequence seqX,
            ProteinSequence seqY,
            SimilarityMatrix similarityMatrix,
            int? gapOpenPenalty = null,
            int? gapExtendPenalty = null)
        {
            _seqX = seqX;
            _seqY = seqY;
            _similarityMatrix = similarityMatrix;
            _gapOpenPenalty = gapOpenPenalty ?? 0;
            _gapExtendPenalty = gapExtendPenalty ?? 0;

            _ruleMatrix = new List<Rule>[seqX.Size, seqY.Size];
            _substitutionScore = new int[seqX.Size, seqY.Size];
            _gapXScore = new int[seqX.Size, seqY.Size];
            _gapYScore = new int[seqX.Size, seqY.Size];

            for (var i = 1; i < seqX.Size; i++)
                _gapXScore[i, 0] = -_gapOpenPenalty - i * _gapExtendPenalty;

            for (var j = 1; j < seqY.Size; j++)
                _gapYScore[0, j] = -_gapOpenPenalty - j * _gapExtendPenalty;
        }

        public void CalculateOptimalAlignment()
        {
            for (var i = 1; i < _seqX.Size; i++)
            {
                for (var j = 1; j < _seqY.Size; j++)
                {
                    var matchScore = _similarityMatrix.GetScore(_seqX.Sequence[i], _seqY.Sequence[j]);

                    _gapXScore[i, j] = new[]
                    {
                        _substitutionScore[i - 1, j] - _gapOpenPenalty - _gapExtendPenalty,
                        _gapXScore[i - 1, j] - _gapExtendPenalty,
                    }.Max();

                    _gapYScore[i, j] = new[]
                    {
                        _substitutionScore[i, j - 1] - _gapOpenPenalty - _gapExtendPenalty,
                        _gapYScore[i, j - 1] - _gapExtendPenalty,
                    }.Max();

                    (int score, Rule rule)[] possibleMoves =
                    [
                        (_gapYScore[i - 1, j - 1] + matchScore, Rule.GapY),
                        (_gapXScore[i - 1, j - 1] + matchScore, Rule.GapX),
                        (_substitutionScore[i - 1, j - 1] + matchScore, Rule.Substitution),
                        (0, Rule.None)
                    ];

                    //Correct answer printed, algorithm modified
                    /*
                    (int score, Rule rule)[] possibleMoves =
                    [
                        (_substitutionScore[i - 1, j - 1] + matchScore, Rule.Substitution),
                        (_gapYScore[i, j], Rule.GapY),
                        (_gapXScore[i, j], Rule.GapX),
                        (0, Rule.None)
                    ];
                    */
                    //Incorrect answer, algorithm modified (we just show gaps first if there are multiple variants)
                    /*
                    (int score, Rule rule)[] possibleMoves =
                    [
                        (_gapYScore[i, j], Rule.GapY),
                        (_gapXScore[i, j], Rule.GapX),
                        (_substitutionScore[i - 1, j - 1] + matchScore, Rule.Substitution),
                        (0, Rule.None)
                    ];
                    */

                    var maxMove = possibleMoves.Max(p => p.score);
                    _substitutionScore[i, j] = maxMove;
                    var bestMove = possibleMoves.Where(pm => pm.score == maxMove).Select(pm => pm.rule).ToList();
                    _ruleMatrix[i, j] = bestMove;
                }
            }
        }

        public int Score => _substitutionScore.Cast<int>().Max();

        public string GetAlignmentText()
        {
            var score = Score;
            List<Rule> steps = [];

            for (var i = 1; i < _seqX.Size; i++)
            {
                for (var j = 1; j < _seqY.Size; j++)
                {
                    if (_substitutionScore[i, j] == score)
                    {
                        var x = i;
                        var y = j;
                        while (_ruleMatrix[x, y]?.All(x => x != Rule.None) == true)
                        {
                            var ttt = _ruleMatrix[x, y];
                            var rule = _ruleMatrix[x, y].First();
                            steps.Insert(0, rule);
                            switch (rule)
                            {
                                case Rule.Substitution:
                                    x--;
                                    y--;
                                    break;
                                case Rule.GapX:
                                    x--;
                                    break;
                                case Rule.GapY:
                                    y--;
                                    break;
                            }
                        }

                        var sbX = new StringBuilder();
                        var sbY = new StringBuilder();
                        var sbStep = new StringBuilder();

                        var seqXEnumerator = _seqX.Sequence.GetEnumerator();
                        var seqYEnumerator = _seqY.Sequence.GetEnumerator();

                        var firstAlignmentIndex = Math.Min(x, y) + 1; //+1 because initial alignment will be next step (since we stopped on Rule.None)
                        var maxLen = Math.Max(_seqX.Size, _seqY.Size);
                        for (var k = 0; k < maxLen; k++)
                        {
                            var stepIndex = k - firstAlignmentIndex;
                            var step = stepIndex >= 0 && stepIndex < steps.Count ? (Rule?)steps[stepIndex] : null;

                            if (step != Rule.GapX)
                            {
                                if (seqXEnumerator.MoveNext())
                                    sbX.Append(seqXEnumerator.Current);
                            }
                            else
                                sbX.Append('-');

                            if (step != Rule.GapY)
                            {
                                if (seqYEnumerator.MoveNext())
                                    sbY.Append(seqYEnumerator.Current);
                            }
                            else
                                sbY.Append('-');

                            sbStep = step switch
                            {
                                Rule.Substitution => sbStep.Append(seqXEnumerator.Current == seqYEnumerator.Current ? '=' : '+'),
                                Rule.GapX => sbStep.Append('-'),
                                Rule.GapY => sbStep.Append('-'),
                                _ => sbStep.Append(' ')
                            };
                        }

                        var newScore = _similarityMatrix.GetScore('D', 'L');
                        //newScore += -2;
                        //newScore += -1;
                        //newScore += -1;
                        //newScore += -1;
                        //newScore += -1;
                        //newScore += -1;
                        newScore += _similarityMatrix.GetScore('G', 'A');
                        newScore += _similarityMatrix.GetScore('D', 'R');
                        newScore += _similarityMatrix.GetScore('G', 'N');
                        newScore += _similarityMatrix.GetScore('D', 'D');
                        newScore += _similarityMatrix.GetScore('G', 'R');
                        newScore += _similarityMatrix.GetScore('D', 'V');
                        //newScore += -2;
                        //newScore += -2;
                        newScore += _similarityMatrix.GetScore('R', 'V');
                        newScore += _similarityMatrix.GetScore('N', 'A');
                        newScore += _similarityMatrix.GetScore('D', 'R');
                        newScore += _similarityMatrix.GetScore('K', 'N');
                        newScore += _similarityMatrix.GetScore('I', 'D');
                        newScore += _similarityMatrix.GetScore('L', 'W');
                        newScore += _similarityMatrix.GetScore('I', 'W');
                        newScore += _similarityMatrix.GetScore('A', 'W');
                        newScore += _similarityMatrix.GetScore('R', 'W');
                        newScore += _similarityMatrix.GetScore('N', 'W');

                        return sbX.ToString() + Environment.NewLine +
                               sbY.ToString() + Environment.NewLine +
                               sbStep.ToString();
                    }
                }
            }

            return "Not found";
        }

        private enum Rule
        {
            None = 0,
            Substitution,
            GapX,
            GapY,
        }
    }
}
