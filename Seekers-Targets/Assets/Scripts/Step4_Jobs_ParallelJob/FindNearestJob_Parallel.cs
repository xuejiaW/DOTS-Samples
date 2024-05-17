using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

[BurstCompile]
public struct FindNearestJob_Parallel : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> targetPositions;
    [ReadOnly] public NativeArray<float3> seekerPositions;

    public NativeArray<float3> nearestTargetPositions;

    public void Execute(int index)
    {
        float3 seekerPos = seekerPositions[index];
        float nearestDistSq = float.MaxValue;
        for (int j = 0; j != targetPositions.Length; ++j)
        {
            float3 targetPos = targetPositions[j];
            float distSq = distancesq(seekerPos, targetPos);
            if (distSq < nearestDistSq)
            {
                nearestDistSq = distSq;
                nearestTargetPositions[index] = targetPos;
            }
        }
    }
}