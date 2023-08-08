using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSwitcher : MonoBehaviour, IObsTypes
{
    [field: SerializeField]
    public ObsTypes obsType { get; set; }

    [Tooltip("How much Delay between every Ground Change (In seconds)")]
    public float Delay;
    [Tooltip("I Will creat a range equal to [Delay - DelayRange / 2 , Delay + DelayRange] that will randomly generate the true Delay that will be used")]
    public float DelayRange;
    [Tooltip("The Offset to the center of the Overlap Box")]
    public Vector3 Offset;
    [Tooltip("Size of the overlap Box")]
    public Vector3 Size;
    public LayerMask PlayerLayer;    

    private GameObject[] Grs;    

    void Start()
    {
        Grs = new GameObject[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            Grs[i] = transform.GetChild(i).gameObject;
        }

        Delay = Random.Range(Delay - DelayRange / 2, Delay + DelayRange);

        InvokeRepeating("ChangeDelay", 0, Delay);
    }    

    void ChangeDelay()
    {
        if(Grs[0].activeSelf == false)
        {
            Grs[0].SetActive(true);
            Grs[1].SetActive(false);
        }
        else
        {
            Grs[0].SetActive(false);
            Grs[1].SetActive(true);
        }       

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + Offset, Size * 2);
    }

    void Update()
    {
        if (Physics.OverlapBox(transform.position + Offset, Size ,new Quaternion(),PlayerLayer).Length > 0) 
        {
            CancelInvoke();
        }

    }

}
