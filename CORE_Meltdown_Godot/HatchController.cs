using Godot;
using System;

public partial class HatchController : Node3D
{
	private Node3D _hatch;

	public override void _Ready()
	{
		_hatch = this;
	}

	public void OpenHatch()
	{
		var tween = CreateTween();
		tween.TweenProperty(_hatch, "rotation", new Vector3(0, 0, Mathf.DegToRad(120)), 6.0f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);
	}
}