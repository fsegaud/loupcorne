namespace LoupCorne.Framework
{
    public interface IDatatable
    {
    }

    public interface IDatatable<out T> : IDatatable
    {
        T GetElement(string name);
        T[] GetElements();
    }
}