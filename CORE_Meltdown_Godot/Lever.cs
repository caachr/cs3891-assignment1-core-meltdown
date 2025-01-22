using Godot;
using System;

/// <summary>
/// Manages interactable lever objects.
/// </summary>
public partial class Lever : Interactable
{
	/// <summary>
	/// Lever state (on or off). In-game visual indicator: up position is off, down position is on.
	/// </summary>
	public bool activated;

	/// <summary>
	/// Lever handle mesh (for animation purposes).
	/// </summary>
	[Export] private Node3D handleMesh;

	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        locked = false;
		activated = false;
		audioManager = GetNode<AudioManager>("/root/Main/AudioManager");
    }

    /// <summary>
	/// Called when the lever is toggled on or off.
	/// </summary>
    public void Toggle()
	{
		if (!locked)
		{
			// If the lever is currently activated (down position), it will now go up, so play the lever up sfx.
			// Otherwise, play the lever down sfx.
			if (activated)
			{
				audioManager.Play("LeverUp", GlobalPosition);
			}
			else {
				audioManager.Play("LeverDown", GlobalPosition);
			}

			// Set activation status.
			activated = !activated;

			// Play lever animation.
			var tween = CreateTween();
			tween.TweenProperty(handleMesh, "rotation", new Vector3(0, activated ? Mathf.DegToRad(-60) : Mathf.DegToRad(60), 0), 0.5f);
		}
		else {
			PlayLockedSFX();
		}
	}
}
