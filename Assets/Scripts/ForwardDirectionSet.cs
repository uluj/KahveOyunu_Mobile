using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]

public class ForwardDirectionSet : MonoBehaviour
{
public Transform forwardDirection;
    void Update()
    {
        transform.forward = forwardDirection.forward;
    }
}
