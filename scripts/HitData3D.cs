#nullable enable
using Godot;

namespace Raele.BasicCombatNodes;

[Tool]
public partial class HitData3D : GodotObject
{
	// public HitBox3D HitBox;
	public HurtBox3D HurtBox;
	public AttackData AttackData;

	// public QuerySpace.GetRestInfoResult? FindContactPoint()
	// 	=> this.HitBox.QuerySpaceIntersectShapeGetCollisionDetails(
	// 		new() {
	// 			CollideWithAreas = true,
	// 			CollideWithBodies = false,
	// 			CollisionMask = this.HurtBox.CollisionLayer,
	// 			Shape = this.HitBox.Shape,
	// 			Transform = this.HitBox.GlobalTransform,
	// 		},
	// 		out QuerySpace.GetRestInfoResult? result
	// 	)
	// 		? result
	// 		: null;
}
