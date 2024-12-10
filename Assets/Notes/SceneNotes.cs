using UnityEngine;

[ExecuteAlways]
public class SceneNote : MonoBehaviour
{
    [TextArea]
    public string note = "Remember to coordinate scene edits to avoid Git conflicts!";
}
