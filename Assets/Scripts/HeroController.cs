﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RemptyTool.ES_MessageSystem;
using ColorSystem;
using UnityEngine.EventSystems;

public class HeroController : MonoBehaviour
{
    public static HeroController instance;

    private ColorType ct = ColorType.Null;

    // health system
    public int maxHealth = 5;
    public int health { get { return currentHealth; } } // get and set method for health
    int currentHealth;

    // hero speed
    public float heroSpeed;

    // invincible system
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    // animation system
    public Animator animator;
    public Vector2 lookDirection = new Vector2(0, -1);

    // physic system
    public Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    // Inventory system
    public Inventory inventory;
    public bool inventory_open = false;
    [SerializeField] UI_Inventory uiInventory;

    public bool isGameStop = false;

    #region Game Loop
    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        // adjust frame number
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // getComponets
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();

        currentHealth = maxHealth;
        //currentHealth = 1;

        Inventory.instance = new Inventory();
        inventory = Inventory.instance;
        if (uiInventory)
            uiInventory.SetInventory(inventory);

        //Item.AddItem(Item.ItemType.Key, "不知道是打開什麼的鑰匙");
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStop) return;
        // input detection
        GetInput();
        // communication with animator
        //ManageAmination();
        // invincible timer
        ManageInvinciale();
        // raycasting to detect object
        TriggerDialog();
        ShowInventory();

        //TestAnimation();
    }

    // Update is called via physics system
    void FixedUpdate()
    {
        if (isGameStop) { return; }
        Move();
        // communication with animator
        ManageAmination();
    }

    #endregion

    //functions
    #region Private functions
    private void TestAnimation() {
        if (Input.GetKey(KeyCode.C)) {
            animator.Play("Base Layer.walk_up");
        }

    }

    private void GetInput()
    {
        if (!inventory_open && !ES_MessageSystem.instance.IsDoingTextTask && !Portal.IsTransporting)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            UseColor();
        }
        else
            horizontal = vertical = 0;

        if(Input.GetKey(KeyCode.LeftShift)) heroSpeed = 9;
        else heroSpeed = 3;
    }

    private void Move()
    {
        if (ES_MessageSystem.instance.IsDoingTextTask || inventory_open || Portal.IsTransporting) return;
        Vector2 position = rigidbody2d.position;
        position.x += heroSpeed * horizontal * Time.deltaTime;
        position.y += heroSpeed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
        //gameObject.transform.position = position;
    }

    private void TriggerDialog()
    {
        if (!inventory_open && !Portal.IsTransporting && Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 0.5f, LayerMask.GetMask("npc") | LayerMask.GetMask("object"));
            if (hit.collider != null)
            {
                Animator npc_animator = hit.collider.GetComponent<Animator>();
                if(npc_animator != null) {
                    npc_animator.SetFloat("Look X", -lookDirection.x);
                    npc_animator.SetFloat("Look Y", -lookDirection.y);
                }
                NewDialogueTrigger dialogueTrigger = hit.collider.GetComponent<NewDialogueTrigger>();
                if (dialogueTrigger != null)
                {
                    dialogueTrigger.TriggerDialogue();
                }
            }
        }
    }

    public void ShowInventory()
    {
        if (!ES_MessageSystem.instance.IsDoingTextTask && !Portal.IsTransporting && Input.GetKeyDown(KeyCode.X))
        {
            if (inventory_open)
            {
                uiInventory.gameObject.SetActive(false);
                inventory_open = false;
            }
            else
            {
                //uiInventory.SelectFirstButton();
                uiInventory.gameObject.SetActive(true);
                uiInventory.SelectFirstButton();
                inventory_open = true;
            }
        }
    }

    private void ManageAmination()
    {
        Vector2 move = new Vector2(horizontal, vertical);
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
    }

    private void ManageInvinciale()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }

    private void UseColor()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) { Debug.Log("On UI."); return; }
            Vector2 vLook2Dir = lookDirection;
            Vector2 vTargetDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Vector2.Angle(vLook2Dir, vTargetDir);
            //Debug.Log(angle);
            if (angle > 60) return;
            Vector2 vOrigion = transform.position;
            RaycastHit2D ri = Physics2D.Raycast(vOrigion, vTargetDir, 1f, 1 << LayerMask.NameToLayer("object"));
            if (ri)
            {
                ColorObject co = ri.collider.GetComponent<ColorObject>();
                if (co) { co.SetColor(ct); }
            }
        }
    }

    #endregion

    #region Public functions
    public void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            if (isInvincible) return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    public void SelectColor(ColorType _ct)
    {
        ct = _ct;
    }

    #endregion

    #region Events
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("portal"))
        {
            Portal p = collision.gameObject.GetComponent<Portal>();
            p.Transport(transform);
        }
    }
    
    #endregion
}
