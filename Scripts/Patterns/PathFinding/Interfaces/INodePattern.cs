using System.Collections.Generic;

namespace PathFinder
{
    public interface INodePattern<T, TP>
    {
        IReadOnlyList<T> Patterns { get; }

        HashSet<TP> GetNeighbours(T nodePosition);
        Dictionary<TP, HashSet<TP>> GenerateNeighboursMap();
    }
}