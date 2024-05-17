using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class FindNearest_ParallelJob : MonoBehaviour
{
    private NativeArray<float3> m_TargetPositions;
    private NativeArray<float3> m_SeekerPositions;
    private NativeArray<float3> m_NearestTargetPositions;

    public void Start()
    {
        var spawner = FindObjectOfType<Spawner>();

        m_TargetPositions = new NativeArray<float3>(spawner.numTargets, Allocator.Persistent);
        m_SeekerPositions = new NativeArray<float3>(spawner.numSeekers, Allocator.Persistent);
        m_NearestTargetPositions = new NativeArray<float3>(spawner.numSeekers, Allocator.Persistent);
    }

    public void Update()
    {
        for (int i = 0; i != m_TargetPositions.Length; ++i)
        {
            m_TargetPositions[i] = Spawner.targetTransforms[i].localPosition;
        }

        for (int i = 0; i != m_SeekerPositions.Length; ++i)
        {
            m_SeekerPositions[i] = Spawner.seekerTransforms[i].localPosition;
        }

        FindNearestJob_Parallel findJob = new FindNearestJob_Parallel
        {
            targetPositions = m_TargetPositions,
            seekerPositions = m_SeekerPositions,
            nearestTargetPositions = m_NearestTargetPositions
        };

        JobHandle findHandle = findJob.Schedule(m_SeekerPositions.Length, 100);
        findHandle.Complete();

        for (int i = 0; i != m_SeekerPositions.Length; ++i)
        {
            Debug.DrawLine(m_SeekerPositions[i], m_NearestTargetPositions[i]);
        }
    }

    private void OnDestroy()
    {
        m_TargetPositions.Dispose();
        m_SeekerPositions.Dispose();
        m_NearestTargetPositions.Dispose();
    }
}