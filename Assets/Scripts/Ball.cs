using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour{
    public Vector3 initialVelocity = new(2, 0, 1);
    private Vector3 velocity;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private int finalScore = 142;
    public AudioClip beep;
    public AudioClip win;
    public AudioClip lose;
    public GameObject collisionPowerup;
    public GameObject pedalPowerup;
    public GameObject slowPowerup;
    private long timeOfCollectionSpeed = 0;
    private bool collisions = true;
    private long timeOfCollectionCollision = 0;
    public GameObject[] hearts = new GameObject[3];
    private int health;


    void Start(){
        velocity = initialVelocity;
        health = hearts.Length - 1;
    }

    void FixedUpdate(){
        transform.position += velocity * Time.deltaTime;
        if(timeOfCollectionSpeed != 0 && System.DateTimeOffset.Now.ToUnixTimeSeconds() - timeOfCollectionSpeed > 6){
            velocity *= 2;
            timeOfCollectionSpeed = 0;
        }
        if(timeOfCollectionCollision != 0 && System.DateTimeOffset.Now.ToUnixTimeSeconds() - timeOfCollectionCollision > 6){
            collisions = true;
            timeOfCollectionCollision = 0;
        }
    }

    void OnTriggerEnter(Collider other){
        AudioSource.PlayClipAtPoint(beep, Vector3.zero);
        if(other.gameObject.CompareTag("Wall")){
            if (other.gameObject.name == "Left" || other.gameObject.name == "Right"){
                velocity.z *= -1;
            }
            else if (other.gameObject.name == "Top"){
                velocity.x *= -1;
            }
            else if (other.gameObject.name == "Bottom"){
                if (health > 0){
                    ResetBall();
                    health--;
                }else{
                    scoreText.text = "Game Over\nFinal Score: " + score;
                    ResetBall();
                    Destroy(gameObject);
                }
                AudioSource.PlayClipAtPoint(lose, Vector3.zero);
            }
        }
        else if(other.gameObject.name == "Pedal"){
            velocity.x *= -1;
            finalScore--;
        }
        else if (other.gameObject.name.Contains("Cube")){
            if (other.gameObject.GetComponent<Block>().isFortified){
                other.gameObject.GetComponent<Block>().removeArmor();
            }else{
                Destroy(other.gameObject);
                SpawnPowerup(other.gameObject.transform.position);
                score++;
                if (score == 42){
                    scoreText.text = "You Win!\nFinal Score: " + finalScore;
                    AudioSource.PlayClipAtPoint(win, Vector3.zero);
                    gameObject.SetActive(false);
                }else{
                    scoreText.text = "Score: " + score;
                }
            }
            if (collisions) velocity.x *= -1;
        }
    }

    private void ResetBall(){
        transform.position = new Vector3(-3.5f, 0, 0);
        velocity = initialVelocity;
        timeOfCollectionSpeed = 0;
        timeOfCollectionCollision = 0;
        collisions = true;
        hearts[health].SetActive(false);
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (GameObject powerup in powerups){
            Destroy(powerup);
        }
    }


    public void SpeedPowerup(){
        if (timeOfCollectionSpeed != 0) return;
        velocity /= 2;
        timeOfCollectionSpeed = System.DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public void CollisionPowerup(){
        collisions = false;
        timeOfCollectionCollision = System.DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    private void SpawnPowerup(Vector3 position){
        if(Random.Range(0, 100) < 20){
            Instantiate(slowPowerup, transform.position, Quaternion.identity);
        } else if(Random.Range(0, 100) < 20){
            Instantiate(pedalPowerup, transform.position, Quaternion.identity);
        } else if(Random.Range(0, 100) < 20){
            Instantiate(collisionPowerup, transform.position, Quaternion.identity);
        }
    }
}

