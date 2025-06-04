using System.Collections.Generic;
using Fusion;
using UnityEngine;

// Classe singleton qui sert de référence globale au jeu
public class GameRef : Singleton<GameRef>
{
    // Référence à la base de données des cartes (ScriptableObject)
    public CardDataBase DataBase;

    // Référence au GameObject représentant le terrain de jeu dans la scène

    // Référence au joueur local (celui qui contrôle ce client)
    public Player LocalPlayer;
    
    public SelectManager SelectManager;

    // Dictionnaire qui associe chaque PlayerRef (référence réseau d’un joueur) à son script Player
    // Permet d’accéder facilement aux joueurs connectés
    public Dictionary<PlayerRef, Player> AllPlayers = new();
}