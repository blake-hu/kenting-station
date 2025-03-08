using Godot;
using KentingStation.Entity.Instance;

public partial class HealthBar : ProgressBar
{
	private Player _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ValueChanged += OnValueChanged;
	}

	private void OnValueChanged(double value)
	{
		if (value <= 0)
			_player.PlayerDie();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RegisterPlayer(Player player)
	{
		_player = player;
	}
}
