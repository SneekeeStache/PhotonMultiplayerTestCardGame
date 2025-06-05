using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCardColorOnSelected : NetworkBehaviour
{
    
    public Image  cardImageComponent;
    
    [Networked]
    public Color cardColorDefault {get; set;}
    [Networked]
    public Color cardColorSelected {get; set;}
    

    public void ChangeSelectedCardColor()
    {
        cardImageComponent.color = cardColorSelected;
    }

    public void ChangeUnselectedCardColor()
    {
        cardImageComponent.color = cardColorDefault;
    }
    
}
