using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region VARIABLES

    #region PRIVATE
    Rigidbody rb { get { return GetComponent<Rigidbody>(); } } // JRH v0.0.11
    CapsuleCollider collider {get {return GetComponent<CapsuleCollider>(); }} // JRH v0.3.6
    Animator anim { get { return GetComponent<Animator>(); } }    // JRH v0.3.6

    [SerializeField]
    Renderer renderer;

    public Vector3 jumpVelocity { get { return new Vector3(0f, Mathf.Sqrt(Mathf.Abs(Physics.gravity.y) * GameManager.Instance.JumpHeight * 2), 0f); } } // JRH v0.0.11
    float actionTime { get { return 1f / (GameManager.Instance.GameSpeed * 1.5f); } } // JRH v0.0.14

    bool isFalling, isSliding, isMoving;     // JRH v0.0.14
    bool isStanding;

    int playerChances = 3;
    bool isRecovering = false;

    PlayerAction? queuedAction;
    Vector3 queueDir;
    #endregion

    #region PUBLIC
    public Rigidbody RB { get { return rb; } }     // JRH v0.0.13

    public PlayerState State     // JRH v0.0.14
    {
        get
        {
            if (isFalling) return PlayerState.Falling;
            if (isSliding) return PlayerState.Sliding;
            if (isMoving) return PlayerState.Moving;
            return PlayerState.Standing;
        }
    }

    int PlayerChances { get { return Mathf.Clamp(playerChances, 0, 3); } }
    public bool IsRecovering { get { return isRecovering; } }
    #endregion

    #endregion

    #region METHODS

    #region USER-DEFINED
    // JRH v0.0.11
    /// <summary>
    /// Called in Awake, sets up initial variables 
    /// </summary>
    void Initialise()
    {
        // JRH v0.0.11 nothing here yet
    }

    // JRH v0.0.11
    /// <summary>
    /// Called in InputManager, Applies a force to the RigidBody to allow the player to Jump if it is allowed
    /// </summary>
    public void Jump()
    {
        if (!isFalling && !isMoving)
        {
            queuedAction = null;

            rb.velocity = Vector3.zero;
            rb.AddForce(jumpVelocity, ForceMode.VelocityChange);
            isFalling = true;

            anim.SetTrigger("Jump");
            AudioManager.Instance.PlaySFX("JumpSFX");
            // JRH v0.2.4: Play the jumping sound effect
        }
        else queuedAction = PlayerAction.Jump;
    }

    // JRH v0.0.11
    /// <summary>
    /// Called on OnCollisionEnter, disables gravity to keep the player y position at a constant and enable jumping
    /// </summary>
    public void Land()
    {
        if (isFalling) AudioManager.Instance.PlaySFX("LandSFX");
        // JRH v0.2.4: Play the landing sound effect

        isFalling = false;  // JRH v0.1.7 - Fixing physics issues

        anim.SetTrigger("Recover");

        DoQueuedAction();
    }

    // JRH v0.0.11
    /// <summary>
    /// Called on OnCollisionExit, enables gravity to make the player fall
    /// </summary>
    void Fall()
    {
        rb.useGravity = true;
        // JRH v0.0.11 - turns on gravity so the player starts falling
        isFalling = true;
        // JRH v0.1.7 - Fixing physics issues
    }

    // JRH v0.2.11:
    /// <summary>
    /// Checks to see if the player can slide, makes the player slide if so and otherwise queues a slide for the next action
    /// </summary>
    public void Slide()
    {
        if (State == PlayerState.Standing)
        {
            StartCoroutine(SlideCoroutine());
        }
        else queuedAction = PlayerAction.Slide;
    }

    // JRH v0.0.13
    /// <summary>
    /// Called by InputManager, allows the player to slide for as long as slideTime after which the player reverts to standing
    /// </summary>
    /// <returns></returns>
    public IEnumerator SlideCoroutine()
    {
        if (queuedAction != null) queuedAction = null;
        isSliding = true;
        anim.SetTrigger("Slide");
        AudioManager.Instance.PlaySFX("SlideSFX");
        collider.direction = 2;
        collider.height = 1.5f;
        renderer.transform.parent.Translate(0f, -0.75f, 0f);

        collider.center = Vector3.down * collider.radius;
        yield return new WaitForSeconds(GameManager.Instance.LaneDistance * 1.5f / GameManager.Instance.GameSpeed);
        Stand();
    }

    // JRH v0.0.13
    /// <summary>
    /// Reverts to the player to standing after sliding
    /// </summary>
    void Stand()
    {
        GetComponent<CapsuleCollider>().direction = 1;
        GetComponent<CapsuleCollider>().center = Vector3.zero;

        collider.direction = 1;
        collider.center = Vector3.zero;
        collider.height = 2;
        renderer.transform.parent.Translate(0f, 0.75f, 0f);

        anim.SetTrigger("Recover");

        transform.position += Vector3.up * 0.5f;
        isSliding = false;

        DoQueuedAction();
    }

    // JRH v0.2.11
    /// <summary>
    /// Void method that starts the SwitchLane coroutine so long as the conditions are so, otherwise queues it until the conditions are
    /// </summary>
    public void Move()
    {
        if (Input.GetKeyDown(KeyCode.A)) queueDir = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D)) queueDir = Vector3.right;

        if (State == PlayerState.Standing && queueDir != Vector3.zero) StartCoroutine(SwitchLane(queueDir));
        else queuedAction = PlayerAction.Move;
    }

    // JRH v0.0.14
    /// <summary>
    /// Called by InputManager, allows the player to slide into the next lane position
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    IEnumerator SwitchLane(Vector3 dir)
    {
        // JRH v0.2.11: Set the conditions for having started the moving procedure
        if (queuedAction == PlayerAction.Move) queuedAction = null;
        isMoving = true;
        Vector3 initialPos = transform.position;
        float nextX = Mathf.Clamp(initialPos.x + (Mathf.Sign(dir.x) * GameManager.Instance.LaneDistance), 0 - GameManager.Instance.LaneDistance, 0 + GameManager.Instance.LaneDistance);
        float t = 0f;

        // JRH v0.3.6: Set animation bools
        string animString = "Move";
        switch ((int)dir.x) { case -1: animString += "Left"; break; case 1: animString += "Right"; break; }
        //Debug.Log(animString);


        anim.SetTrigger(animString);

        // JRH v0.2.11: Begin the moving procedure
        while (t < 1)
        {
            t += Time.deltaTime / (1 / GameManager.Instance.GameSpeed);
            transform.position = new Vector3(Mathf.Lerp(initialPos.x, nextX, t), initialPos.y, initialPos.z);
            yield return null;
        }

        // JRH v0.2.11: Termiate the moving procedure
        transform.position = new Vector3(nextX, initialPos.y, initialPos.z);
        isMoving = false;
        anim.SetBool(animString, false);

        Land();
    }

    void DoQueuedAction()
    {
        switch (queuedAction)
        {
            case PlayerAction.Jump: if (State == PlayerState.Standing) Jump(); break;
            case PlayerAction.Move: if (State == PlayerState.Standing) StartCoroutine(SwitchLane(queueDir)); break;
            case PlayerAction.Slide: if (State == PlayerState.Standing) StartCoroutine(SlideCoroutine()); break;
            default: queuedAction = null; break;
        }
    }

    public IEnumerator FlashColor(Color artColor)
    {
        float t = 0;
        Color initColor = renderer.material.GetColor("_OutlineColor");
        float initLine = renderer.material.GetFloat("_Outline");

        while (t < 1)
        {
            renderer.material.SetFloat("_Outline", Mathf.Lerp(initLine * 5, initLine, t));
            renderer.material.SetColor("_OutlineColor", Color.Lerp(artColor, initColor, t));
            yield return null;
            t += Time.deltaTime;
        }
        renderer.material.SetColor("_OutlineColor", initColor);
        renderer.material.SetFloat("_Outline", initLine);
    }

    public void Damage()
    {
        anim.SetTrigger("Injury");

        if (playerChances - 1 > 0)
        {
            StartCoroutine(TakeDamage());
            GameManager.Instance.StaggerGamespeed();
            GameObject.Find("Boulder").GetComponent<Boulder>().RollForward();
            // JRH v0.2.3: Move the boulder along closer. Just a lil. Just enough to make the player feel uneasy.
        }
        else
        {
            anim.SetTrigger("Death");
            GameManager.Instance.EndGame();
            //gameObject.SetActive(false);
        }
    }

    IEnumerator TakeDamage()
    {
        //float tI = 0;
        float tMax = GameManager.Instance.RecoveryTime;
        //Renderer renderer = gameObject.GetComponent<Renderer>();
        Color cI = renderer.material.GetColor("_OutlineColor");

        // JRH v0.2.1: Harm the player. Make them bleed.
        playerChances--;
        isRecovering = true;
        AudioManager.Instance.PlaySFX("DamageSFX");

        // JRH v0.2.2: Show the player that they're bleeding and give them some time to stop doing that, you butt
        renderer.material.SetColor("_OutlineColor", Color.red) ;
        yield return new WaitForSeconds(tMax);

        // JRH v0.2.1: Okay, the player has stopped bleeding. Supposedly. You can harm them again.
        isRecovering = false;
        renderer.material.SetColor("_OutlineColor", cI);
        yield return null;
    }

    // todo: Reincorporate this into the main loop for optimisation
    public void GainChance()
    {
        playerChances = Mathf.Clamp(playerChances + 1, 0, 3);
    }
    #endregion

    #region MONOBEHAVIOUR
    void Awake()
    {
        Initialise();     // JRH v0.0.1
    }

    void Update()
    {
        if (transform.position.y <= -30f)   // JRH v0.1.3: disables the player once out of FOV
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Platform" && isFalling) Land();    // JRH v0.1.7
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {
            if (other.GetComponent<Interactable>().GetType() == typeof(Reward)) other.GetComponent<Interactable>().Interact();
            else if (!isRecovering) other.GetComponent<Interactable>().Interact();
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (!Physics.Raycast(transform.position, Vector3.down)) Fall();
    }
    #endregion

    #endregion
}

public enum PlayerState
{
    Standing, Falling, Jumping, Moving, Sliding
}

public enum PlayerAction
{
    Jump, Move, Slide
}