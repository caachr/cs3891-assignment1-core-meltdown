using Godot;
using System;
using System.Security.Cryptography;

public partial class FloorManager : Node
{
    // The floor number for this instance.
    [Export] private int floor;

    // Game manager singleton.
    [Export] private GameManager gameManager;

    // Hatch manager for this floor.
    [Export] private Hatch hatch;

    // Levers on this floor.
    [Export] private Lever[] levers;

    // Keypad for floor 5.
    [Export] private KeypadManager keypad;

    // Array for determining which floors have been completed. Used mainly for swtich statement proper functioning.
    private static bool[] floorComplete;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        floorComplete = new bool[5];
        if (floor == 5)
        {
            levers[0].locked = true;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        switch (floor)
        {
            // Floor 1 puzzle: lever must be activated for the hatch to open.
            case 1:
                if (!floorComplete[0]
                    && levers[0].activated) {
                    // Lock lever and open hatch.
                    levers[0].locked = true;
                    hatch.Open();
                    floorComplete[0] = true;
                }
                break;

            // Floor 2 puzzle: all levers must be activated for the hatch to open.
            case 2:
                if (!floorComplete[1]
                    && levers[0].activated && levers[1].activated && levers[2].activated && levers[3].activated) {
                    // Lock all levers and open hatch.
                    foreach (var lever in levers) {
                        lever.locked = true;
                    }
                    hatch.Open();
                    floorComplete[1] = true;
                }
                break;

            // Floor 3 puzzle: lever 4, and only lever 4, must be activated for the hatch to open.
            case 3:
                if (!floorComplete[2]
                    && levers[3].activated
                    && !levers[0].activated && !levers[1].activated && !levers[2].activated) {
                    foreach(var lever in levers) {
                        lever.locked = true;
                    }
                    hatch.Open();
                    floorComplete[2] = true;
                }
                break;

            // Floor 4 puzzle: levers 2, 4, 6, 8, and only these levers, must be activated for the hatch to open.
            case 4:
                if (!floorComplete[3]
                    && levers[1].activated && levers[3].activated && levers[5].activated && levers[7].activated
                    && !levers[0].activated && !levers[2].activated && !levers[4].activated && !levers[6].activated) {
                    foreach(var lever in levers) {
                        lever.locked = true;
                    }
                    hatch.Open();
                    floorComplete[3] = true;
                }
                break;

            // Floor 5 puzzle: lever must be unlocked by solving a keypad puzzle, then activated to beat the game.
            case 5:
                if (!floorComplete[4]
                    && levers[0].activated) {
                    levers[0].locked = true;

                    floorComplete[4] = true;
                    gameManager.gameWin = true;
                }
                break;
        }
    }
}