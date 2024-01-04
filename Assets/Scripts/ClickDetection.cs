using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickDetection : MonoBehaviour
{
    public Text ResolvedText;
    private CubeFacesCheck facesCheck;

    public List<Transform> cubes;
    public List<Transform> slice;
    RaycastHit firstHit = new RaycastHit();
    Vector3 normal;

    [SerializeField] bool autoRotating = false;
    [SerializeField] bool rotating = false;
    [SerializeField] Vector2 mouseDirRotation;
    [SerializeField] float currentSliceRotation = 0;
    Vector3 currAxis;

    [SerializeField] float mouseSensitivity = 0.8f;
    bool isResolved = false;

    

    public void ReloadCube()
    {
        cubes.Clear();
        cubes = new List<Transform>(GetComponentsInChildren<Transform>());
        cubes.RemoveAt(0);
    }

    private void Start()
    {
        facesCheck = GetComponent<CubeFacesCheck>();
        firstHit.point = Vector3.zero;
        ReloadCube();
    }
    private void Update()
    {
        if (Input.GetMouseButton(1) && !autoRotating && !rotating)
        {
            OnClick();
        }

        if (rotating)
        {
            HoldToRotate();
        }
    }

    void HoldToRotate()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        float angle = Vector2.Dot(mouseDirRotation, mouseDelta);
        //print("mouseDelta : " + mouseDelta);
        //print("angle : " + angle);
        RotateSlice(angle * mouseSensitivity);
        currentSliceRotation += angle * mouseSensitivity;

        if (Input.GetMouseButtonUp(1))
        {
            print("Start snapping");
            rotating = false;
            autoRotating = true;
            StartCoroutine(SliceRotation(Mathf.Round(currentSliceRotation / 90f) * 90));
        }
    }

    void RotateSlice(float angle)
    {
        foreach (Transform cube in slice)
        {
            cube.position = Quaternion.AngleAxis(angle, currAxis) * cube.position;
            cube.rotation = Quaternion.AngleAxis(angle, currAxis) * cube.rotation;
        }
    }

    void OnClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out hit, 100)) // Raycast hits nothing -> Skip
            return;

        if (firstHit.point == Vector3.zero) // First Click
        {
            firstHit = hit;
            normal = hit.normal;
            return;
        }

        if (Vector3.Distance(hit.point, firstHit.point) < 0.2f) // If Second Click = First Click -> Skip
            return;

        mouseDirRotation = Camera.main.WorldToScreenPoint(hit.point) - Camera.main.WorldToScreenPoint(firstHit.point);

        // Second Click

        rotating = true;
        slice.Clear();

        Vector3 refVector = hit.point - firstHit.point; // RefVector = RaycastHit1, RaycastHit2 Vector
        refVector.Normalize();

        // Find Closest Directionnal Vector (vecDir)
        float dotProduct = Vector3.Dot(refVector, Vector3.up);
        Vector3 vecDir = Vector3.up;
        MathExtend.CompareDot(ref dotProduct, Vector3.down, refVector, ref vecDir);
        MathExtend.CompareDot(ref dotProduct, Vector3.right, refVector, ref vecDir);
        MathExtend.CompareDot(ref dotProduct, Vector3.left, refVector, ref vecDir);
        MathExtend.CompareDot(ref dotProduct, Vector3.forward, refVector, ref vecDir);
        MathExtend.CompareDot(ref dotProduct, Vector3.back, refVector, ref vecDir);

        // Rotation Axis
        Vector3 rotAxis = Vector3.Cross(normal, vecDir);
        rotAxis.x = Mathf.RoundToInt(rotAxis.x);
        rotAxis.y = Mathf.RoundToInt(rotAxis.y);
        rotAxis.z = Mathf.RoundToInt(rotAxis.z);
        if (rotAxis == Vector3.zero)
        {
            firstHit.point = Vector3.zero;
            autoRotating = false;
            return;
        }

        //Debug.Log("RotAxis : " + rotAxis);
        //Debug.Log("VecDir : " + vecDir);


        if (rotAxis == Vector3.left || rotAxis == Vector3.right) // RotAxis = X Axis
        {
            foreach (Transform cube in cubes)
            {
                if (MathExtend.ApproxFloatEqual(MathExtend.CustomRound(cube.localPosition.x), MathExtend.CustomRound(firstHit.transform.localPosition.x), 0.1f))
                {
                    slice.Add(cube);
                }
            }
        }
        else if (rotAxis == Vector3.up || rotAxis == Vector3.down) // RotAxis = Y Axis
        {
            print("UP, DOWN");
            foreach (Transform cube in cubes)
            {
                if (MathExtend.ApproxFloatEqual(MathExtend.CustomRound(cube.localPosition.y), MathExtend.CustomRound(firstHit.transform.localPosition.y), 0.1f))
                {
                    slice.Add(cube);
                }
            }
        }

        else if (rotAxis == Vector3.forward || rotAxis == Vector3.back) // RotAxis = Z Axis
        {
            foreach (Transform cube in cubes)
            {
                if (MathExtend.ApproxFloatEqual(MathExtend.CustomRound(cube.localPosition.z), MathExtend.CustomRound(firstHit.transform.localPosition.z), 0.1f))
                {
                    slice.Add(cube);
                }
            }
        }

        currAxis = rotAxis;
        // Rotation animation
        //StartCoroutine(SliceRotation(slice, rotAxis));

        firstHit.point = Vector3.zero;

    }
    IEnumerator SliceRotation(float angle)
    {
        print(angle);
        float cureentRot = currentSliceRotation;
        while (Mathf.Abs(angle - cureentRot) > 2)
        {
            float deltaRot = angle < cureentRot ? -1 : 1;
            cureentRot += deltaRot;
            foreach (Transform cube in slice)
            {
                cube.position = Quaternion.AngleAxis(deltaRot, currAxis) * cube.position;
                cube.rotation = Quaternion.AngleAxis(deltaRot, currAxis) * cube.rotation;
            }

            yield return new WaitForSeconds(0f);
        }
        foreach (Transform cube in slice)
        {
            cube.position = Quaternion.AngleAxis(angle - cureentRot, currAxis) * cube.position;
            cube.rotation = Quaternion.AngleAxis(angle - cureentRot, currAxis) * cube.rotation;
        }

        if (facesCheck.IsResolved(cubes))
            ResolvedText.enabled = true;
        else
            ResolvedText.enabled = false;

        slice.Clear();
        autoRotating = false;
        currentSliceRotation = 0;
        yield return new WaitForSeconds(0f);
    }
}


