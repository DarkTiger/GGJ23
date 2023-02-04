using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed = 0.1f;
    [SerializeField] float jumpForce = 1f;
    [SerializeField] Material[] materials;

    public InputAction MoveAction { get; private set; }
    public InputAction VisualAction { get; private set; }
    public InputAction JumpAction { get; private set; }

    PlayerInput playerInput;
    MeshRenderer meshRenderer;
    Rigidbody rb;
    bool isGrounded = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        playerInput = GetComponent<PlayerInput>();
        MoveAction = playerInput.actions["Movement"];
        VisualAction = playerInput.actions["Visual"];
        JumpAction = playerInput.actions["Jump"];
    }

    void Start()
    {
        if (FindObjectsByType<Player>(FindObjectsSortMode.None).Length > 1)
        {
            transform.position = new Vector3(18.65f, 2.83f, -3f);
            meshRenderer.material = materials[0];
        }
        else
        {
            transform.position = new Vector3(-18.65f, 2.83f, -3f);
            meshRenderer.material = materials[1];
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = MoveAction.ReadValue<Vector2>() * movementSpeed;       
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.y);

        Vector3 rbVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (MoveAction.ReadValue<Vector2>().magnitude > 0.25f)
        {
            transform.forward = Vector3.Lerp(transform.forward, rbVelocity.normalized, 1f);
        }
    }

    private void Update()
    {
        if (JumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
