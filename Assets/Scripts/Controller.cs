using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Tabtale.TTPlugins;


public class Controller : MonoBehaviour
{
    [SerializeField] private bool isMoving;

    private Vector3 lastPosition;

    private Transform myTransform;
    private Animator myAnimator;
    
    [SerializeField] private GameObject pistol;
    
    [SerializeField] private GameObject pistolBullet;
    [SerializeField] private bool isFiring = false;

    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject failCanvas;
    
    [SerializeField] private int range = 7 ;
    [SerializeField] private float fireRate = 0.2f ;
    
    private Transform closestEnemyTransform;
    
    public Slider slider;
    public Image fill;

    [SerializeField] private int neededNumToOpenGate;
    [SerializeField] Transform camTransform;
    private bool goNextLevel = false;
    private bool goMiddle = false;





    private void Awake()
    {
        // Initialize CLIK Plugin
        TTPCore.Setup();
        // Your code here
    }
    void Start()
    {
        myTransform = transform;
        lastPosition = myTransform.position;
        isMoving = false;
        myAnimator = gameObject.GetComponent<Animator>();
        slider.maxValue = getEnemyCount();
    }

    // Update is called once per frame
    void Update()
    {
        if ((myTransform.position.x != lastPosition.x) || myTransform.position.z != lastPosition.z)
        {
            isMoving = true;
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            isMoving = false;
            myAnimator.SetBool("isRunning", false);
        }
        lastPosition = myTransform.position;
        Fire();
        OpenTheGates();
        GoMiddleOfLevel();
    }

    void Fire()
    {
        FillSlider();
        if (isMoving == true)
        {
            myAnimator.SetBool("hasPistol", false);
            pistol.SetActive(false);
        }

        else
        {
            closestEnemyTransform = FindClosestEnemy().transform;
            if ((transform.position - closestEnemyTransform.position).sqrMagnitude < range * range)
            {
                // transform.rotation = Quaternion.LookRotation(new Vector3(0, closestEnemyTransform.position.y, closestEnemyTransform.position.z));
                //transform.rotation = Quaternion.LookRotation(-(new Vector3(0, transform.position.y - closestEnemyTransform.position.y, transform.position.x- closestEnemyTransform.position.z)));
                        transform.LookAt(closestEnemyTransform.position);
                //transform.rotation = Quaternion.LookRotation(new Vector3(0,closestEnemyTransform.position.y, closestEnemyTransform.position.z));
                if (isFiring == false)
                {
                    myAnimator.SetBool("hasPistol", true);
                    pistol.SetActive(true);
                    StartCoroutine(fireWithPistol(closestEnemyTransform));
                    isFiring = true;
                    
                }
            }
           
        }
    }

    IEnumerator fireWithPistol(Transform closestEnemyTransformm)
    { 
        if (isMoving == false )
        {
            var bullet = Instantiate(pistolBullet, pistol.transform.position,
                Quaternion.LookRotation(closestEnemyTransformm.position));
            bullet.GetComponent<bullet>().goTo(closestEnemyTransformm.position);
            yield return new WaitForSeconds(fireRate);
        }
        yield return new WaitForSeconds(0.2f);
        isFiring = false;
    }
    
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        if (closest == null)
        {
            winCanvas.SetActive(true);
        }
        return closest;
    }

    private int getEnemyCount()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        return gos.Length;
    }
    
    private void FillSlider()
    {
        slider.value = slider.maxValue - getEnemyCount();
        fill.fillAmount = slider.value;
      /*
        if (slider.value == slider.maxValue)
        {
            winCanvas.SetActive(true);
        }
        */
    }

    private void OpenTheGates()
    {
       
        if( getEnemyCount() < 20 && goNextLevel )
        {
            Debug.Log(getEnemyCount());
            camTransform.DOMoveZ(21.8f, 1);
            goNextLevel = false;
        }

    }
    private void GoMiddleOfLevel()
    {
        if (goMiddle)
        {
            camTransform.DOMoveZ((-1), 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NextLevelTrigger"))
        {
            goMiddle = false;
            goNextLevel = true;
        }
        if (other.CompareTag("MiddleTag1"))
        {

            goMiddle = true;
        }
    }
}
