using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CollectableObject : MonoBehaviour
{
    public GameObject block;
    public Item item = null; //item in collectable object
    public int itemCount = 1; //how much item?
    public float collectSpeed = 1.0f;
    public bool collecting = false;

    [Header("Sounds")]
    public AudioSource miningSFX;
    public AudioSource breakingSFX;

    private Vector3 startScale = Vector3.one;
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        //Set collectable object's mateirlas to item's material
        if(item.collectableObjectMaterial != null)
        {
            block.GetComponent<Renderer>().sharedMaterial = item.collectableObjectMaterial;
        }
        //get local scale
        startScale = block.transform.localScale;
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Shrink while collecting
        if (collecting)
        {
            //start particle system
            if (!ps.isPlaying)
                ps.Play(true);
            //Sound
            if (!miningSFX.isPlaying)
                miningSFX.Play();
            //collect at 50% size
            if (block.transform.localScale.x <= startScale.x / 2.0f)
            {
                Collected();
                return;
            }
            //Shrink
            block.transform.localScale *= 1.0f - (collectSpeed * Time.deltaTime);
            collecting = false;
        }
        //Unshrink while !collecting
        else if (block.transform.localScale != startScale)
        {
            //Sound
            if (miningSFX.isPlaying)
                miningSFX.Stop();
            //back to regular size
            if (block.transform.localScale.x >= startScale.x / 2.0f)
            {
                block.transform.localScale = startScale;
                //Shut off particle system
                if (ps.isPlaying)
                    ps.Stop();
                return;
            }
            //Unshrink
            block.transform.localScale /= collectSpeed * Time.deltaTime;
        }
    }

    public void Collected()
    {
        breakingSFX.Play();

        if (Inventory.HasInstance)
        {
            bool result = Inventory.instance.AddItem(item, itemCount);

            if(result == true)
            {
                Debug.Log("Added " + item.itemName);
            }
            else
            {
                Debug.Log("Couldn't added " + item.itemName);
            }
         
        }
        else
        {
            Debug.LogWarning("CollectableObject: Inventory singleton instance not exist!");
        }
        Destroy(gameObject);
    }
}
