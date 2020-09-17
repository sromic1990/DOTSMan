using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class MovableSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Movable mov, ref Translation translation, ref Rotation rotation) =>
        {
            translation.Value += mov.speed * mov.direction * deltaTime;
            rotation.Value = math.mul(rotation.Value.value, quaternion.RotateY(mov.speed * deltaTime));
        }).Schedule();
    }
}
