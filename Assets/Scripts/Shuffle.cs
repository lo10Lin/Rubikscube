using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    List<GameObject> children = new List<GameObject>();
    List<GameObject> slice = new();
    public int shuffleDepth = 3;


    

    public void ReloadShuffle()
    {
        children = new List<GameObject>();
        children.Clear();
        foreach (Transform child in transform)
        {
            if(child.gameObject.activeInHierarchy)
            {
                children.Add(child.gameObject);
            }
        }

        for (int i = 0; i < shuffleDepth; i++)
        {
            RotateRandomSlice();
        }

    }

    private void RotateRandomSlice()
    {
        slice.Clear();
        Vector3 pos = children[Random.Range(0, children.Count - 1)].transform.localPosition;
        Vector3 axis = Vector3.zero;
        switch (Random.Range(0, 3))
        {
            case 0:
                axis = Vector3.up;
                foreach (GameObject cube in children)
                {
                    if (MathExtend.ApproxFloatEqual(MathExtend.CustomRound(cube.transform.localPosition.y), MathExtend.CustomRound(pos.y), 0.1f))
                    {
                        slice.Add(cube);
                    }
                }
                break;

            case 1:
                axis = Vector3.right;
                foreach (GameObject cube in children)
                {
                    if (MathExtend.ApproxFloatEqual(MathExtend.CustomRound(cube.transform.localPosition.x), MathExtend.CustomRound(pos.x), 0.1f))
                    {
                        slice.Add(cube);
                    }
                }
                break;

            case 2:
                axis = Vector3.forward;
                foreach (GameObject cube in children)
                {
                    if (MathExtend.ApproxFloatEqual(MathExtend.CustomRound(cube.transform.localPosition.z), MathExtend.CustomRound(pos.z), 0.1f))
                    {
                        slice.Add(cube);
                    }
                }
                break;


            default:
                print("ya un truc qui va pas");
                break;
        }

        foreach (GameObject cube in slice)
        {
            cube.transform.position = Quaternion.AngleAxis(90, axis) * cube.transform.position;
            cube.transform.rotation = Quaternion.AngleAxis(90, axis) * cube.transform.rotation;
        }
        
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 25;

        GUI.Label(new Rect(10, 130, 150, 40), "Depth = " + shuffleDepth, style);
        shuffleDepth = (int)GUI.HorizontalSlider(new Rect(10, 170, 200, 30), shuffleDepth, 0, 100);
        //if (GUI.Button(new Rect(10, 150, 70, 30), "Clear"))
        //{
        //    print("clear");
        //    children.Clear();
        //}
    }

}

