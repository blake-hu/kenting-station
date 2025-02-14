namespace Kenting.Interface;

public interface IOneAxisMover
{
    public bool NextMove(out float moveValue);
}