using Fusion;
using UnityEngine;

// Classe qui gère le spawn des joueurs dans la simulation Fusion
public class Spawner : SimulationBehaviour, IPlayerJoined
{
    // Référence au prefab du joueur à instancier
    public GameObject PlayerPrefab;

    // Méthode appelée automatiquement par Fusion lorsqu'un joueur rejoint la partie
    public void PlayerJoined(PlayerRef player)
    {
        // On vérifie si le joueur qui vient de rejoindre est le joueur local sur ce client
        if (player == Runner.LocalPlayer)
        {
            // On spawn (instancie) le prefab du joueur à la position (0,0,0) avec une rotation neutre,
            // et on assigne l’autorité d’entrée à ce joueur (input authority)
            Debug.Log("PlayerJoined");
            Runner.Spawn(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity, player);
        }
    }
}