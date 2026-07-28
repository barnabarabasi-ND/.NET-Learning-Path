namespace Variant.Interfaces;
public interface IProducer<out T>
{
    T GetItem();
}