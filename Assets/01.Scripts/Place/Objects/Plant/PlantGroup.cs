using UnityEngine;

public class PlantGroup : MonoBehaviour
{
    private readonly Vector3[] plantPositions = 
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(-1, 0, -1)
    };

    public Vector3[] PlantPositions => plantPositions;
}
