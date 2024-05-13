using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public static Transform[] targetTransforms;
    public static Transform[] seekerTransforms;

    public GameObject seekerPrefab;
    public GameObject targetPrefab;

    public int numSeekers;
    public int numTargets;

    public Vector2 bounds;

    public void Start()
    {
        Random.InitState(123);

        seekerTransforms = new Transform[numSeekers];
        for (int i = 0; i != numSeekers; i++)
        {
            GameObject go = Instantiate(seekerPrefab);
            Seeker seeker = go.GetComponent<Seeker>();
            Vector2 dir = Random.insideUnitCircle;
            seeker.direction = new Vector3(dir.x, 0, dir.y);
            seekerTransforms[i] = go.transform;
            go.transform.localPosition = new Vector3(Random.Range(0, bounds.x), 0, Random.Range(0, bounds.y));
        }

        targetTransforms = new Transform[numSeekers];
        for (int i = 0; i != numTargets; i++)
        {
            GameObject go = Instantiate(targetPrefab);
            Target target = go.GetComponent<Target>();
            Vector2 dir = Random.insideUnitCircle;
            target.direction = new Vector3(dir.x, 0, dir.y);
            targetTransforms[i] = go.transform;
            go.transform.localPosition = new Vector3(Random.Range(0, bounds.x), 0, Random.Range(0, bounds.y));
        }
    }
}