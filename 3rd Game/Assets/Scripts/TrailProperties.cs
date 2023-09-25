using UnityEngine;

[CreateAssetMenu(fileName = "New Trail", menuName = "Scriptable Objects/New Trail")]
public class TrailProperties : ScriptableObject
{
    public Material ConcernedMat;

    [Header("Trail Material Stuff")]

    public Color Color1;
    public Color Color2;
    [Tooltip("The Intensity Value for the Colors on the Trail Material")]
    public float Intensity1, Intensity2;    

    [Header("Trail Rendrer Stuff")]

    [Tooltip("The Time Propertie for the Trail Rendrer (default is 1)")]
    public float Time = 1;

    [Tooltip("The Width Propertie for the Trail Rendrer (default is 0.5)")]
    public float width = .5f;
}
