using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Transparency")]
    [Tooltip("If I should make the objects that comes between the player and camera (generaly doors or falling balls) Transparent")]
    public bool UseTransparency;
    public List<ObsTypes> ConcernedTypes;
    public LayerMask ConcernedLayers;
    public Material TransparWhiteMat;

    [Space]
    [Header("Position Related")]
    [Tooltip("The Limit that the X axe of the Camera can't go beyond")]
    public float SideLimit;
    [Tooltip("The Difference between the player X and camera X from which the player start going out of sight")]
    [Range(0 , 15f)]
    public float OutOfSight;
    public Transform Player;
    public Vector3 Offset;

    [Tooltip("The Y value of the Lowest piece of Ground that the Camera shouldn't go below (default is 0)")]
    public float MinHeight;
    [Tooltip("The Height that should be kept from the ground")]
    private float Height;

    void Start()
    {
        //Physics.Raycast(new Ray(transform.position, Vector3.down) , out RaycastHit hit , 7);

        Height = Player.position.y + Offset.y;
    }

    void LateUpdate()
    {
        if (!PlayerInteractions.Dead)
        {
            //For the Y and Z Axes
            transform.position = new Vector3(transform.position.x, Player.position.y + Offset.y >= Height + MinHeight
             ? Player.position.y + Offset.y : Height + MinHeight, Player.position.z + Offset.z);

            //For the X Axe
            if (Player.position.x - transform.position.x > OutOfSight)
            {
                float X = Mathf.Clamp(Player.position.x - OutOfSight, -SideLimit, SideLimit);

                transform.position = new Vector3(X, transform.position.y, transform.position.z);

            }
            else if (Player.position.x - transform.position.x < -OutOfSight)
            {
                float X = Mathf.Clamp(Player.position.x + OutOfSight, -SideLimit, SideLimit);

                transform.position = new Vector3(X, transform.position.y, transform.position.z);

            }

            if (UseTransparency)
            {
                Vector3 offs = Player.position - transform.position;

                if (Physics.Raycast(transform.position, offs, out RaycastHit hit, offs.magnitude, ConcernedLayers))
                {
                    if (hit.transform.TryGetComponent(out MeshRenderer mesh))
                    {

                        Transform Orig = hit.transform;

                        do
                        {
                            if (Orig.TryGetComponent(out IObsTypes type))
                            {
                                if (ConcernedTypes.Contains(type.obsType))
                                {
                                    mesh.material = TransparWhiteMat;
                                }

                                break;
                            }

                            Orig = Orig.parent;

                        } while (Orig != null);
                    }

                }
            }
            
        }       
    }
}
