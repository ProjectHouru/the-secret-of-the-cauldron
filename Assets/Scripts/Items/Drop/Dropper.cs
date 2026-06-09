using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dropper: MonoBehaviour
{
    [SerializeField] private GameObject _dropContainerPrefab;
    [SerializeField] private float _dropForce;

    public void DropManyItems(List<ItemType> itemsTypes)
    {
        Debug.Log("Drop " + itemsTypes.Count + " elements");
        
        StartCoroutine(DropItemsCoroutine(itemsTypes));
    }

    public void DropOneItem(ItemType itemType)
    {
        DropItem(itemType);
        
        DestroyObject();
    }
    
    private IEnumerator DropItemsCoroutine(List<ItemType> itemsTypes)
    {
        foreach (var itemType  in itemsTypes)
        {
            Debug.Log("Drop " + itemType);
            
            DropItem(itemType);

            yield return new WaitForSeconds(0.2f);
        }

        DestroyObject();
    }
    
    private void DropItem(ItemType itemType)
    {
        var itemPrefab = ItemsBase.Instance.GetItemPrefab(itemType);

        if (itemPrefab)
        {
            var droppedItem = CreateDroppedItem(itemPrefab);

            AddForce(droppedItem);
            
            // Sound
            SoundManager.Instance.PlaySound(SoundType.ItemDrop);
        }
    }

    private GameObject CreateDroppedItem(GameObject itemPrefab)
    {
        var dropContainer = Instantiate(_dropContainerPrefab, transform.position, Quaternion.identity);
        var droppableItem = dropContainer.GetComponent<DropItemContainer>();
        
        droppableItem.AddInnerItem(itemPrefab);

        return dropContainer;
    }

    private void AddForce(GameObject droppedItem)
    {
        var rb = droppedItem.GetComponent<Rigidbody2D>();

        var direction = new Vector2(Random.Range(-2f, 2f) * _dropForce, Random.Range(-2f, 2f) * _dropForce);
        
        rb.AddForce(direction, ForceMode2D.Impulse);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}