using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour {
    public float rotationSpeed = 60.0f; //ideal rotation speed 60-70
    public float movementSpeed = 20.0f; //ideal walking speed 20

    private float speedMultiplier = 1;
    public float dashSpeedMultiplier = 5;
    public float dashDuration = 0.25f; // in seconds

    private bool coolDown = false;
    public float coolDownTime = 3; // in seconds

    public bool hasKart = false;// ShopKart uses this bool to check whether or not the player already has a kart
    public SoundPlayer soundPlayer;

    void Start()
    {
        //Camera.main.gameObject.AddComponent<AudioSource>;
    }

    // Update is called once per frame
    void Update () {

        float translation = Time.deltaTime*speedMultiplier;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) && !soundPlayer.isSourcePlaying("Player-Body"))
        {
            soundPlayer.PlaySoundClip("Outdoor-Walking", true, "Player-Body"); //change with walking sounds
            
        }
        else 
        if((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) &&
           !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E)) && soundPlayer.isSourcePlaying("Player-Body"))
        {
            soundPlayer.StopSoundClip("Player-Body");
            Debug.Log("Sound.stopclip ThingaMaging was called");
        }

        transform.Rotate(Input.GetAxis("Rotate") * Vector3.up * translation * rotationSpeed); // Rotates on 'Q' & 'E' //Instructions to set up Rotate in the bottom:
        transform.Translate(Input.GetAxis("Horizontal") * Vector3.back * translation * movementSpeed); //Goes Left and Right
        transform.Translate(Input.GetAxis("Vertical") * Vector3.right * translation * movementSpeed); //Goes Fwd and Bck


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!coolDown) { StartCoroutine(Dash()); } else { Debug.Log("Cannot Dash while on CoolDown"); }
        }
        
        
	}

    IEnumerator Dash()
    {
        speedMultiplier = dashSpeedMultiplier;
        Debug.Log("Dash in progress");

        yield return new WaitForSeconds(dashDuration);
        speedMultiplier = 1;
        StartCoroutine(DashCoolDown(coolDownTime));

        // gameObject.transform.position = currentPlayerPos + new Vector3(x, 0, z); //Just teleports player

    }

    IEnumerator DashCoolDown(float secs) {
        Debug.Log("Dash-Cooldown Initiated");
        coolDown = true;
        yield return new WaitForSeconds(secs);
        coolDown = false;
    }
    /*
     *To set up the Rotate Axis, go to Edit->Project settings-> Input Manager->
     * On the Size tab right under 'Axes' add 1 more to whatever the number is, next go to the last Input/tab, should be a second "cancel"
     * Set it up as follows: 
     * Name: Rotate
     * Negative button: q
     * Positive button: e
     * Gravity: 3
     * Dead: 0.001
     * Sensitivity: 3
     * Type: Keys or Mouse button
     * Axis: x-Axis
     * Joy Num: Get Motion from all Joysticks
     * You can leave everything else blank.
     */
}


