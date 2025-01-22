using Godot;
using System;
using System.Globalization;
using System.Linq;

/// <summary>
/// Manages logic for the keypad puzzle and its buttons.
/// </summary>
public partial class KeypadManager : Node3D
{
	/// <summary>
    /// Lever to unlock upon solution being reached.
    /// </summary>
    [Export] private Lever lever;

    /// <summary>
    /// Individual digit displays.
    /// </summary>
    [Export] private MeshInstance3D[] digitDisplays;

    /// <summary>
    /// Keypad buttons (for locking purposes).
    /// </summary>
    private KeypadButton[] buttons;

    /// <summary>
    /// Indicates whether or not the keypad puzzle has been solved.
    /// </summary>
    private bool solved;

    /// <summary>
    /// Stores the current digits registered in the keypad.
    /// </summary>
    private int[] digits;

    /// <summary>
    /// Audio manager instance.
    /// </summary>
    private AudioManager audioManager;

    // Ready is called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioManager = GetNode<AudioManager>("/root/Main/AudioManager");

        buttons = new KeypadButton[8];
        int i = 0;
        foreach(var button in GetTree().GetNodesInGroup("Keypad Buttons"))
        {
            buttons[i] = (KeypadButton)button;
            ++i;
        }

        solved = false;
        digits = new int[4];

        for(int digitIdx = 0; digitIdx < 4; ++digitIdx)
        {
            updateDisplay(digitIdx);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!solved
            && digits[0] == 2 && digits[1] == 4 && digits[2] == 6 && digits[3] == 8)
        {
            solved = true;
            lockAllButtons();
            lever.locked = false;
            audioManager.Play("KeypadSolved", GlobalPosition);
        }
    }

    /// <summary>
    /// Locks the keypad by locking all its buttons.
    /// </summary>
    private void lockAllButtons()
    {
        foreach (Interactable button in buttons)
        {
            button.locked = true;
        }
    }

    /// <summary>
    /// Increases by 1 the digit at the specified index.
    /// </summary>
    /// <param name="digitIdx">The index of the digit being increased.</param>
    public void increaseDigit(int digitIdx)
    {
        digits[digitIdx] = digits[digitIdx] >= 8 ? 0 : digits[digitIdx] + 1;
        updateDisplay(digitIdx);
    }

    /// <summary>
    /// Decreases by 1 the digit at the specified index.
    /// </summary>
    /// <param name="digitIdx">The index of the digit being decreased.</param>
    public void decreaseDigit(int digitIdx)
    {
        digits[digitIdx] = digits[digitIdx] <= 0 ? 8 : digits[digitIdx] - 1;
        updateDisplay(digitIdx);
    }

    /// <summary>
    /// Updates the in-game display with the changed digit.
    /// </summary>
    /// <param name="digitIdx">The index of the digit being updated.</param>
    private void updateDisplay(int digitIdx)
    {
        TextMesh mesh = (TextMesh)digitDisplays[digitIdx].Mesh;
        mesh.Text = $"{digits[digitIdx]}";
    }
}