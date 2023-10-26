using UnityEngine;
using System.Collections;

public class ContainerObject : WorldObject
{
    public int capacity;
    [SerializeField]
    private int count;

    public bool IsEmpty { get { return count <= 0; } }

    public bool HasEnough(int amount) {
        return count > amount;
    }

    public void Consume(int amount)
    {
        count -= amount;
    }
}
