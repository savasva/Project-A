using System;
public class RequestMemory : Memory
{
    BaseAction requested;
    bool fulfilled;
    public RequestMemory(BaseAction _requested, bool _fulfilled = false)
    {
        requested = _requested;
        fulfilled = _fulfilled;
    }

    public override void Modify()
    {
        fulfilled = !fulfilled;
    }
}
