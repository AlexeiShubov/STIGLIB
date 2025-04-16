namespace PathFinder
{
    public interface INodeData<T>
    {
        T InteractableType { get; set; }
        int Level { get; }
    }
}