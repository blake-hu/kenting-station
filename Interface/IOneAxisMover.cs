namespace KentingStation.Interface;

public interface IOneAxisMover
{
    public bool NextMove(out float moveValue);
}