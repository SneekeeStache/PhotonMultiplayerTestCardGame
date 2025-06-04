using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Cards/CardsData", fileName = "cardData")]
public class cardData : ScriptableObject
{
    public int id;
    public string cardName;
    public Sprite visual;
    public string description;
}
