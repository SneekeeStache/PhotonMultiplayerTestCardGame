using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class SelectManager : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public cardInfoPrefab cardSelected;

    public void selectCard(cardInfoPrefab card)
    {
        Debug.Log("click card");
        if (cardSelected != card)
        {
            if (cardSelected)
            {
                cardSelected.changeState(cardInfoPrefab.state.InHand);
            }
            cardSelected = card;
            Debug.Log("selected");
            card.changeState(cardInfoPrefab.state.Picked);
        }
        else
        {
            cardSelected.changeState(cardInfoPrefab.state.InHand);
            cardSelected = null;
            Debug.Log("unselected");
        }
    }

    public void selectTerrain(GameObject terrain)
    {
        Debug.Log("click terrain");
        if(cardSelected == null) return;
        NetworkObject monTerrain = terrain.GetComponent<NetworkObject>();
        NetworkObject maCarte = cardSelected.networkObjectComponent;
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
