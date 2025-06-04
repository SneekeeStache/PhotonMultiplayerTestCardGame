using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class SelectManager : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject cardSelected;

    public void selectCard(GameObject card)
    {
        Debug.Log("click card");
        if (cardSelected != card)
        {
            cardSelected = card;
            Debug.Log("selected");
        }
        else
        {
            cardSelected = null;
            Debug.Log("unselected");
        }
    }

    public void selectTerrain(GameObject terrain)
    {
        Debug.Log("click terrain");
        if(cardSelected == null) return;
        NetworkObject monTerrain = terrain.GetComponent<NetworkObject>();
        NetworkObject maCarte = cardSelected.GetComponent<NetworkObject>();
        Debug.Log(monTerrain.Id.Raw);
        RPC_dropCard(monTerrain.Id,  maCarte.Id);
        cardSelected = null;
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_dropCard(NetworkId terrainId,NetworkId cardId)
    {
        Debug.Log("terrain: "+terrainId+"card: "+ cardId);
        
        Runner.TryFindObject(cardId,out NetworkObject myCard);
        Runner.TryFindObject(terrainId, out NetworkObject terrain);
        myCard.gameObject.transform.SetParent(terrain.gameObject.transform);
        RectTransform rectCard = myCard.GetComponent<RectTransform>();
        rectCard.anchorMin = new Vector2(0.5f, 0.5f);
        rectCard.anchorMax = new Vector2(0.5f, 0.5f);
        

    }
}
