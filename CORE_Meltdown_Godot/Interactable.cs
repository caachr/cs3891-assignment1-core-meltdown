using Godot;
using System;

public partial class Interactable : Node3D
{
    // Control whether or not this object can currently be interacted with.
    public bool locked;

    public override void _Ready()
    {
        locked = false;
    }
}