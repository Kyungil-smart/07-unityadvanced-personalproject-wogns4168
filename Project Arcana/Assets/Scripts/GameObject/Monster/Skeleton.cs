using System;
using System.Collections;
using UnityEngine;

public class Skeleton : MonsterBase
{
    public override string Name => "Skeleton";
    public override IEnumerator Act()
    {
        Debug.Log("Skeleton Turn Start");

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Skeleton Attack!");

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Skeleton Turn End");
    }

    public override void Reward()
    {
        throw new NotImplementedException();
    }
}