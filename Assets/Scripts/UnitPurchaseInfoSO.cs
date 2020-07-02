using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class UnitPurchaseInfoSO : ScriptableObject
{
   public string Name;
   public int Cost;
   public Sprite Image;

   public GameObject UnitPrefab;
}
