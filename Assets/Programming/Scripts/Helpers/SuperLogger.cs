using UnityEngine;

public static class SuperLogger
{
    public static void Log(ISuperLoggable source, string message, params string[] args)
    {
        string output = string.Format(message, args);

        Debug.LogFormat("{0}: {1}", source.Prefix, output);
        //Debug.LogFormat(logType: LogType.Log, LogOption.None, source, "{0}: {1}", new object[] { source.Prefix, output })
    }
}