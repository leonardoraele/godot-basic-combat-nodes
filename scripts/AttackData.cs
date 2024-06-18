using Godot;

namespace Raele.BasicCombatNodes;

[Tool]
[GlobalClass]
public partial class AttackData : Resource
{
	[Export] public ulong Damage = 1;

	[ExportGroup("Tags")]
	[Export] public string[]? Tags;
}
