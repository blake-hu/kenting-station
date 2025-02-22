namespace KentingStation.Interface;

// Represents in-game characters which can be killed, including friendly and hostile mobs
public interface IKillableEntity
{
    public void Die();
}