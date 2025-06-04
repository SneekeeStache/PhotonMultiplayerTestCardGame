using System.Threading.Tasks;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private NetworkRunner runner;
    [SerializeField] TMP_Text RoomNameText;
    NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
    string roomName;
    public async void HostJoinGame()
    {
        var sceneRef = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        if (RoomNameText.text.IsNullOrEmpty())
        {
            roomName = "Default-Lobby";
        }
        else
        {
            roomName = RoomNameText.text;
        }
        sceneInfo.AddSceneRef(sceneRef,LoadSceneMode.Single);
        await StartSharedGame();
    }

    private async Task StartSharedGame()
    {
        StartGameArgs gameArgs = new StartGameArgs()
        {
            Scene = sceneInfo,
            GameMode = GameMode.Shared,
            CustomLobbyName = roomName,
        };
        var result = await runner.StartGame(gameArgs);
        if (result.Ok)
        {
            Debug.Log("Game started");
        }
        else
        {
            Debug.Log($"Game failed: {result.ShutdownReason}");
        }
    }
    
}
