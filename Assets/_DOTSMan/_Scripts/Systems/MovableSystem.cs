using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class MovableSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, in Movable mov) =>
        {
            var step = mov.direction * mov.speed;
            physicsVelocity.Linear = step;
        }).Schedule();
    }
}
