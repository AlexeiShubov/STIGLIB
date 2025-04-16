using System;
using System.Collections.Generic;

namespace PathFinder
{
    public class PathFinderWave : IPathFinder<Tuple<int, int>, INode<bool>>
    {
        private Dictionary<INode<bool>, HashSet<INode<bool>>> _neighboursMap;

        public INodePattern<Tuple<int, int>, INode<bool>> NodePattern { get; }

        public PathFinderWave(INodePattern<Tuple<int, int>, INode<bool>> nodePattern)
        {
            NodePattern = nodePattern;
        }
        
        public Dictionary<int, HashSet<INode<bool>>> GetPathWave(List<INode<bool>> from)
        {
            _neighboursMap = NodePattern.GenerateNeighboursMap();
            
            var step = 0;
            var q = new Queue<HashSet<INode<bool>>>();
            var visitedCells = new HashSet<INode<bool>>(from);
            var pathWave = new Dictionary<int, HashSet<INode<bool>>>{{step++, new HashSet<INode<bool>>(from)}};

            q.Enqueue(new HashSet<INode<bool>>(from));

            while (q.Count > 0)
            {
                var newWave = GetWave(q.Dequeue(), visitedCells);
                
                if (newWave.Count == 0) continue;

                foreach (var nodeData in newWave)
                {
                    visitedCells.Add(nodeData);
                }
                
                q.Enqueue(newWave);
                pathWave.Add(step++, newWave);
            }
            
            return pathWave;
        }

        private HashSet<INode<bool>> GetWave(HashSet<INode<bool>> nodes, HashSet<INode<bool>> visitedNodeDatas)
        {
            var result = new HashSet<INode<bool>>();

            foreach (var node in nodes)
            {
                if (!visitedNodeDatas.Contains(node))
                {
                    result.Add(node);
                }
                
                if (!_neighboursMap.ContainsKey(node)) continue;
                
                foreach (var newNodeData in _neighboursMap[node])
                {
                    if(visitedNodeDatas.Contains(newNodeData)) continue;
                    
                    result.Add(newNodeData);
                }
            }

            return result;
        }
    }
}