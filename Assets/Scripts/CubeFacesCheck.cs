using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFacesCheck : MonoBehaviour
{
    Vector3 RoundVectorToInt(Vector3 vec)
    {
        vec.x = Mathf.RoundToInt(vec.x);
        vec.y = Mathf.RoundToInt(vec.y);
        vec.z = Mathf.RoundToInt(vec.z);

        return vec;
    }

    public bool IsResolved(List<Transform> cubes)
    {
        Vector3 targetRotation = RoundVectorToInt(cubes[0].rotation.eulerAngles);

        for (int i = 1; i < cubes.Count; i++)
        {
            if (targetRotation != RoundVectorToInt(cubes[i].rotation.eulerAngles))
                return false;
        }
        return true;
    }
}
