using Godot;
using System;

public partial class Interactable : Node3D
{
    // Audio player instance
    protected AudioManager audioManager;

    // Control whether or not this object can currently be interacted with.
    public bool locked;

    public override void _Ready()
    {
        // NOTE: If the Interactable script itself is not in the scene tree, this code has no effect!
        // The _Ready() function is only called if the node itself is in the scene tree, NOT its derived classes!
        audioManager = GetNode<AudioManager>("/root/Main/AudioManager");
        locked = false;
    }

    protected void PlayLockedSFX()
    {
        audioManager.PlayLockedSFX(GlobalPosition);
    }
}