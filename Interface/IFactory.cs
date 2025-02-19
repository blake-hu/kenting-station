namespace Kenting.Interface;

public interface IFactory<out T>
{
    public T GetInstance();
}