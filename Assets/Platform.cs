using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private int jumps = 1;
    public Animator anim;
    public Collider2D coll;

    public void JumpedOn()
    {
        jumps -= 1;
        if (jumps <= 0)
        {
            FadeAwayPlatform();
        }
    }

    private void FadeAwayPlatform()
    {
        coll.enabled = false;
        anim.SetBool("fade", true);
    }

    public void DestroyPlatform()
    {
        Destroy(this.gameObject);
    }
}
