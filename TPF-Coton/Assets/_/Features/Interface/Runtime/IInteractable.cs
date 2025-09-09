namespace Interface.Runtime
{
    public interface IInteractable
    {
        void Interact();
        string[] InteractionLabel { get; }
    }
}
