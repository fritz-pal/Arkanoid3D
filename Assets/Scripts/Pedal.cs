using UnityEngine;
using UnityEngine.InputSystem;

public class Pedal : MonoBehaviour
{
    public float speed = 10.0f;
    public float maxZ = 10.0f;
    public float minZ = -10.0f;
    public GameObject ball;
    public AudioClip powerup;
    private float dir = 0.0f;
    private long timeOfCollection = 0;

    void Update()
    { 
        transform.position += speed * Time.deltaTime * new Vector3(0, 0, -dir);

        float subtract = timeOfCollection != 0 ? 0.75f : 0;
        float clampedZ = Mathf.Clamp(transform.position.z, minZ + subtract, maxZ - subtract);
        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);

        if(timeOfCollection != 0 && System.DateTimeOffset.Now.ToUnixTimeSeconds() - timeOfCollection > 8){
            transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z / 2);
            timeOfCollection = 0;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        dir = input.x;
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Powerup")){
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(powerup, Vector3.zero);

            if(other.gameObject.name.Contains("Slow")){
                if (ball == null) return;
                    ball.GetComponent<Ball>().SpeedPowerup();
            }
            else if(other.gameObject.name.Contains("Paddle")){
                if (ball == null || timeOfCollection != 0) return;
                transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z * 2);
                timeOfCollection = System.DateTimeOffset.Now.ToUnixTimeSeconds();
            }
            else if(other.gameObject.name.Contains("Collision")){
                if (ball == null) return;
                ball.GetComponent<Ball>().CollisionPowerup();
            }
        }
    }
}
