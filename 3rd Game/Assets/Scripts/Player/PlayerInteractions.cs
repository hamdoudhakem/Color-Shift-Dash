using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerInteractions : MonoBehaviour
{
    public static int StarsNum { get; private set; }
    public static List<int> IndexsOfObtainedStars { get; private set; }
    public static bool Dead , Win;

    public Material StartCol;
    public GameObject ParticleEffect;
    [HideInInspector] public float Origin;
    [Tooltip("How much the player needs to fall from his original platform to Die")]
    public float FallLimit;
    [Tooltip("The quantity of speed to add to my original speed after the speed boost lv1")]
    public float BoostValueLv1;
    [Tooltip("The quantity of speed to add to my original speed after the speed boost lv2")]
    public float BoostValueLv2;
    [Tooltip("The Duraction of the Speed Boost (In Seconds)")]
    public float SpeedBoostTime;
    public List<TrailProperties> Trails;

    [Header("Events")]
    public UnityEvent<int> ShowStar;
    public UnityEvent Won;
    public UnityEvent Lost;

    private LayerMask ColorSwitch , ColorObst , FinishLine , SpeedBoost , StarLayer;
    private MeshRenderer Mat;
    private PlayerMovement Pm;
    private EffectsBehavior Effs;
    private bool AlreadyIn;

    void Awake()
    {
        AlreadyIn = false;
        StarsNum = 0;
        Dead = false;
        Win = false;
        Origin = transform.position.y;
        Pm = GetComponent<PlayerMovement>();
        Mat = GetComponent<MeshRenderer>();
        Effs = Camera.main.GetComponent<EffectsBehavior>();

        ColorSwitch = LayerMask.NameToLayer("Color Switch");
        ColorObst = LayerMask.NameToLayer("Color Obst");
        FinishLine = LayerMask.NameToLayer("Finish Line");
        SpeedBoost = LayerMask.NameToLayer("Speed Boost");
        StarLayer = LayerMask.NameToLayer("Star");

        Mat.material.color = StartCol.color;
        ChangeTrail();

        //Save System Stuff
        int CurLv = SceneManager.GetActiveScene().buildIndex;

        IndexsOfObtainedStars = PlayerData.LvXStars[CurLv] > 1
                       ? PlayerData.CollectedStarsIndex[CurLv - 1] : new List<int>();

        StarsNum = PlayerData.LvXStars[CurLv];

        for(int i = 0; i < StarsNum; i++)
        {
            ShowStar.Invoke(i+1);
        }
    }

    void Update()
    {
        if (!ScreensEventHandlers.IsPaused)
        {
            if (Origin - transform.position.y >= FallLimit)
            {
                Die();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ColorSwitch)
        {
            Mat.material.color = other.GetComponent<MeshRenderer>().material.color;

            ChangeTrail();

            AudioManager.AudMan.Play("Color Switch" , true);
        }
        else if (other.gameObject.layer == ColorObst)
        {
            DeathCheck(other);
        }
        else if (other.gameObject.layer == SpeedBoost)
        {
            float BoostVal = 0, BoostTimeVal = SpeedBoostTime;
            bool TakeInput = true;
            BoostProperties Bp = other.GetComponent<BoostProperties>();

            //This can be considered the Boost Presets
            if (other.tag == "Boost lv 1")
            {
                BoostVal = BoostValueLv1;
            }
            else if (other.tag == "Boost lv 2")
            {
                BoostVal = BoostValueLv2;
                TakeInput = false;
            }

            if (Bp.OverideBoostVal)
            {
                BoostVal = Bp.OveridedBoostVal;
            }

            if (Bp.OverideBoostTime)
            {
                BoostTimeVal = Bp.OveridedBoostTime;
            }

            StartCoroutine(Pm.SpeedUp(BoostVal, BoostTimeVal, TakeInput));

        }
        else if (other.tag == "Cannon Stuff")
        {
            other.transform.parent.GetComponent<CannonsObs>().DestCan(other.transform);
        }
        else if (other.gameObject.layer == StarLayer)
        {
            if (!AlreadyIn)
            {
                AudioManager.AudMan.Play("Star");

                AlreadyIn = true;

                //Increase Stars
                StarsNum++;
                ShowStar.Invoke(StarsNum);

                //Deal With The Star
                other.GetComponent<Collider>().enabled = false;

                var index = other.transform.parent.GetSiblingIndex();
                IndexsOfObtainedStars.Add(index);
                Debug.Log("I Got the star at the index = " + index);


                other.GetComponent<Animator>().SetTrigger("Disappear");

                StartCoroutine(DisbaleStar(other.transform.parent.gameObject , .5f));
            }
        }

        AlreadyIn = false;
    }   

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ColorObst)
        {
            DeathCheck(collision.collider);
        }
        else if (collision.gameObject.layer == FinishLine)
        {
            if(StarsNum < 3)
            {
                StarsNum++;
                ShowStar.Invoke(StarsNum);
            }                                  

            StartCoroutine(Pm.Stop());

            Win = true;
            AudioManager.AudMan.StopAll();

            Won.Invoke();
        }
        else if (collision.transform.tag == "Obstacle")
        {            
            Die();
        }
    }      

    void DeathCheck(Collider col)
    {             
        if (col.gameObject.GetComponent<MeshRenderer>().material.color != Mat.material.color)
        {          
            Die();
        }
        else
        {
            if(col.tag == "Ring")
            {
                AudioManager.AudMan.Play("Ring Passed", true);
            }
        }     
    }

    void Die()
    {
        Debug.Log("You Died !");        

        Dead = true;

        AudioManager.AudMan.StopAll();
        AudioManager.AudMan.Play("Lost");
        AudioManager.AudMan.Play("Died");        

        Lost.Invoke();

        Instantiate(ParticleEffect, transform.position, new Quaternion()).transform.Rotate(Vector3.right * -90);

        Pm.StopAllCoroutines();

        Destroy(gameObject);
    }

    IEnumerator DisbaleStar(GameObject star,float Time)
    {
        yield return new WaitForSeconds(Time);

        star.SetActive(false);
    }

    void ChangeTrail()
    {
        Material mat = Effs.trailRendrer.material;

        float intMul = Mathf.Pow(2, Trails[0].Intensity1);
        Color col = Trails[0].Color1.linear;

        Debug.Log("COlor 1 is (gamma)" + mat.GetColor("_Color1").gamma + " /(linear) : " + mat.GetColor("_Color1").linear);

        mat.SetColor("Color 1", col * intMul);
       

        //// if not using gamma color space, convert from linear to gamma
        //# ifndef UNITY_COLORSPACE_GAMMA
        //        emissiveColor.rgb = LinearToGammaSpace(emissiveColor.rgb);
        //#endif

        //// apply intensity exposure
        //emissiveColor.rgb *= pow(2.0, _foggedIntensity);

        //// if not using gamma color space, convert back to linear
        //# ifndef UNITY_COLORSPACE_GAMMA
        //        emissiveColor.rgb = GammaToLinearSpace(emissiveColor.rgb);
        //#endif
    }
}
