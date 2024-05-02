using System;
using System.Collections.Generic;

public class MemoryContainer
{
    Dictionary<string, Memory> dict = new Dictionary<string, Memory>();

    public MemoryContainer() { }

    public void Store(string identifier, Memory memory)
    {
        if (!Exists(identifier) && !Exists(memory))
        {
            dict.Add(identifier, memory);
        }
    }

    public Memory Retrieve(string identifier)
    {
        return dict.GetValueOrDefault(identifier);
    }

    public bool Exists(string identifier)
    {
        return dict.ContainsKey(identifier);
    }

    public bool Exists(Memory memory)
    {
        return dict.ContainsValue(memory);
    }
}
