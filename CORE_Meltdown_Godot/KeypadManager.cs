using Godot;
using System;
using System.Globalization;
using System.Linq;

public partial class KeypadManager : Node3D
{
	// Lever to unlock upon solution being reached.
    [Export] private Lever lever;

    // Individual digit displays.
    [Export] private MeshInstance3D[] digitDisplays;

    // Keypad buttons (for locking purposes).
    private KeypadButton[] buttons;

    // Indicates whether or not the keypad puzzle has been solved.
    private bool solved;

    // Stores the current digits registered in the keypad.
    private int[] digits;

    // Ready is called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
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
        }
    }

    // Locks the keypad by locking all its buttons.
    private void lockAllButtons()
    {
        foreach (Interactable button in buttons)
        {
            button.locked = true;
        }
    }

    // Increases by 1 the digit at the specified index.
    public void increaseDigit(int digitIdx)
    {
        digits[digitIdx] = digits[digitIdx] >= 8 ? 0 : digits[digitIdx] + 1;
        updateDisplay(digitIdx);
    }

    // Decreases by 1 the digit at the specified index.
    public void decreaseDigit(int digitIdx)
    {
        digits[digitIdx] = digits[digitIdx] <= 0 ? 8 : digits[digitIdx] - 1;
        updateDisplay(digitIdx);
    }

    // Updates the in-game display with the changed digit.
    private void updateDisplay(int digitIdx)
    {
        TextMesh mesh = (TextMesh)digitDisplays[digitIdx].Mesh;
        mesh.Text = $"{digits[digitIdx]}";
    }
}