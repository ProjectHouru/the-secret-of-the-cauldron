using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropSpawner: MonoBehaviour
{
     [SerializeField] private Transform _dropPoint;
     [SerializeField] private Dropper _dropperPrefab;
     [SerializeField] private List<ItemType> _dropItems;
     [SerializeField] private DropMode _dropMode;

     public void Spawn()
     {
          var dropper = CreateDropper();
          
          switch (_dropMode)
          {
               case DropMode.All:
                    SpawnAll(dropper);
                    break;
               case DropMode.Random:
                    SpawnRandom(dropper);
                    break;
          }
     }

     private Dropper CreateDropper()
     {
          var dropperObj = Instantiate(_dropperPrefab, _dropPoint.position, Quaternion.identity);
          
          return dropperObj.GetComponent<Dropper>();
     }

     private void SpawnAll(Dropper dropper)
     {
          dropper.DropManyItems(_dropItems);
     }

     private void SpawnRandom(Dropper dropper)
     {
          if (_dropItems.Count > 0)
          {
               var itemType = _dropItems.ElementAt(Random.Range(0, _dropItems.Count));
               
               dropper.DropOneItem(itemType);
          }
     }
}