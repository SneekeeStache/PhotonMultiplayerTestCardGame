using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/CardsDataBase", fileName = "cardDataBase")]
public class CardDataBase : ScriptableObject
{
    public List<cardData> cards = new List<cardData>();
    
    public cardData GetCardById(int id)
    {
        return cards.Find(card => card.id == id);
    } 
}
