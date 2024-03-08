using UnityEngine;

public static class RandomPointUtility
{
    private static Vector3 GetRandomViewportPoint()
    {
        return new Vector3(Random.value, Random.value, 0);
    }

    public static Vector3 GetRandomWorldPointInCamera(Camera camera)
    {
        Vector3 randomViewportPoint = GetRandomViewportPoint();
        return camera.ViewportToWorldPoint(randomViewportPoint);
    }
}