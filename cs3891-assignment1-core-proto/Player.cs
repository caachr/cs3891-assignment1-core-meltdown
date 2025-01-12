using Godot;
using System;

public partial class Player : RigidBody3D
{
	private float mouse_sensitivity = 0.0006f;
	private float twistInput = 0.0f;
	private float pitchInput = 0.0f;

	private Node3D twistPivot;
	private Node3D pitchPivot;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		twistPivot = GetNode<Node3D>("TwistPivot");
		pitchPivot = GetNode<Node3D>("TwistPivot/PitchPivot");

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector3 input = Vector3.Zero;
		input.X = Input.GetAxis("move_left", "move_right");
        input.Z = Input.GetAxis("move_forward", "move_back");

		ApplyCentralForce(twistPivot.Basis * input * 1200 * (float)delta);

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		twistPivot.RotateY(twistInput);
        pitchPivot.RotateX(pitchInput);
		pitchPivot.Rotation = pitchPivot.Rotation with {
			X = Mathf.Clamp(
			pitchPivot.Rotation.X,
			Mathf.DegToRad(-30),
			Mathf.DegToRad(30))
		};

		twistInput = 0;
		pitchInput = 0;
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
		{
			if (Input.MouseMode == Input.MouseModeEnum.Captured)
			{
				twistInput = -mouseMotion.Relative.X * mouse_sensitivity;
				pitchInput = -mouseMotion.Relative.Y * mouse_sensitivity;
			}
		}
    }
}
