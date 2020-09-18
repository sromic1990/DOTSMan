using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class EnemySystem : SystemBase
{
    private Random _random = new Random(1234);
    
    protected override void OnUpdate()
    {
        var rayCaster = new MovementRaycast(){ physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld };
        _random.NextInt();
        var randomTemp = _random;
        
        
        Entities.ForEach((ref Movable movable, ref Enemy enemy, in Translation translation) =>
        {
            if (math.distance(translation.Value, enemy.previousCell) > 0.9f)
            {
                enemy.previousCell = math.round(translation.Value);
                
                //Perform raycasts here
                var validDirection = new NativeList<float3>(Allocator.Temp);
                
                //4 raycasts - up down left right
                if(!rayCaster.CheckRay(translation.Value, new float3(0, 0, -1), movable.direction))
                {
                    validDirection.Add(new float3(0, 0, -1));
                }
                if(!rayCaster.CheckRay(translation.Value, new float3(0, 0, 1), movable.direction))
                {
                    validDirection.Add(new float3(0, 0, 1));
                }
                if(!rayCaster.CheckRay(translation.Value, new float3(-1, 0, 0), movable.direction))
                {
                    validDirection.Add(new float3(-1, 0, 0));
                }
                if(!rayCaster.CheckRay(translation.Value, new float3(1, 0, 0), movable.direction))
                {
                    validDirection.Add(new float3(1, 0, 0));
                }

                movable.direction = validDirection[randomTemp.NextInt(validDirection.Length)];
                validDirection.Dispose();
            }
        }).Schedule();
    }

    private struct MovementRaycast
    {
        [ReadOnly]public PhysicsWorld physicsWorld;
        
        public bool CheckRay(float3 position, float3 direction, float3 currentDirection)
        {
            if (direction.Equals(-currentDirection))
                return true;
            
            RaycastInput ray = new RaycastInput()
            {
                Start = position,
                End = position + (direction) * 0.9f,
                Filter = new CollisionFilter()
                {
                    BelongsTo = 1u << 1,
                    CollidesWith = 1u << 2,
                    GroupIndex = 0
                }
            };
            return physicsWorld.CastRay(ray);
        }
    }
}