using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetPrefab : MonoBehaviour
{
    public int direction = 0;
    public int speed = 20;
    public Vector3 vector;
    public int threshold = 5;
    private Vector3 startPosition;
    public int minRange = 10;
    public int maxRange = 20;
    public static bool first = true;
    public int minYRange = 0;
    public int maxYRange = 10;
    public int lifetime = 30;
    private bool hovering = false;
    // Start is called before the first frame update
    [SerializeField] private InputActionAsset actionAsset;
    void Start()
    {
        if (first) {
            first = false;
            changeDirection(0);
        } else {
            transform.position = randomVector();
            changeDirection(Random.Range(0, 3));
            // transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.down, Camera.main.transform.rotation * Vector3.back);
        }
        startPosition = transform.position;
        speed = Random.Range(speed/4, speed);
        var activate = actionAsset.FindActionMap("XRI RightHand").FindAction("Activate");
        activate.Enable();
        activate.performed += OnActivate;
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (System.Math.Abs((startPosition - transform.position)[direction]) > threshold) {
            vector = -1 * vector;
        }
        transform.Translate(vector * speed * Time.deltaTime); 
    }

    public void changeDirection(int direction){
        this.direction = direction;
        vector = getVector(direction);
    }

    Vector3 getVector(int direction){
        switch (direction)
        {
            case 0:
                return Vector3.right;
            case 2:
                return Vector3.forward;
            case 1:
                return Vector3.up;
            default:
                return Vector3.right;
        }
    }

    private int randomRange(int min, int max) { return Random.Range(min, max); }

    private Vector3 randomVector() { return new Vector3(randomRange(minRange, maxRange), randomRange(minYRange, maxYRange), randomRange(minRange, maxRange)); }

    public void Hovering() {
        hovering = true;
    }

    public void ExitHovering() {
        hovering = false;
    }

    void OnActivate(InputAction.CallbackContext context){
        if (hovering) {
            Destroy(this.gameObject);
            Camera.main.GetComponent<MainCamera>().hit();
        }
    }
}
