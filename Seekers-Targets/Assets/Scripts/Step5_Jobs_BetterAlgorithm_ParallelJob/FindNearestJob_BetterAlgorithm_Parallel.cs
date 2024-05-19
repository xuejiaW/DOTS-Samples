using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

// ReSharper disable InconsistentNaming

[BurstCompile]
public struct FindNearestJob_BetterAlgorithm_Parallel : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> targetPositions;
    [ReadOnly] public NativeArray<float3> seekerPositions;

    public NativeArray<float3> nearestTargetPositions;

    public void Execute(int index)
    {
        float3 seekerPos = seekerPositions[index];

        // Find the target with the closest X coord.
        int startIdx = targetPositions.BinarySearch(seekerPos, new AxisXComparer());

        // When no precise match is found, BinarySearch returns the bitwise negation of the last-searched offset.
        // So when startIdx is negative, we flip the bits again, but we then must ensure the index is within bounds.
        if (startIdx < 0)
            startIdx = ~startIdx;

        if (startIdx >= targetPositions.Length) startIdx = targetPositions.Length - 1;

        // The position of the target with the closest X coord.
        float3 nearestTargetPos = targetPositions[startIdx];
        float nearestDistSq = math.distancesq(seekerPos, nearestTargetPos);

        // Searching upwards through the array for a closer target.
        Search(seekerPos, startIdx + 1, targetPositions.Length, +1, ref nearestTargetPos, ref nearestDistSq);

        // Search downwards through the array for a closer target.
        Search(seekerPos, startIdx - 1, -1, -1, ref nearestTargetPos, ref nearestDistSq);

        nearestTargetPositions[index] = nearestTargetPos;
    }

    private void Search(float3 seekerPos, int startIdx, int endIdx, int step, ref float3 nearestTargetPos,
                        ref float nearestDistSq)
    {
        for (int i = startIdx; i != endIdx; i += step)
        {
            float3 targetPos = targetPositions[i];
            float xDiff = seekerPos.x - targetPos.x;

            // If the square of the x distance is greater than the current nearest, we can stop searching.
            if (xDiff * xDiff > nearestDistSq) break;

            float distSq = math.distancesq(targetPos, seekerPos);

            if (!(distSq < nearestDistSq)) continue;

            nearestDistSq = distSq;
            nearestTargetPos = targetPos;
        }
    }
}