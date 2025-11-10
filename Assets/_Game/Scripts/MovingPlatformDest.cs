using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformDest : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform GetDestPoint(int index)
    {
        return transform.GetChild(index);
    }

    public int GetNextIndex(int currentIndex)
    {
        int nextIndex = currentIndex + 1;

        if (nextIndex == transform.childCount)
        {
            nextIndex = 0;
        }
        return nextIndex;
    }
}
