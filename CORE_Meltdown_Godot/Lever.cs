using Godot;
using System;

public partial class Lever : Interactable
{
	// Lever state (on or off). In-game visual indicator: up position is off, down position is on.
	public bool activated;

	// Lever handle mesh (for animation purposes).
	[Export] private Node3D handleMesh;

    public override void _Ready()
    {
        activated = false;
		audioManager = GetNode<AudioManager>("/root/Main/AudioManager");
    }

    // Called when the lever is toggled on or off.
    public void Toggle()
	{
		if (!locked)
		{
			// If the lever is currently activated (down position), it will now go up, so play the lever up sfx.
			// Otherwise, play the lever down sfx.
			if (activated)
			{
				audioManager.PlayLeverUpSFX(GlobalPosition);
			}
			else {
				audioManager.PlayLeverDownSFX(GlobalPosition);
			}

			activated = !activated;
			var tween = CreateTween();
			tween.TweenProperty(handleMesh, "rotation", new Vector3(0, activated ? Mathf.DegToRad(-60) : Mathf.DegToRad(60), 0), 0.5f);
		}
		else {
			PlayLockedSFX();
		}
	}
}
