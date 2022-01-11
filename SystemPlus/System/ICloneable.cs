namespace SystemPlus
{
    /// <summary>
    /// Generic version of ICloneable
    /// </summary>
    public interface ICloneable<out T> : ICloneable where T : ICloneable<T>
    {
        new T Clone();
    }
}
