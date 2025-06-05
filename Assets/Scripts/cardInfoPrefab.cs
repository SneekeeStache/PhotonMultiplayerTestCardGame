using System;
using Fusion;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class cardInfoPrefab : NetworkBehaviour
{
    // Identifiant de la carte (utilisé pour récupérer les données dans la base)
    [Header("infoCard")]
    public int indexCard;
    
    [Header("events")]
    public UnityEvent Event_OnStateChange;
    
    public UnityEvent Event_OnStateEnter_InHand;
    public UnityEvent Event_OnStateEnter_Played;
    public UnityEvent Event_OnStateEnter_OnTile;
    public UnityEvent Event_OnStateEnter_Picked;
    
    public UnityEvent Event_OnStateExit_InHand;
    public UnityEvent Event_OnStateExit_Played;
    public UnityEvent Event_OnStateExit_OnTile;
    public UnityEvent Event_OnStateExit_Picked;
    
    [Header("components")]
    public NetworkObject networkObjectComponent;
    public Button buttonComponent;

    public enum state
    {
        InHand,
        Picked,
        Played,
        OnTile,
    };

    [Networked]
    public state currentState {get; set;}

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

        changeState(state.InHand);
        
        // Définit le sprite de la carte en fonction de l’index dans la base de données
        cardImage.sprite = cardData.GetCardById(indexCard).visual;

        // Tente de récupérer le joueur correspondant au propriétaire (owner) via le dictionnaire global
        if (GameRef.Instance.AllPlayers.TryGetValue(owner, out var player))
        {
            // Si trouvé, attache la carte dans la hiérarchie de la main du joueur (handUI)
            gameObject.transform.SetParent(player.handUI.transform);
        }

        buttonComponent.onClick.AddListener(pickCard);
    }

    public void changeState(state newState)
    {
        RPC_ChangeState(newState);
    }

    [Rpc(RpcSources.All,RpcTargets.All)]
    public void RPC_ChangeState(state newState)
    {
        Event_OnStateChange.Invoke();
        exitState();
        currentState = newState;
        enterState();
    }

    public void enterState()
    {
        switch (currentState)
        {
            case state.InHand:
                if(HasInputAuthority){}
                Event_OnStateEnter_InHand.Invoke();
                Debug.Log("enterState inhand");
                break;
            case state.Played:
                Event_OnStateEnter_Played.Invoke();
                Debug.Log("enterState played");
                changeState(state.OnTile);
                break;
            case state.OnTile:
                Event_OnStateEnter_OnTile.Invoke();
                Debug.Log("enterState onTile");
                break;
            case state.Picked:
                Event_OnStateEnter_Picked.Invoke();
                Debug.Log("enterState picked");
                break;
        }
    }

    public void exitState()
    {
        switch (currentState)
        {
            case state.InHand:
                Event_OnStateExit_InHand.Invoke();
                Debug.Log("exitState inhand");
                break;
            case state.Played:
                Event_OnStateExit_Played.Invoke();
                Debug.Log("exitState played");
                break;
            case state.OnTile:
                Event_OnStateExit_OnTile.Invoke();
                Debug.Log("exitState onTile");
                break;
            case state.Picked:
                Event_OnStateExit_Picked.Invoke();
                Debug.Log("exitState picked");
                break;
        }
    }

    public void pickCard()
    {
        if(currentState == state.OnTile) return;
        GameRef.Instance.SelectManager.selectCard(this);
    }
    
}