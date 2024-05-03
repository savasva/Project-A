using UnityEngine;

public static class ActionHelpers
{
    /// <summary>
    /// Gives the distance between a colonist and this object.
    /// </summary>
    /// <param name="state">The state of the Colonist you want to take the proximity to.</param>
    /// <returns>The distance between the Colonist and this Interactable</returns>
    public static float Proximity(ColonistState state, IInteractable target)
    {
        return Vector3.Distance(state.position, target.GetDestination());
    }

    /// <summary>
    /// Gives the distance between a colonist and this object.
    /// </summary>
    /// <param name="state">The state of the Colonist you want to take the proximity to.</param>
    /// <returns>The distance between the Colonist and this Interactable</returns>
    public static float Proximity(ColonistState state, WorldObjState target)
    {
        return Vector3.Distance(state.position, target.position);
    }
}