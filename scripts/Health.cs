#nullable enable
using System;
using Godot;

namespace Raele.BasicCombatNodes;

[Tool]
public partial class Health : Node
{
	// -----------------------------------------------------------------------------------------------------------------
	// STATICS
	// -----------------------------------------------------------------------------------------------------------------

	// public static readonly string MyConstant = "";

	// -----------------------------------------------------------------------------------------------------------------
	// EXPORTS
	// -----------------------------------------------------------------------------------------------------------------

	[Export] public double CurrentHP = 1;

	/// <summary>
	/// If this property is set to a negative value, the current HP has no maximum limit.
	/// </summary>
	[Export] public double MaximumHP = -1;

	// -----------------------------------------------------------------------------------------------------------------
	// FIELDS
	// -----------------------------------------------------------------------------------------------------------------

	// private

	// -----------------------------------------------------------------------------------------------------------------
	// PROPERTIES
	// -----------------------------------------------------------------------------------------------------------------

	public bool Dead => this.CurrentHP <= double.Epsilon;

	// -----------------------------------------------------------------------------------------------------------------
	// SIGNALS
	// -----------------------------------------------------------------------------------------------------------------

	[Signal] public delegate void HurtEventHandler(AttackData attackData);
	[Signal] public delegate void SurvivedEventHandler(AttackData attackData);
	[Signal] public delegate void DiedEventHandler();

	// -----------------------------------------------------------------------------------------------------------------
	// INTERNAL TYPES
	// -----------------------------------------------------------------------------------------------------------------

	// private enum Type {
	// 	Value1,
	// }

	// -----------------------------------------------------------------------------------------------------------------
	// EVENTS
	// -----------------------------------------------------------------------------------------------------------------

	// public override void _EnterTree()
	// {
	// 	base._EnterTree();
	// }

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

	// public override string[] _GetConfigurationWarnings()
	// 	=> base._PhysicsProcess(delta);

	// -----------------------------------------------------------------------------------------------------------------
	// METHODS
	// -----------------------------------------------------------------------------------------------------------------

	public void ApplyAttack(AttackData attackData)
	{
		if (this.Dead) {
			return;
		}
		this.CurrentHP = Math.Clamp(
			this.CurrentHP - attackData.Damage,
			0,
			this.MaximumHP > 0 ? this.MaximumHP : double.PositiveInfinity
		);
		this.EmitSignal(SignalName.Hurt, attackData);
		if (this.Dead) {
			this.EmitSignal(SignalName.Died);
		} else {
			this.EmitSignal(SignalName.Survived, attackData);
		}
	}

	public void TryKill()
	{
		if (this.Dead) {
			return;
		}
		this.ForceKill();
	}

	public void ForceKill()
	{
		this.CurrentHP = 0;
		this.EmitSignal(SignalName.Died);
	}

	public void OnHurtBox3DHit(HitData3D hitData) => this.ApplyAttack(hitData.AttackData);
}
