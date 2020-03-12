using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System;

public class AK47_Logic : MonoBehaviour
{
    [Header("AK47")]
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    
    [Header("Score")]
    public int killCount;
    public Text killCountText;

    [Header("Camera")]
    public Camera fpsCam;
    
    [Header("Bullet")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    [Header("Audio")]
	public AudioClip gunFire;
    public AudioSource audioSource;

    [Tooltip("The names of the axes and buttons for Unity's Input Manager."), SerializeField]
    private FpsInput input;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
        
    }

    void Shoot(){
        muzzleFlash.Play();
        audioSource.GetComponent<AudioSource>().clip = gunFire;
        audioSource.Play();

        RaycastHit hit; 
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) {

            Damageable_Target target = hit.transform.GetComponent<Damageable_Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
                Debug.Log(target.health);
                if(target.health <= 0) {
                    killCount = Int32.Parse(killCountText.text);
                    killCount += 1;

                    killCountText.text = killCount.ToString();
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal*impactForce );
            }

            GameObject impactOBJ = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactOBJ, 2f);
        }
    }


    /// Input mappings
        [Serializable]
        private class FpsInput
        {
            [Tooltip("The name of the virtual axis mapped to rotate the camera around the y axis."),
             SerializeField]
            private string rotateX = "Mouse X";

            [Tooltip("The name of the virtual axis mapped to rotate the camera around the x axis."),
             SerializeField]
            private string rotateY = "Mouse Y";

            [Tooltip("The name of the virtual axis mapped to move the character back and forth."),
             SerializeField]
            private string move = "Horizontal";

            [Tooltip("The name of the virtual axis mapped to move the character left and right."),
             SerializeField]
            private string strafe = "Vertical";

            [Tooltip("The name of the virtual button mapped to run."),
             SerializeField]
            private string run = "Fire3";

            [Tooltip("The name of the virtual button mapped to jump."),
             SerializeField]
            private string jump = "Jump";

            /// Returns the value of the virtual axis mapped to rotate the camera around the y axis.
            public float RotateX
            {
                get { return Input.GetAxisRaw(rotateX); }
            }
				         
            /// Returns the value of the virtual axis mapped to rotate the camera around the x axis.        
            public float RotateY
            {
                get { return Input.GetAxisRaw(rotateY); }
            }
				        
            /// Returns the value of the virtual axis mapped to move the character back and forth.        
            public float Move
            {
                get { return Input.GetAxisRaw(move); }
            }
				       
            /// Returns the value of the virtual axis mapped to move the character left and right.         
            public float Strafe
            {
                get { return Input.GetAxisRaw(strafe); }
            }
				    
            /// Returns true while the virtual button mapped to run is held down.          
            public bool Run
            {
                get { return Input.GetButton(run); }
            }
				     
            /// Returns true during the frame the user pressed down the virtual button mapped to jump.          
            public bool Jump
            {
                get { return Input.GetButtonDown(jump); }
            }
        }
}
