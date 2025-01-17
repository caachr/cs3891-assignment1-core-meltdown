using Godot;
using System;

public partial class Player : RigidBody3D
{
	// Movement logic variables
	private float movementSpeed = 5.0f;
	private float frictionFactor = 60.0f;

	// Jump logic variables
	private bool isGrounded = false;
	private float jumpForce = 45.0f;
	private RayCast3D groundCheck;

	// Camera system variables
	private float mouse_sensitivity = 0.0006f;
	private float twistInput = 0.0f;
	private float pitchInput = 0.0f;
	private Node3D twistPivot;
	private Node3D pitchPivot;

	// Interaction logic variables
	private RayCast3D interactionRay;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		groundCheck = GetNode<RayCast3D>("GroundCheck");
		interactionRay = GetNode<RayCast3D>("TwistPivot/PitchPivot/Camera3D/InteractionRay");

		twistPivot = GetNode<Node3D>("TwistPivot");
		pitchPivot = GetNode<Node3D>("TwistPivot/PitchPivot");

		// Capture and hide the mouse during gameplay.
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Show and free the mouse based on user input.
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		// Pitch and twist the camera based on user mouse movement.
		twistPivot.RotateY(twistInput);
        pitchPivot.RotateX(pitchInput);
		pitchPivot.Rotation = pitchPivot.Rotation with {
			X = Mathf.Clamp(
			pitchPivot.Rotation.X,
			Mathf.DegToRad(-60),
			Mathf.DegToRad(60))
		};

		// Reset camera system variables for next frame.
		twistInput = 0;
		pitchInput = 0;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Generate movement right, left, back, or forward based on user input.
		// See project Input Map settings for keyboard mappings.
		Vector3 input = Vector3.Zero;
		input.X = Input.GetAxis("move_left", "move_right");
        input.Z = Input.GetAxis("move_forward", "move_back");

		ApplyCentralForce(twistPivot.Basis * input * 50000 * (float)delta);

		// Limit the max speed for smooth movement.
		Vector3 horizontalVel = LinearVelocity with { Y = 0 };
		if (horizontalVel.Length() > movementSpeed)
		{
			Vector3 maxedVel = horizontalVel.Normalized() * movementSpeed;
			LinearVelocity = new Vector3(maxedVel.X, LinearVelocity.Y, maxedVel.Z);
		}

		// Check if Player is on the ground.
		isGrounded = groundCheck.IsColliding();

		// Generate friction force if player is grounded.
		if (isGrounded)
		{
			// Get horizontal velocity (ignore vertical movement)
			Vector3 horizontalVelocity = LinearVelocity with { Y = 0 };
			
			// Apply friction force opposite to movement direction
			Vector3 frictionForce = -horizontalVelocity * frictionFactor;
			ApplyCentralForce(frictionForce);
		}

		// Generate jumping motion (when on ground) based on user input.
		if (Input.IsActionJustPressed("jump") && isGrounded)
		{
			LinearVelocity = LinearVelocity with { Y = 0 };
			ApplyCentralImpulse(Vector3.Up * jumpForce);
			isGrounded = false;
		}
	}

	public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("interact"))
        {
			GD.Print("Mouse clicked!");
			if (interactionRay.IsColliding())
            {
				GD.Print("Collision detected!");
                var collider = interactionRay.GetCollider();
				GD.Print("Collided with: ", ((Node3D)collider).Name);
                if (collider is StaticBody3D body && body.Name == "1levmountbody")
                {
					GD.Print("Interacting with lever!");
					GetNode<LeverController>("/root/Main/World/Level 1/1_lev_mount").Toggle();
                }
            }
        }
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
