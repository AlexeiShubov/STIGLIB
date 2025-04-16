namespace PathFinder
{
    public interface IPathFinder<T, TP>
    {
        INodePattern<T, TP> NodePattern { get; }
    }
}