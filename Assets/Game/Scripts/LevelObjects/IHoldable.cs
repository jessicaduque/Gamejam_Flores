public interface IHoldable 
{
    public string holdableTypeName { get; }
    public bool CanHold();
}
