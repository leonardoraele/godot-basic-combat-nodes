#nullable enable
using Godot;

namespace Raele.BasicCombatNodes;

[Tool]
public partial class HitBox3D : Area3D
{
	// -----------------------------------------------------------------------------------------------------------------
	// STATICS
	// -----------------------------------------------------------------------------------------------------------------

	// public static readonly string MyConstant = "";

	// -----------------------------------------------------------------------------------------------------------------
	// EXPORTS
	// -----------------------------------------------------------------------------------------------------------------

	[Export] public AttackData? AttackData;
	[Export] public HitBoxModeEnum Mode = HitBoxModeEnum.Constant;

	// -----------------------------------------------------------------------------------------------------------------
	// FIELDS
	// -----------------------------------------------------------------------------------------------------------------

	// public

	// -----------------------------------------------------------------------------------------------------------------
	// PROPERTIES
	// -----------------------------------------------------------------------------------------------------------------

	private bool Active => this.Mode != HitBoxModeEnum.Passive;

	// -----------------------------------------------------------------------------------------------------------------
	// SIGNALS
	// -----------------------------------------------------------------------------------------------------------------

	[Signal] public delegate void HitRegisteredEventHandler(HitData3D hitData);

	// -----------------------------------------------------------------------------------------------------------------
	// INTERNAL TYPES
	// -----------------------------------------------------------------------------------------------------------------

	public enum HitBoxModeEnum {
		/// <summary>
		/// A constant hit box applies damage to hurt boxes touching it every frame.
		/// </summary>
		Constant = 1,
		/// <summary>
		/// A passive hit box never applies damage to hurt boxes.
		/// </summary>
		Passive = 0,
		/// <summary>
		/// An immediate hit box applies damage to hurt boxes touching it only once, in the next physics frame, then it
		/// reverts back to Passive mode.
		/// </summary>
		Immediate = 2,
	}

	// -----------------------------------------------------------------------------------------------------------------
	// EVENTS
	// -----------------------------------------------------------------------------------------------------------------

	public override void _EnterTree()
	{
		base._EnterTree();
		this.Monitoring = true;
		this.Monitorable = false;
		this.CollisionLayer = 0;
	}

	// public override void _Ready()
	// {
	// 	base._Ready();
	// }

    // public override void _Process(double delta)
    // {
    // 	base._Process(delta);
    // }

    public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (Engine.IsEditorHint()) {
			this.SetPhysicsProcess(false);
			return;
		}
		if (this.Mode != HitBoxModeEnum.Passive) {
			this.HitOverlappingHurtBoxes();
		}
		if (this.Mode == HitBoxModeEnum.Immediate) {
			this.Mode = HitBoxModeEnum.Passive;
		}
	}

    public override string[] _GetConfigurationWarnings()
		=> new ConfigurationWarningsBuilder()
			.Concat(base._GetConfigurationWarnings())
			.MaybeAppend(this.CollisionMask == 0, "This HitBox has no CollisionMask set. It won't hit anything.")
			.MaybeAppend(this.CollisionMask == 1, "Property CollisionMask is left at the default value. Did you forget to set it?")
			.MaybeAppend(this.AttackData == null, "This HitBox has no AttackData set. It won't apply any damage.")
			.Build();

    // -----------------------------------------------------------------------------------------------------------------
    // METHODS
    // -----------------------------------------------------------------------------------------------------------------

    public void ActivateImmediate()
	{
		if (!this.Active) {
			this.Mode = HitBoxModeEnum.Immediate;
		}
	}

	private void HitOverlappingHurtBoxes()
	{
		if (this.AttackData == null || !this.Active) {
			return;
		}
		foreach (Area3D? area in this.GetOverlappingAreas()) {
			if (area is HurtBox3D hurtBox) {
                HitData3D hitData = new() {
					// HitBox = this,
					HurtBox = hurtBox,
					AttackData = this.AttackData,
				};
				hurtBox.TryApplyHit(hitData);
				this.EmitSignal(SignalName.HitRegistered, hitData);
			}
		}
	}
}
