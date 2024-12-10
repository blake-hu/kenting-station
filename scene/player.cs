using Godot;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite2D;

	[Export] public float Speed = 100.0f;

	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Move();
		AnimateMovement();
	}

	private void AnimateMovement()
	{
		if (Input.IsActionPressed("left"))
			_animatedSprite2D.Play("left");
		else if (Input.IsActionPressed("right"))
			_animatedSprite2D.Play("right");
		else if (Input.IsActionPressed("up"))
			_animatedSprite2D.Play("up");
		else if (Input.IsActionPressed("down"))
			_animatedSprite2D.Play("down");
		else
			_animatedSprite2D.Play("stationary");
	}

	private void Move()
	{
		Velocity = Input.GetVector("left", "right", "up", "down") * Speed;
		MoveAndSlide();
	}
}
