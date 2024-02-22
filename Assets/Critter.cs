
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.iOS;
using UnityEngine;

public class Critter : MonoBehaviour
{
    public int speed;
    public int sense;
    public int breed;
    public int move_speedX = 5;
    public int move_speedY = 5;
    public int[] directions = {0,1,2,3};
    public int val = 0;
    public int x_dir = 1;
    public int y_dir = 1;
    // Start is called before the first frame update
    void Start()
    {
        // double size = speed + sense + breed;
        // float y_hitbox = (float) Math.Ceiling(size %3);
        // gameObject.GetComponent<BoxCollider2D>().size = new Vector3(3, y_hitbox, 1);

        x_dir = directions[UnityEngine.Random.Range(0,directions.Length)];
        // multiply move speed by delta time, so that the pipe moves the same speed for every frame rate

        y_dir = directions[UnityEngine.Random.Range(0,directions.Length)];
        // multiply move speed by delta time, so that the pipe moves the same speed for every frame rate

        move_speedX = UnityEngine.Random.Range(3,6);
        move_speedY = UnityEngine.Random.Range(3,6);
        
    }

    // Update is called once per frame
    void Update()
    {
        // switch(val){
        //     case(0):
        //         transform.position = transform.position + Vector3.left * move_speedX * Time.deltaTime;
        //         break;
        //     case(1):
        //         transform.position = transform.position + Vector3.right * move_speedX * Time.deltaTime;
        //         break;
        //     case(2):
        //         transform.position = transform.position + Vector3.up * move_speedY * Time.deltaTime;
        //         break;
        //     case(3):
        //         transform.position = transform.position + Vector3.down * move_speedY * Time.deltaTime;
        //         break;
            
        // }
        
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("collision");
        val = directions[UnityEngine.Random.Range(0,directions.Length)];
        // multiply move speed by delta time, so that the pipe moves the same speed for every frame rate

        // y_dir = directions[UnityEngine.Random.Range(0,directions.Length)];
        // // multiply move speed by delta time, so that the pipe moves the same speed for every frame rate
        // move_speedX = UnityEngine.Random.Range(3,6);
        // move_speedY = UnityEngine.Random.Range(3,6);
    }
}
