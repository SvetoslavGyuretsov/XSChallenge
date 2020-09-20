using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //variables 
    public static AudioClip collectApple, die;
    static AudioSource audioSource;

    [SerializeField] private AudioClip eat = null;
    [SerializeField] private AudioClip death = null;
    [SerializeField] private AudioSource source = null;
    
    // Start is called before the first frame update
    void Start()
    {
        //assign the static fields to be equal to the ones that can be easily set up in the Inspector
        collectApple = eat;
        die = death;
        audioSource = source;
    }

    //Public Static method that can be easily used in other classes when needed
    //The method has a String parameter
    public static void PlaySound(string clip)
    {

        switch (clip)
        {
            case "eat":
                audioSource.PlayOneShot(collectApple);
                break;
            case "die":
                audioSource.PlayOneShot(die);
                break;
        }
    }
}
