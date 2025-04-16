using System;
using System.Collections.Generic;
using PathFinder;

namespace FM
{
    public class NodeMapController2DFM
    {
        private readonly List<INode<bool>> _nodes;
        private readonly INodePattern<Tuple<int, int>, INode<bool>> _nodePattern;
        private readonly PathFinderWave _pathFinder;

        public NodeMapController2DFM(List<INode<bool>> nodes, (int width, int hight) mapSize, List<Tuple<int, int>> patterns)
        {
            _nodes = nodes;

            _nodePattern = new NodePatternWave2D(GetNodesMap((mapSize.width, mapSize.hight)), patterns);
            _pathFinder = new PathFinderWave(_nodePattern);
        }

        public Dictionary<int, HashSet<INode<bool>>> GetPathWave(List<INode<bool>> from)
        {
            var result = new Dictionary<int, HashSet<INode<bool>>>();
            var pathWave = _pathFinder.GetPathWave(from);

            for (var i = 0; i < pathWave.Count; i++)
            {
                result.Add(i, pathWave[i]);
            }
            
            return result;
        }

        private INode<bool>[,] GetNodesMap((int width, int hight) mapSize)
        {
            var nodesMap = new INode<bool>[mapSize.width, mapSize.hight];
            
            foreach (var node in _nodes)
            {
                var x = (node.NodeData.Level - 1) / nodesMap.GetLength(0);
                var y = (node.NodeData.Level - 1) % nodesMap.GetLength(0);
                
                nodesMap[x, y] = node;
            }
            
            return nodesMap;
        }
    }
}