using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCube : MonoBehaviour
{
    ClickDetection cubeArray;

    [SerializeField]
    private Material red, yellow, black, blue, white, lime, orange;
    public GameObject prefab;
    public int UIsize;

    public Transform mainCamera;

    private Shuffle shuffle;

    void Start()
    {
        cubeArray = GetComponent<ClickDetection>();
        shuffle = GetComponent<Shuffle>();
        Reload(UIsize);
    }

    public void Reload(int length)
    {
        cubeArray.cubes.Clear();
        
        int cubeCount = 0;
        mainCamera.localPosition = new Vector3(0, 0, length * 1.4f * -1 - 3);
        //mainCamera.localPosition = new Vector3(0, 0, -15);

        float cubeCutoff = length * Mathf.Cos((5f * Mathf.PI) / 8f);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
            //transform.DetachChildren();
        }
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < length; k++)
                {
                    Vector3 cubePose = new Vector3(i, j, k) - new Vector3(length, length, length) / 2f + Vector3.one / 2f;
                    //print(cubePose);

                    if (cubePose.x - 0.5f >= cubeCutoff && cubePose.x + 0.5f <= -cubeCutoff 
                        && cubePose.y - 0.5f >= cubeCutoff && cubePose.y + 0.5f <= -cubeCutoff 
                        && cubePose.z - 0.5f >= cubeCutoff && cubePose.z + 0.5f <= -cubeCutoff)
                        continue;

                    transform.rotation = Quaternion.identity;
                    GameObject cube = Instantiate(prefab, cubePose, Quaternion.identity, transform);
                    //print(cube.transform.localPosition);
                    cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    Renderer rend = cube.GetComponent<Renderer>();

                    cubeArray.cubes.Add(cube.transform);

                    if (i != 0)
                        rend.materials[2].color = black.color;  
                    if (j != 0)
                        rend.materials[4].color = black.color;
                    if (k != 0)
                        rend.materials[3].color = black.color;
                    if (i != length - 1)
                        rend.materials[5].color = black.color;
                    if (j != length - 1)
                        rend.materials[1].color = black.color;
                    if (k != length - 1)
                        rend.materials[6].color = black.color;

                    cubeCount++;

                }
            }
        }
        print(cubeCount);
       shuffle.ReloadShuffle();
    }

    
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 25;

        if (GUI.Button(new Rect(10, 10, 70, 30), "Reload"))
        {
            print("reloading");
            Reload(UIsize);
        }
        GUI.Label(new Rect(10, 50, 500, 150), "Size = " + UIsize, style);
        UIsize = (int)GUI.HorizontalSlider(new Rect(10, 90, 150, 30), UIsize, 2, 10);
    }
}
