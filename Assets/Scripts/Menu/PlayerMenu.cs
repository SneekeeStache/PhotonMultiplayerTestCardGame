using Fusion;
using UnityEngine;

public class PlayerMenu : NetworkBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        base.Spawned();
        gameObject.transform.SetParent(objectsRef.Instance.playerMenuPanel.transform);
    }
}
