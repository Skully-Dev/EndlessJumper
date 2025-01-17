﻿using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField]
    private Transform midScreen;
    [SerializeField]
    private Transform background;

    private float loopPoint;

    private float loopAmount = 20.48f;
    // Start is called before the first frame update
    void Start()
    {
        loopPoint = loopAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (midScreen.position.y > loopPoint)
        {
            Vector2 temp = background.position;
            temp.y += loopAmount;
            background.position = temp;
            loopPoint += loopAmount;
        }
    }
}
