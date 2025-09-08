using UnityEngine;

public class SimpleAnimatorScript : MonoBehaviour
{
    private Animator animator;
    private bool isStopped;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isStopped)
            {
                animator.SetBool("Stop", true);
                isStopped = true;
            }
            else
            {
                animator.SetBool("Stop", false);
                isStopped = false;
            }
        }
        
        
    }
}
