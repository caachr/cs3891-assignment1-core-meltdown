using Godot;
using System;

/// <summary>
/// Abstract class for objects intended to be interacted with by the user.
/// </summary>
public abstract partial class Interactable : Node3D
{
    /// <summary>
    /// Audio manager instance
    /// </summary>
    protected AudioManager audioManager;

    /// <summary>
    /// Control whether or not interaction with this object is currently prohibited.
    /// </summary>
    public bool locked;

    /// <summary>
    /// Must initialize audio manager and initial locked status.
    /// </summary>
    public abstract override void _Ready();

    /// <summary>
    /// Play the corresponding sound when interaction is attempted but currently locked.
    /// </summary>
    protected void PlayLockedSFX()
    {
        audioManager.Play("Locked", GlobalPosition);
    }
}