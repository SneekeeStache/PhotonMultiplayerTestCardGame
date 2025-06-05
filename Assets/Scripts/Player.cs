using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{
    // Liste réseau des cartes en main, capacité max 30 cartes
    [Networked, Capacity(30)]
    public NetworkLinkedList<int> hand { get; }

    // Liste réseau du deck (pioche), capacité max 30 cartes
    [Networked, Capacity(30)]
    public NetworkLinkedList<int> deck { get; }

    // Canvas UI du joueur (interface visible pour le joueur local)
    [SerializeField] private Canvas playerUi;
    
    // Référence à la base de données des cartes (ScriptableObject)
    private CardDataBase DataBase;

    // Objet parent pour afficher les cartes en main (dans l'UI)
    [SerializeField] public GameObject handUI;

    // Prefab de la carte à instancier
    [SerializeField] public GameObject cardTemplate;

    public CardDeck CardDeck; // Référence au deck de cartes du joueur (ScriptableObject) si utilisé et qu'on veutlui filer un deck spécifique et préconstruit

    // Méthode appelée une fois que l’objet réseau est spawné
    public override void Spawned()
    {
        // Ajoute ce joueur dans le dictionnaire global des joueurs (référence par PlayerRef)
        GameRef.Instance.AllPlayers[Object.InputAuthority] = this;

        // Récupère la base de données des cartes depuis le singleton GameRef
        DataBase = GameRef.Instance.DataBase;

        // Si ce joueur a l’autorité d'entrée (c’est le joueur local sur ce client)
        if (HasInputAuthority)
        {
            print("test autority"); // Debug pour vérifier l’autorité

            // Définit ce joueur comme le joueur local dans GameRef
            GameRef.Instance.LocalPlayer = this;

            if (CardDeck != null)
            {
                InitializeDeck(CardDeck); // Initialise le deck de cartes du joueur avec le ScriptableObject CardDeck
            }

            // Tire 2 cartes au début (appel local)
            DrawCard();
            DrawCard();
        }
        else
        {
            // Désactive l’UI du joueur si ce n’est pas le joueur local (pour ne pas afficher les UI des autres)
            playerUi.enabled = false;
        }
    }

    // Fonction pour tirer une carte (appel local sur le joueur local)
    public void DrawCard()
    {
        // Appelle un RPC vers le serveur pour déplacer une carte dans la main
        RPC_MoveCardToHand();
    }

    // RPC appelé par le joueur local vers le StateAuthority (serveur) pour déplacer une carte dans la main
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_MoveCardToHand()
    {
        // Choisit un index aléatoire dans le deck (nombre de cartes restantes)
        int cardId = Random.Range(0, deck.Count);

        // Instancie (spawn) un nouvel objet carte dans la scène réseau, avec InputAuthority sur ce joueur
        NetworkObject netCard = Runner.Spawn(
            cardTemplate,                  // Prefab à instancier
            Vector3.zero,                 // Position de spawn (à modifier si besoin)
            Quaternion.identity,          // Rotation par défaut
            Object.InputAuthority,        // Autorité d'entrée sur la carte (le joueur qui la possède)
            (_, obj) =>                   // Callback après spawn
            {
                // Récupère le script de gestion de la carte
                var cardInfo = obj.GetComponent<cardInfoPrefab>();

                // Assigne l’ID de la carte à afficher
                cardInfo.indexCard = deck[cardId];

                // Assigne le propriétaire de la carte (pour gérer la main, interaction, etc.)
                cardInfo.owner = this.Object.InputAuthority;
                cardInfo.leButton = obj.GetComponent<Button>();
                cardInfo.leButton.onClick.AddListener(delegate
                {
                    pickCard(obj.gameObject);
                    
                });
            });

        
        // Ajoute la carte dans la main réseau
        hand.Add(deck[cardId]);

        // Supprime la carte du deck réseau (elle est maintenant dans la main)
        deck.Remove(deck[cardId]);
    }

    public void pickCard(GameObject card)
    {
        GameRef.Instance.SelectManager.selectCard(card);
    }

    public void InitializeDeck(CardDeck cardDeck)
    {
        if (cardDeck != null)
        {
            deck.Clear(); // Vide le deck actuel

            foreach (cardData card in cardDeck.DeckCards)
            {
                deck.Add(card.id);
            }
        }
        else
        {
            Debug.LogWarning("CardDeck is null. Cannot initialize CardDeckHandler.");
        }
    }
}
