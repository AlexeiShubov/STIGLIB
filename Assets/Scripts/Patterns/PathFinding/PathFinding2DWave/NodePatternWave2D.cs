using System;
using System.Collections.Generic;

namespace PathFinder
{
    public class NodePatternWave2D : INodePattern<Tuple<int, int>, INode<bool>>
    {
        private readonly INode<bool>[,] _nodesMap;
        
        public IReadOnlyList<Tuple<int, int>> Patterns { get; }

        public NodePatternWave2D(INode<bool>[,] nodesMap, IReadOnlyList<Tuple<int, int>> patterns)
        {
            _nodesMap = nodesMap;
            Patterns = patterns;
        }
        
        public Dictionary<INode<bool>, HashSet<INode<bool>>> GenerateNeighboursMap()
        {
            var neighboursMap = new Dictionary<INode<bool>, HashSet<INode<bool>>>();
            
            for (var i = 0; i < _nodesMap.GetLength(1); i++)
            {
                for (var j = 0; j < _nodesMap.GetLength(0); j++)
                {
                    var neighbours = GetNeighbours(Tuple.Create(j, i));

                    if (neighbours.Count == 0) continue;

                    neighboursMap.Add(_nodesMap[j, i], neighbours);
                }
            }

            return neighboursMap;
        }
        
        public HashSet<INode<bool>> GetNeighbours(Tuple<int, int> nodePosition)
        {
            var result = new HashSet<INode<bool>>();

            foreach (var pattern in Patterns)
            {
                var (x, y) = (pattern.Item1 + nodePosition.Item1, pattern.Item2 + nodePosition.Item2);

                if (x < 0 || y < 0) continue;
                if (x >= _nodesMap.GetLength(0) || y >= _nodesMap.GetLength(1)) continue;

                result.Add(_nodesMap[x, y]);
            }

            return result;
        }
    }
}