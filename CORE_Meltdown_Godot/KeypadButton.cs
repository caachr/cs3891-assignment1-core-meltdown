using Godot;
using System;

/// <summary>
/// Manages interactable buttons that are a part of some specified keypad.
/// </summary>
public partial class KeypadButton : Interactable
{
    /// <summary>
    /// The KeypadManager instance.
    /// </summary>
    [Export] private KeypadManager keypad;

    /// <summary>
    /// The digit to which this keypad button is assigned.
    /// </summary>
    [Export] private int digitIndex;

    /// <summary>
    /// Indicates whether this is an 'up' button or a 'down' button.
    /// </summary>
    [Export] private bool isUpButton;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        locked = false;
        audioManager = GetNode<AudioManager>("/root/Main/AudioManager");
    }

    /// <summary>
    /// Called when the button is pressed. Increases the corresponding digit if up button, otherwise decreases the digit.
    /// </summary>
    public void Press()
    {
        if (!locked)
        {
            audioManager.Play("Keypad", GlobalPosition);
            if (isUpButton) 
            {
                keypad.increaseDigit(digitIndex);
            }
            else {
                keypad.decreaseDigit(digitIndex);
            }
        }
        else {
            PlayLockedSFX();
        }
    }
}