using Godot;
using KentingStation.Common.Util;
using KentingStation.Entity.Instance;

public partial class HealthBar : ProgressBar
{
	private const int ResetDelay = 2;
	private readonly RandomDelay _resetDelayTimer = new(ResetDelay);
	private int _baseHealth;
	private Player _player;
	private bool _resetInProgress;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_baseHealth = (int)Value;
		ValueChanged += OnValueChanged;
	}

	public void ResetHealth()
	{
		_resetInProgress = true;
	}

	private void OnValueChanged(double value)
	{
		if (value <= 0) _player.PlayerDie();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_resetInProgress && _resetDelayTimer.Done())
		{
			Value = _baseHealth;
			_resetInProgress = false;
		}
	}

	public void RegisterPlayer(Player player)
	{
		_player = player;
	}
}
