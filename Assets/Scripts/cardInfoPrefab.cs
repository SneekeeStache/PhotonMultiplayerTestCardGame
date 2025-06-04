using System;
using Fusion;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class cardInfoPrefab : NetworkBehaviour
{
    // Identifiant de la carte (utilisé pour récupérer les données dans la base)
    public int indexCard;

    // Indique si la carte est posée sur le terrain (true) ou non (false)
    public bool OnTerrain = false;

    // Référence à l’image UI qui affiche le visuel de la carte
    public Image cardImage;

    // Référence à la base de données contenant toutes les cartes (ScriptableObject)
    public CardDataBase cardData;

    // Référence réseau du propriétaire de la carte (le joueur qui la possède)
    [Networked]
    public PlayerRef owner { get; set; }
    
    // Méthode appelée lorsque l’objet réseau est spawné dans la scène
    public override void Spawned()
    {
        Debug.Log(owner); // Affiche dans la console le PlayerRef du propriétaire (debug)

        // Récupère la base de données des cartes depuis le singleton global
        cardData = GameRef.Instance.DataBase;

        // Vérifie que la base est bien chargée
        if (!cardData)
        {
            Debug.Log("bug cardData");
        }

        // Définit le sprite de la carte en fonction de l’index dans la base de données
        cardImage.sprite = cardData.GetCardById(indexCard).visual;

        // Tente de récupérer le joueur correspondant au propriétaire (owner) via le dictionnaire global
        if (GameRef.Instance.AllPlayers.TryGetValue(owner, out var player))
        {
            // Si trouvé, attache la carte dans la hiérarchie de la main du joueur (handUI)
            gameObject.transform.SetParent(player.handUI.transform);
        }
    }

    // Fonction appelée quand on clique sur la carte (test)
    public void testClick()
    {
        // Si la carte est déjà sur le terrain, on ne fait rien
        if (OnTerrain) return;

        // Sinon on appelle la fonction réseau pour déplacer la carte sur le terrain
        RPC_moveCard();
    }
    
    // RPC appelé par le propriétaire (InputAuthority) et reçu par tous (tout le monde synchronise)
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    void RPC_moveCard()
    {
        // Change le parent de la carte pour la déplacer dans la hiérarchie sous l’objet terrain (visible par tous)
        gameObject.transform.SetParent(GameRef.Instance.terrain.transform);

        // Marque la carte comme étant sur le terrain
        OnTerrain = true;
    }
}