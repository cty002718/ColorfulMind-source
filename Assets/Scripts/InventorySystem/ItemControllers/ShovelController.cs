using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShovelController : ItemController
{
    Sprite alternativeSprite;
    GameObject cross;

    public void UseItem() {
        Debug.Log("use shovel");
    	RaycastHit2D hit = Physics2D.Raycast(HeroController.instance.GetComponent<Rigidbody2D>().position + Vector2.up * 0.2f, HeroController.instance.lookDirection, 0.5f, LayerMask.GetMask("npc") | LayerMask.GetMask("object"));
        if (hit.collider != null && hit.collider.tag == "cross")
        {
            if(QuestManager.instance.CheckIfComplete("UseShovel") == true) return;
            NewDialogueTrigger dialogueTrigger = hit.collider.GetComponent<NewDialogueTrigger>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Pictures/Church/Grave");
            cross = Resources.Load<GameObject>("Picture/cross");

            foreach (Sprite s in sprites) { if (s.name == "Grave_3") alternativeSprite = s; }
            
            if (dialogueTrigger != null)
            {
                UI_Inventory.instance.gameObject.SetActive(false);
                HeroController.instance.inventory_open = false;
                //HeroController.instance.dialogue_open = true;
                dialogueTrigger.dialoguePath = "Level1/useShovel";
                dialogueTrigger.TriggerDialogue();
            }
            SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
            if (sr) { sr.sprite = alternativeSprite; }
            GameObject go = GameObject.Instantiate(cross);
            go.transform.parent = hit.collider.transform;
            go.transform.localPosition = Vector3.zero + new Vector3(0, 0.1f, 0);
            go.transform.Rotate(new Vector3(0, 0, -30));
            go.GetComponent<SpriteRenderer>().material.SetFloat("_Saturation", 0f);
        }
    }
}
