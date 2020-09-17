using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");

        Entities
            .WithAll<Player>()
            .ForEach((ref Movable mov) =>
        {
            mov.direction = new float3(x, 0, y);
        }).Schedule();
    }
}
