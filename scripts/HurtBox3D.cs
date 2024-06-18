#nullable enable
using Godot;
using Raele.GodotUtil;

namespace Raele.BasicCombatNodes;

[Tool]
public partial class HurtBox3D : Area3D
{
	// -----------------------------------------------------------------------------------------------------------------
	// STATICS
	// -----------------------------------------------------------------------------------------------------------------

	// public static readonly string MyConstant = "";

	// -----------------------------------------------------------------------------------------------------------------
	// EXPORTS
	// -----------------------------------------------------------------------------------------------------------------

	[Export] public bool Enabled = true;

	[ExportGroup("Invincibility After Hit")]
	[Export] public double InvincibilityDurationOnHitSec = 0;

	[ExportGroup("Tags")]
	[Export] public string[]? Tags;

	// -----------------------------------------------------------------------------------------------------------------
	// FIELDS
	// -----------------------------------------------------------------------------------------------------------------

	private Tween? InvincibilityTween;

	// -----------------------------------------------------------------------------------------------------------------
	// PROPERTIES
	// -----------------------------------------------------------------------------------------------------------------

	public bool CanTakeHit => this.Enabled && !this.InvincibilityActive;
	public bool InvincibilityActive => this.InvincibilityTween?.IsRunning() == true;
	public float RemainingInvincibilityDurationSec => this.InvincibilityActive
		? (float) (this.InvincibilityDurationOnHitSec - this.InvincibilityTween!.GetTotalElapsedTime())
		: 0;

	// -----------------------------------------------------------------------------------------------------------------
	// SIGNALS
	// -----------------------------------------------------------------------------------------------------------------

	[Signal] public delegate void HitRegisteredEventHandler(HitData3D hitData);
	[Signal] public delegate void InvincibilityBeginEventHandler();
	[Signal] public delegate void InvincibilityEndEventHandler();

	// -----------------------------------------------------------------------------------------------------------------
	// INTERNAL TYPES
	// -----------------------------------------------------------------------------------------------------------------

	// private enum Type {
	// 	Value1,
	// }

	// -----------------------------------------------------------------------------------------------------------------
	// EVENTS
	// -----------------------------------------------------------------------------------------------------------------

	public override void _EnterTree()
	{
		base._EnterTree();
		this.Monitoring = false;
		this.Monitorable = true;
		this.CollisionMask = 0;
	}

	// public override void _Ready()
	// {
	// 	base._Ready();
	// }

	// public override void _Process(double delta)
	// {
	// 	base._Process(delta);
	// }

	// public override void _PhysicsProcess(double delta)
	// {
	// 	base._PhysicsProcess(delta);
	// }

    public override string[] _GetConfigurationWarnings()
    	=> new ConfigurationWarningsBuilder()
			.Concat(base._GetConfigurationWarnings())
			.MaybeAppend(this.CollisionLayer == 0, "This HurtBox has no CollisionLayer set. It will not detect any hits.")
			.MaybeAppend(this.CollisionLayer == 1, "Property CollisionLayer is left at the default value. Did you forget to set it?")
			.Build();

	// -----------------------------------------------------------------------------------------------------------------
	// METHODS
	// -----------------------------------------------------------------------------------------------------------------

	public bool TryApplyHit(HitData3D hitData)
	{
		if (!this.CanTakeHit)
		{
			return false;
		}
		if (this.InvincibilityDurationOnHitSec > 0) {
			this.InvincibilityTween?.Kill();
			Tween localTween = this.InvincibilityTween = this.CreateTween();
			this.InvincibilityTween.TweenInterval(this.InvincibilityDurationOnHitSec);
			this.InvincibilityTween.TweenCallback(Callable.From(() => {
				if (this.InvincibilityTween == localTween) {
					this.EmitSignal(SignalName.InvincibilityEnd);
				}
			}));
		}
		this.EmitSignal(SignalName.InvincibilityBegin);
		this.EmitSignal(SignalName.HitRegistered, hitData);
		return true;
	}
}
