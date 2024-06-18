#nullable enable
#if TOOLS
using Godot;

namespace Raele.BasicCombatNodes;

[Tool]
public partial class BasicCombatNodesPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		this.SetupCustomNodes();
	}

	public void SetupCustomNodes()
	{
        (string, string)[] scripts = new (string, string)[] {
			(nameof(HitBox3D), nameof(Area3D)),
			(nameof(HitRay3D), nameof(RayCast3D)),
			(nameof(HurtBox3D), nameof(Area3D)),
			(nameof(Health), nameof(Node)),
		};

		foreach ((string classPath, string baseType) in scripts) {
			Texture2D icon = GD.Load<Texture2D>($"res://addons/{nameof(BasicCombatNodes)}/icons/{classPath}.png");
			Script script = GD.Load<Script>($"res://addons/{nameof(BasicCombatNodes)}/scripts/{classPath}.cs");
			this.AddCustomType(classPath, baseType, script, icon);
		}
	}
}
#endif
