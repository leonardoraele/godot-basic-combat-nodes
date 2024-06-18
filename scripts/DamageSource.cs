#nullable enable
using Godot;
using Raele.GodotUtil;

namespace Raele.BasicCombatNodes;

[Tool]
public partial class DamageSource : Node
{
	// -----------------------------------------------------------------------------------------------------------------
	// STATICS
	// -----------------------------------------------------------------------------------------------------------------

	// public static readonly string MyConstant = "";

	// -----------------------------------------------------------------------------------------------------------------
	// EXPORTS
	// -----------------------------------------------------------------------------------------------------------------

	[Export(PropertyHint.Enum, "HurtBox")] int Source;
	[Export] public HurtBox3D? HurtBox;

	// -----------------------------------------------------------------------------------------------------------------
	// FIELDS
	// -----------------------------------------------------------------------------------------------------------------

	private Health? ParentCache;

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
		if (Engine.IsEditorHint() || this.HurtBox == null) {
			return;
		}
		this.HurtBox.HitRegistered += this.OnHitRegistered;
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
			.MaybeAppend(this.GetParent() is not Health, $"This node must be a child of a {nameof(Health)} node.")
	  .MaybeAppend(this.HurtBox == null, $"Property {nameof(this.HurtBox)} is not set. This node will not work without it.")
			.Build();

	public override void _Notification(int what)
	{
		base._Notification(what);
		if (what == Node.NotificationParented && this.GetParent() is Health parent) {
			this.ParentCache = parent;
		} else if (what == Node.NotificationUnparented) {
			this.ParentCache = null;
		}
	}

	// -----------------------------------------------------------------------------------------------------------------
	// METHODS
	// -----------------------------------------------------------------------------------------------------------------

	private void OnHitRegistered(HitData3D hitData)
	{
		if (this.ParentCache == null) {
			return;
		}
		this.ParentCache.ApplyAttack(hitData.AttackData);
	}
}
