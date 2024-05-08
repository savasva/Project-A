
using Sirenix.OdinInspector;

public interface ISuperLoggable
{
    public bool CanDebug
    {
        get;
    }

    public string Prefix { get; }
}