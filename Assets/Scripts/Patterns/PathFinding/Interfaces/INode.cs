namespace PathFinder
{
    public interface INode<T>
    {
        INodeData<T> NodeData { get; }
    }
}