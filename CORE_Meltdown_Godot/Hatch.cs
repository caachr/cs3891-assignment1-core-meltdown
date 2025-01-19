using Godot;
using System;

public partial class Hatch : Node3D
{
	// This hatch node.
	private Node3D hatch;

    // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hatch = this;
	}

    // Opens the hatch.
	public void Open()
	{
		var tween = CreateTween();
		tween.TweenProperty(hatch, "rotation", new Vector3(0, 0, Mathf.DegToRad(120)), 6.0f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
	}
}