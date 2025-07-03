using UnityEngine;
using System.Collections.Generic;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [Header("Chest Settings")]
    public bool isOpened = false;

    [Header("Chest Contents")]
    public List<ChestItem> chestItems = new List<ChestItem>();

    [Header("Animation")]
    public Animator chestAnimator;
    public string openAnimationTrigger = "Open";

    /*    [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip openSound;


        [Header("Visual Effects")]
        public GameObject itemSpawnPoint;
        public ParticleSystem openEffect;
    */

    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = Object.FindFirstObjectByType<InventoryManager>();

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager nicht gefunden!");
        }

        if (chestAnimator != null && isOpened)
        {
            chestAnimator.SetTrigger(openAnimationTrigger);
        }
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpened;
    }

    public string GetInteractionText(PlayerInteraction player)
    {
        return isOpened ? "" : "[E] Truhe öffnen";
    }

    public void Interact(PlayerInteraction player)
    {
        if (isOpened) return;
        OpenChest();
    }

    private void OpenChest()
    {
        isOpened = true;

        if (chestAnimator != null)
        {
            chestAnimator.SetTrigger(openAnimationTrigger);
        }

        /*        PlaySound(openSound);

                if (openEffect != null)
                {
                    openEffect.Play();
                }
        */

        CollectItems();

        UIManager.Instance.ShowMessage($"Truhe geöffnet! {chestItems.Count} Gegenstände gefunden!");
    }

    private void CollectItems()
    {
        if (inventoryManager == null) return;

        int totalValue = 0;

        foreach (ChestItem item in chestItems)
        {
            inventoryManager.AddItem(item.itemType, item.itemName, item.pointValue);
            totalValue += item.pointValue;

         /*   if (itemSpawnPoint != null)
            {
                StartCoroutine(SpawnItemVisual(item));
            }
         */
        }

        if (totalValue > 0)
        {
            UIManager.Instance.AddScore(totalValue);
        }
    }

    /*    private System.Collections.IEnumerator SpawnItemVisual(ChestItem item)
        {
            yield return new WaitForSeconds(0.5f);

            GameObject itemObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            itemObject.transform.position = itemSpawnPoint.transform.position;
            itemObject.transform.localScale = Vector3.one * 0.3f;

            Renderer renderer = itemObject.GetComponent<Renderer>();
            switch (item.itemType)
            {
                case ItemType.Treasure:
                    renderer.material.color = Color.yellow;
                    break;
                case ItemType.Collectible:
                    renderer.material.color = Color.blue;
                    break;
                default:
                    renderer.material.color = Color.gray;
                    break;
            }

            Vector3 startPos = itemObject.transform.position;
            Vector3 endPos = startPos + Vector3.up * 2f;

            float duration = 2f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                itemObject.transform.position = Vector3.Lerp(startPos, endPos, t);

                Color color = renderer.material.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                renderer.material.color = color;

                yield return null;
            }

            Destroy(itemObject);
        }
    */

    /*   private void PlaySound(AudioClip clip)
       {
           if (audioSource != null && clip != null)
           {
               audioSource.PlayOneShot(clip);
           }
       }
    */

    [ContextMenu("Reset Chest")]
    public void ResetChest()
    {
        isOpened = false;
        if (chestAnimator != null)
        {
            chestAnimator.SetTrigger("Close");
        }
    }
}
