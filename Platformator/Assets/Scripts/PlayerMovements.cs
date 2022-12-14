using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
  
    Transform playerCoords;
    Rigidbody2D playerRb;

    private float vForce = 500f/*Мб поменять название*/, hForce = 2f, plSpeed = 6f;
    private float hDirection = 0f, wallDirIndex = -0.1f;
    private float wallCheckDist = 2f;
    private bool canMove, canJump, isOnWall;

    private void Start() {
        playerCoords = GetComponent<Transform>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (canJump){
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)){
                Jump();
                canJump = false;
            } else if (isOnWall){
                //Могут возникать проблемы, если на карте будут соприкасаться платформы и стены. Потенциально - поменять
                if (playerRb.velocity.y < plSpeed){
                    playerRb.velocity = new Vector2(playerRb.velocity.x, wallDirIndex*plSpeed);
                }
            }
        }
        hDirection = Input.GetAxisRaw("Horizontal") * plSpeed;
        
       
        /*Оставить, как альтернативную физику движения, для другого персонажа или класса, например
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            StrafeToTheRight();
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            StrafeToTheLeft();
        }
        */

        if (Input.GetKey(KeyCode.F)){
            playerCoords.localPosition = Vector3.Lerp(playerCoords.localPosition, 
            new Vector3(0, 1f,0), 5f);
        }
    }

    private void FixedUpdate() {
        Physics2D.queriesStartInColliders = false;
        if (Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallCheckDist).collider != null && hDirection == -1) {
            playerRb.velocity = new Vector2(hDirection, playerRb.velocity.y);
        } else if (Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, wallCheckDist).collider != null && hDirection == 1) {
            playerRb.velocity = new Vector2(hDirection, playerRb.velocity.y);
        } else if (Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, wallCheckDist).collider == null && Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallCheckDist).collider == null) {
            playerRb.velocity = new Vector2(hDirection, playerRb.velocity.y);
        }

    }

    private void Jump(){
        playerRb.AddForce(playerCoords.up * vForce);
    }

    private void StrafeToTheRight(){
        playerRb.AddForce(playerCoords.right * hForce);
    }

    private void StrafeToTheLeft(){
        playerRb.AddForce(playerCoords.right * -1 * hForce);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ground" && (!canMove || !canJump)){
            canJump = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Wall"){
            canJump = true;
            isOnWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Wall"){
            isOnWall = false;
        }
    }
}

//Идея: сделать зацеп, когда игрок касается вертикальной части платформы
