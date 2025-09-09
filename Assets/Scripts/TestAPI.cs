using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAPI : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DataAPI.Instance.GetRequest("http://localhost:3069/retrieveAll", (users) =>
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
