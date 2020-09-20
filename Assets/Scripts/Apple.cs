/*Check if the Apple is colliding with the Wall objects 
which expand in order to prevent from spawning outside of screen.*/
using UnityEngine;

public class Apple : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
