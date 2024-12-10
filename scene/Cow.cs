using CozyGame.scene;
using Godot;
using Vector2 = Godot.Vector2;

public partial class Cow : CharacterBody2D
{
	private AnimatedSprite2D _animatedSprite2D;
	private RandomMover _randomHoriMover;
	private RandomMover _randomVertMover;

	[Export] public int MaxMoveDuration = 100;
	[Export] public float MaxSpeedX = 100.0f;
	[Export] public float MaxSpeedY = 50.0f;
	[Export] public int MinMoveDuration = 20;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_randomHoriMover = new RandomMover(MinMoveDuration, MaxMoveDuration, -MaxSpeedX, MaxSpeedX);
		_randomVertMover = new RandomMover(MinMoveDuration, MaxMoveDuration, -MaxSpeedY, MaxSpeedY);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_randomHoriMover.NextMove(out var horiMove))
			MoveHorizontal(horiMove);
		if (_randomVertMover.NextMove(out var vertMove))
			MoveVertical(vertMove);
		MoveAndSlide();
	}

	private void MoveHorizontal(float speed)
	{
		Velocity = speed * Vector2.Right;
		_animatedSprite2D.Scale = new Vector2(speed > 0f ? 1f : -1f, 1f);
		_animatedSprite2D.Play("right");
	}

	private void MoveVertical(float speed)
	{
		Velocity = new Vector2(Velocity.X, speed);
	}
}
