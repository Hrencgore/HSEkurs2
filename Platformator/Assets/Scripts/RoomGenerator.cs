using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private RoomVariants variants;
    private bool spawned = false;
    private int rand;
    private float waitTime = 3f;

    public Direction direction;
    public enum Direction{
        Top,
        Right,
        Left,
        Bottom,
        None
    }

   private void Start() {
    variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    Destroy(gameObject, waitTime);
    Invoke("Spawn", 0.2f);
   }

   public void Spawn() {
    if(!spawned) {
        if (direction == Direction.Top ){
            rand = Random.Range(0, variants.topRooms.Count);
            Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation);
        } else if (direction == Direction.Right) {
            rand = Random.Range(0, variants.rightRooms.Count);
            Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
        } else if (direction == Direction.Left) {
            rand = Random.Range(0, variants.leftRooms.Count);
            Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);
        } else if (direction == Direction.Bottom) {
            rand = Random.Range(0, variants.bottomRooms.Count);
            Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
        }
        spawned = true;
    }
   }

   private void OnTriggerStay2D(Collider2D other) {
    if (other.CompareTag("RoomPoint") && other.GetComponent<RoomGenerator>().spawned){
        Destroy(other.gameObject);
    }
   }
}
