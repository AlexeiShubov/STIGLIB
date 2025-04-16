using System.Collections.Generic;

namespace PathFinder
{
    public class PathFinder<T>
    {
        private readonly INode<T>[,] _cellsMap;
        private readonly List<(int x, int y)> _patterns;
        private readonly Dictionary<INode<T>, HashSet<INode<T>>> _neighboursMap;

        public PathFinder(INode<T>[,] cellsMap, IEnumerable<(int, int)> patterns)
        {
            _cellsMap = cellsMap;
            _patterns = new List<(int, int)>(patterns);
            _neighboursMap = new Dictionary<INode<T>, HashSet<INode<T>>>();
            
            GenerateNeighboursMap();
        }

        public Dictionary<int, HashSet<INode<T>>> GetPathWave(INode<T> from)
        {
            var step = 0;
            var q = new Queue<HashSet<INode<T>>>();
            var visitedCells = new HashSet<INode<T>>();
            var pathWave = new Dictionary<int, HashSet<INode<T>>>{{step++, new HashSet<INode<T>>{from}}};

            q.Enqueue(new HashSet<INode<T>>{from});
            visitedCells.Add(from);

            while (q.Count > 0)
            {
                var newWave = GetWave(q.Dequeue(), visitedCells);
                
                if (newWave.Count == 0) continue;

                foreach (var cell in newWave)
                {
                    visitedCells.Add(cell);
                }
                
                q.Enqueue(newWave);
                pathWave.Add(step++, newWave);
            }
            
            return pathWave;
        }
        
        private HashSet<INode<T>> GetWave(HashSet<INode<T>> list, HashSet<INode<T>> visitedCells)
        {
            var result = new HashSet<INode<T>>();

            foreach (var cell in list)
            {
                if (!visitedCells.Contains(cell))
                {
                    result.Add(cell);
                }
                
                if (!_neighboursMap.ContainsKey(cell)) continue;
                
                foreach (var newCell in _neighboursMap[cell])
                {
                    if(visitedCells.Contains(newCell)) continue;
                    
                    result.Add(newCell);
                }
            }

            return result;
        }

        private void GenerateNeighboursMap()
        {
            for (var i = 0; i < _cellsMap.GetLength(1); i++)
            {
                for (var j = 0; j < _cellsMap.GetLength(0); j++)
                {
                    var neighbours = GetNeighbours((j, i));
                    
                    if(neighbours.Count == 0) continue;
                    
                    _neighboursMap.Add(_cellsMap[j, i], neighbours);
                }
            }
        }

        private HashSet<INode<T>> GetNeighbours((int x, int y) cellPosition)
        {
            var result = new HashSet<INode<T>>();

            foreach (var pattern in _patterns)
            {
                var (x, y) = (pattern.x + cellPosition.x, pattern.y + cellPosition.y);

                if (x < 0 || y < 0) continue;
                if (x >= _cellsMap.GetLength(0) || y >= _cellsMap.GetLength(1)) continue;

                result.Add(_cellsMap[x, y]);
            }

            return result;
        }
    }
}