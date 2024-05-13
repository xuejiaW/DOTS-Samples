using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class FindNearest_Mathematics : MonoBehaviour
{
    public void Update()
    {
        foreach (Transform seekerTransform in Spawner.seekerTransforms)
        {
            float3 seekerPos = seekerTransform.localPosition;
            float3 nearestTargetPos = default;
            float nearestDistSq = float.MaxValue;
            foreach (Transform targetTransform in Spawner.targetTransforms)
            {
                float3 offset = (float3) targetTransform.localPosition - seekerPos;
                float distSq = math.lengthsq(offset);

                if (!(distSq < nearestDistSq)) continue;

                nearestDistSq = distSq;
                nearestTargetPos = targetTransform.localPosition;
            }

            Debug.DrawLine(seekerPos, nearestTargetPos);
        }
    }
}