namespace CycleBreakCalculator.Helpers
{
    internal class DisjointSetUnion
    {
        private readonly int[] _parent, _rank;

        public DisjointSetUnion(int vertexCount)
        {
            _parent = new int[vertexCount];
            _rank = new int[vertexCount];

            for (int i = 0; i < vertexCount; i++)
                _parent[i] = i;
        }

        public int Find(int i)
        {
            var root = _parent[i];

            //optimization, we save the root for each node
            if (_parent[root] != root)
                return _parent[i] = Find(root);

            return root;
        }

        /// <returns>If Union applied</returns>
        public bool TryUnion(int a, int b)
        {
            var aRoot = Find(a);
            var bRoot = Find(b);

            if (aRoot == bRoot)
                return false;

            //rank - optimization, we minimize the height of the resulting tree
            if (_rank[aRoot] < _rank[bRoot])
                _parent[aRoot] = bRoot; //no need to update rank here, since height won't change (we attach smaller tree to bigger one)
            else if (_rank[aRoot] > _rank[bRoot])
                _parent[bRoot] = aRoot;
            else
            {
                _parent[bRoot] = aRoot;
                _rank[aRoot] = _rank[aRoot] + 1;
            }

            return true;
        }
    }
}
