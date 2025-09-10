using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAPI : MonoBehaviour
{
    [SerializeField] private Transform gridParent;

    void Start()
    {
        var api = new DataAPI("http://localhost:3069");


        StartCoroutine(api.GetRequest("/retrieveAll", (users) =>
        {
            if (users != null)
            {
                Debug.Log(users[300].age);
                Debug.Log("Number of users received: " + users.Length);
            }
            else
            {
                Debug.LogError("Failed to load trees");
            }
        }));
        Vector3[,] gridPositions = GridGenerator.GenerateGridPositions(256, 256, 1f, 1f, gridParent);
        
        // Example: spawn cubes at each position
        for (int z = 0; z < gridPositions.GetLength(0); z++)
        {
            for (int x = 0; x < gridPositions.GetLength(1); x++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = gridPositions[z, x];
                cube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
