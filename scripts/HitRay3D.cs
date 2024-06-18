using System;
using Godot;
using Raele.GodotUtil;

namespace Raele.BasicCombatNodes;

[Tool]
public partial class HitRay3D : RayCast3D
{
	// -----------------------------------------------------------------------------------------------------------------
	// STATICS
	// -----------------------------------------------------------------------------------------------------------------

	// public static readonly string MyConstant = "";

	// -----------------------------------------------------------------------------------------------------------------
	// EXPORTS
	// -----------------------------------------------------------------------------------------------------------------

	[Export] public AttackData? AttackData;

	// -----------------------------------------------------------------------------------------------------------------
	// FIELDS
	// -----------------------------------------------------------------------------------------------------------------



	// -----------------------------------------------------------------------------------------------------------------
	// PROPERTIES
	// -----------------------------------------------------------------------------------------------------------------



	// -----------------------------------------------------------------------------------------------------------------
	// SIGNALS
	// -----------------------------------------------------------------------------------------------------------------

	// [Signal] public delegate void EventHandler()

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
		this.HitFromInside = true;
		this.HitBackFaces = false;
		this.CollideWithAreas = true;
		this.CollideWithBodies = false;
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
		this.TryHitIntersectingHurtBox();
	}

	public override string[] _GetConfigurationWarnings()
		=> new ConfigurationWarningsBuilder()
			.Concat(base._GetConfigurationWarnings())
			.MaybeAppend(this.AttackData == null, "This HitRay has no AttackData set. It won't apply any damage.")
			.MaybeAppend(this.CollisionMask == 0, "This HitRay has no CollisionMask set. It won't hit anything.")
			.MaybeAppend(this.CollisionMask == 1, "Property CollisionMask is left at the default value. Did you forget to set it?")
			.Build();


	// -----------------------------------------------------------------------------------------------------------------
	// METHODS
	// -----------------------------------------------------------------------------------------------------------------

	public void ActivateImmediate()
	{
		this.ForceRaycastUpdate();
		this.ForceHitIntersectingHurtBox();
	}

	private void TryHitIntersectingHurtBox()
	{
		if (!this.Enabled) {
			return;
		}
		this.ForceHitIntersectingHurtBox();
	}

	private void ForceHitIntersectingHurtBox()
	{
		if (!this.IsColliding() || this.GetCollider() is not HurtBox3D hurtBox || this.AttackData == null) {
			return;
		}
		hurtBox.TryApplyHit(new() {
			// HitBox = this,
			HurtBox = hurtBox,
			AttackData = this.AttackData,
		});
	}
}
