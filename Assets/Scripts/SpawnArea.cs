using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public int CurrentUnitsInSpawn { get; private set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CurrentUnitsInSpawn++;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        CurrentUnitsInSpawn--;
    }
}
