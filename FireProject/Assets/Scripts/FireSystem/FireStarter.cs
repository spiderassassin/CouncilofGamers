using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lasts for some duration (finite or infinite) of time in the world. 
/// <para>Causes nearby IFlammables to catch fire (and the absense of these will allow extinguishing).</para>
/// <para>Allows additive effects.</para>
/// </summary>
public abstract class FireStarter
{
    protected LayerMask mask;

    public Collider[] detections;
    protected HashSet<Collider> oldDetections;
    protected HashSet<Collider> newDetections;

    public FireStarter(LayerMask mask)
    {
        this.mask = mask;
        newDetections = new HashSet<Collider>();
        oldDetections = new HashSet<Collider>();
    }

    protected void UpdateState()
    {
        newDetections.Clear();
        for (int i = 0; i < detections.Length; ++i)
        {

            if (oldDetections.Contains(detections[i])) continue;
            newDetections.Add(detections[i]);
        }
        oldDetections.Clear();
    }

    public bool IsNew(Collider c)
    {
        return newDetections.Contains(c);
    }
}
public class SphericalFireStarter : FireStarter
{
    public SphericalFireStarter(LayerMask mask) : base(mask)
    {
    }

    public void Update(Vector3 position, float radius)
    {
        detections = new Collider[0];
        detections = Physics.OverlapSphere(position, radius, mask, QueryTriggerInteraction.Ignore);
        UpdateState();
    }
}
/*public class ConicalFireStarter : FireStarter
{

}*/
