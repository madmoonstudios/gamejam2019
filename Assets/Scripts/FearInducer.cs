using UnityEngine;

public class FearInducer : MonoBehaviour
{
    protected void ScareInRadius(Vector3 position, float fearRadius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(position, fearRadius, Vector3.one);

        foreach (RaycastHit hit in hits)
        {
            IFearable fearable = hit.transform.GetComponentInChildren<IFearable>();
            if (fearable != null)
            {
                fearable.Scare();
            }
        }
    }
}