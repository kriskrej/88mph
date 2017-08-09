using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour {
    [SerializeField] Transform roadHolder;
    [SerializeField] List<RoadFragment> roadFragments;
    [SerializeField] int roadLength = 20;
    [SerializeField] RoadFragment safeFragment;
    [SerializeField] int safeAmount = 3;

    Vector3 currentPosition = Vector3.zero;

    void Awake() {
        SpawnRoad();
    }

    void SpawnRoad() {
        var i = 0;
        for (; i < safeAmount; i++)
            SpawnFragment(currentPosition, safeFragment);
        for (; i < roadLength; i++)
            SpawnFragment(currentPosition, GetRandomFragment());
    }

    void SpawnFragment(Vector3 position, RoadFragment fragment) {
        var fragmentObject = Instantiate(fragment.gameObject);
        SetupFragmentTransform(position, fragmentObject);
        UpdateTransformData(fragment);
    }

    void UpdateTransformData(RoadFragment fragment) {
        currentPosition += fragment.positionDelta;
    }

    void SetupFragmentTransform(Vector3 position, GameObject fragmentObject) {
        fragmentObject.transform.parent = roadHolder;
        fragmentObject.transform.localPosition = position;
        fragmentObject.transform.rotation = Quaternion.identity;
    }

    RoadFragment GetRandomFragment() {
        var fragmentIndex = Random.Range(0, roadFragments.Count);
        var fragment = roadFragments[fragmentIndex];
        return fragment;
    }
}