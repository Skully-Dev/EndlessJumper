using UnityEngine;

public class Platform : MonoBehaviour
{
    [Tooltip("Number of jumps before fade away")]
    private int jumps = 1;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Collider2D coll;

    /// <summary>
    /// Reduces jump remaining count by 1, if 0, platform fades away.
    /// </summary>
    public void JumpedOn()
    {
        jumps -= 1;
        if (jumps <= 0)
        {
            FadeAwayPlatform();
        }
    }

    /// <summary>
    /// Disables collider, fades alpha to 0, destroyed on faded.
    /// </summary>
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
