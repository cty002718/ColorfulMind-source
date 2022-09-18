using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType 
    {
        Heart_left,
        Heart_right,
        Heart_down,
        Heart,
        Shovel,
        White,
        Cross,
        Prescription,
        Paper,
        Petal,
        Key,
    }

    public ItemType itemType;
    public int amount;
    public string description;
    public ItemController itemController;

    public static void AddItem(ItemType _itemType, string _description = "") {
        Item item = new Item();
        item.itemType = _itemType;
        item.amount = 1;
        item.description = _description;
        item.SetController();
        HeroController.instance.inventory.AddItem(item);
    }

    public void SetController() {
        switch(itemType) {
            case ItemType.Shovel: itemController = new ShovelController(); break;
            case ItemType.White: itemController = new WhiteController(); break;
            case ItemType.Cross: itemController = new CrossController(); break;
            case ItemType.Prescription: itemController = new PrescriptionController(); break;
            case ItemType.Paper: itemController = new PaperController(); break;
            case ItemType.Key: itemController = new KeyController(); break;
            default: itemController = null; break;
        }
    }

    public string GetDialoguePath() {
        switch(itemType) {
            default:
            case ItemType.Shovel: return ItemAssets.Instance.shovelDialoguePath;
            case ItemType.Petal: return ItemAssets.Instance.petalDialoguePath;
        }
    }
    public Sprite GetSprite() {
        switch(itemType)
        {
            default:  
            case ItemType.Heart: return ItemAssets.Instance.heartSprite;
            case ItemType.Heart_left: return ItemAssets.Instance.heart_left_Sprite;
            case ItemType.Heart_right: return ItemAssets.Instance.heart_right_Sprite;
            case ItemType.Heart_down: return ItemAssets.Instance.heart_down_Sprite; 
            case ItemType.Shovel: return ItemAssets.Instance.shovelSprite;
            case ItemType.White: return ItemAssets.Instance.whiteSprite;
            case ItemType.Cross: return ItemAssets.Instance.crossSprite;
            case ItemType.Prescription: return ItemAssets.Instance.prescriptionSprite;
            case ItemType.Paper: return ItemAssets.Instance.paperSprite;
            case ItemType.Petal: return ItemAssets.Instance.petalSprite;
            case ItemType.Key: return ItemAssets.Instance.keySprite;
        }
    }

    public bool IsStackable()
    {
        switch(itemType)
        {
            default:
                return true;
        }
    }
}
