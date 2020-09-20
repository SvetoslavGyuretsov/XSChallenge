using UnityEngine;
using System;

public class Snake : MonoBehaviour
{
    private Snake next;

    static public Action<String> hit;
    public void SetNext(Snake In)
    {
        next = In;
    }

    public Snake GetNext()
    {
        return next;
    }

    public void RemoveTail()
    {
        Destroy(gameObject);
    }
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        hit?.Invoke(other.tag);
        if (other.CompareTag("Apple"))
        {
            Destroy(other.gameObject);
        }
        
    }
}
