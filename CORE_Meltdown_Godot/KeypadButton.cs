using Godot;
using System;

public partial class KeypadButton : Interactable
{
    // The KeypadManager instance.
    [Export] private KeypadManager keypad;

    // The digit to which this keypad button is assigned.
    [Export] private int digitIndex;

    // Indicates whether this is an 'up' button or a 'down' button.
    [Export] private bool isUpButton;

    // Called when the button is pressed.
    public void Press()
    {
        if (!locked)
        {
            if (isUpButton) 
            {
                keypad.increaseDigit(digitIndex);
            }
            else {
                keypad.decreaseDigit(digitIndex);
            }
        }
    }
}