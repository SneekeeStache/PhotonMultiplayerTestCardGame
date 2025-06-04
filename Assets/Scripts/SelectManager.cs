using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class SelectManager : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject cardSelected;

    public void selectCard(GameObject card)
    {
        if (cardSelected != card)
        {
            cardSelected = card;
        }
        else
        {
            cardSelected = null;
        }
    }

    public void selectTerrain(GameObject terrain)
    {
        if(cardSelected == null) return;
        NetworkObject monTerrain = terrain.GetComponent<NetworkObject>();
        NetworkObject maCarte = cardSelected.GetComponent<NetworkObject>();
        RPC_dropCard(monTerrain.Id,  maCarte.Id);
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_dropCard(NetworkId terrainId,NetworkId cardId)
    {
        Runner.TryFindObject(cardId,out NetworkObject myCard);
        Runner.TryFindObject(terrainId, out NetworkObject terrain);
        myCard.gameObject.transform.SetParent(terrain.gameObject.transform);
    }
}
