using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielHammer : MonoBehaviour
{
    [SerializeField]
    private float knockbackForce;
    private Vector2 swingDirection;

    [SerializeField]
    private Transform hammer;

    [SerializeField]
    private HammerHitbox hammerHitboxPrefab;

    [SerializeField]
    private Transform hammerHitboxLocation;

    [SerializeField]
    private Animator anim;

    private bool hammerSwung = false;

    public MenuPause pauseMenuCheck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("HammerSwing"))
        {
            if (pauseMenuCheck.pauseStatus == false)
            {
                AimHammer();
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(pauseMenuCheck.pauseStatus == false)
            {
                SwingHammer();
            }
        }
    }

    private void AimHammer()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = mousePos - (Vector2)transform.position;
        hammer.transform.up = mouseDirection;
    }

    private void SwingHammer()
    {
        anim.Play("HammerSwing");
        swingDirection = (hammer.transform.up).normalized;
    }

    public void ToggleHammerSwung()
    {
        hammerSwung = !hammerSwung;
    }

    public void SpawnHammerHitbox()
    {
        HammerHitbox hitboxInstance = Instantiate(hammerHitboxPrefab, hammerHitboxLocation.position, hammerHitboxLocation.rotation);
        hitboxInstance.forceOfSwing = knockbackForce;
        hitboxInstance.directionOfSwing = swingDirection;
    }
}
