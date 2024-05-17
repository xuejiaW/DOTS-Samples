using UnityEngine;

public class FindNearest : MonoBehaviour
{
    public void Update()
    {
        foreach (Transform seekerTransform in Spawner.seekerTransforms)
        {
            Vector3 seekerPos = seekerTransform.localPosition;
            Vector3 nearestTargetPos = default;
            float nearestDistSq = float.MaxValue;
            foreach (Transform targetTransform in Spawner.targetTransforms)
            {
                Vector3 offset = targetTransform.localPosition - seekerPos;
                float distSq = offset.sqrMagnitude;

                if (!(distSq < nearestDistSq)) continue;

                nearestDistSq = distSq;
                nearestTargetPos = targetTransform.localPosition;
            }

            Debug.DrawLine(seekerPos, nearestTargetPos);
        }
    }
}