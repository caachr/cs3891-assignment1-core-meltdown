using Godot;
using System;

public partial class LeverController : Node3D
{
	private bool isActivated = false;
	private Node3D handleMesh;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		handleMesh = GetNode<MeshInstance3D>("1_lev_handle");
	}

	public void Toggle()
	{
		isActivated = true;
		var tween = CreateTween();
		tween.TweenProperty(handleMesh, "rotation", new Vector3(0, isActivated ? Mathf.DegToRad(-60) : 0, 0), 0.5f);

		GetParent().GetNode<HatchController>("1_hatch").OpenHatch();
	}
}
