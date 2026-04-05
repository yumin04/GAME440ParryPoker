using UnityEngine;

public class PlayerForTrailer : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        // anim.SetTrigger("Hit");
    }
}