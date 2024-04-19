using UnityEngine;

[System.Serializable]
public class MTRANS : BaseAction
{
    public string id;
    public Memory mem;
    public MemoryContainer dest;

    public MTRANS(Colonist _doer, string _name, string _identifier, Memory _memory, MemoryContainer _target) : base(_doer, _name)
    {
        mem = _memory;
        dest = _target;
        id = _identifier;
    }

    public override void OnStart()
    {
        base.OnStart();
        dest.Store(id, mem);
        Complete();
    }
}
