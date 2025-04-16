namespace PathFinder
{
    public class NodeData2D : INodeData<bool>
    {
        public bool InteractableType { get; set; }
        public int Level { get; }

        public NodeData2D(bool interactableType, int level)
        {
            InteractableType = interactableType;
            Level = level;
        }

        public override string ToString()
        {
            return $"InteractableType {InteractableType}, Level: {Level}";
        }
    }
}