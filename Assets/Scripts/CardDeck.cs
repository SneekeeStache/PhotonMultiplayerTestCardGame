using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayingDecks", menuName = "ScriptableObjects/PlayingDecks")]
public class CardDeck : ScriptableObject {
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private List<cardData> _deckCards;

    public int Id => _id;
    public string Name => _name;
    public string Description => _description;
    public List<cardData> DeckCards => _deckCards;
}