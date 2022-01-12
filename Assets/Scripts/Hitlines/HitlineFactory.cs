using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitlineFactory : Singleton<HitlineFactory>
{
    //Prefab references
    private GameObject hitlineBig;
    private GameObject hitlineSmall;

    protected override void Awake()
    {
        base.Awake();

        hitlineBig = Resources.Load("Prefabs/Hitline/BigHitline") as GameObject;
        hitlineSmall = Resources.Load("Prefabs/Hitline/Hitline") as GameObject;
    }

    public GameObject GetHitline(HitlineType hitlineType)
    {
        GameObject tempHitline = null;

        switch (hitlineType)
        {
            case HitlineType.BIG:
                tempHitline = MonoBehaviour.Instantiate(hitlineBig);
                break;

            case HitlineType.SMALL:
                tempHitline = MonoBehaviour.Instantiate(hitlineSmall);
                break;

            default:
                Debug.Log("Default case returned from factory");
                break;
        }

        return tempHitline;
    }

    public GameObject GetHitline(HitlineType hitlineType, Transform transform)
    {
        return GetHitline(hitlineType, transform, false);
    }

    public GameObject GetHitline(HitlineType hitlineType, Transform transform, bool instantiateInWorldSpace)
    {
        GameObject tempHitline = null;

        switch (hitlineType)
        {
            case HitlineType.BIG:
                tempHitline = MonoBehaviour.Instantiate(hitlineBig, transform, instantiateInWorldSpace);
                break;

            case HitlineType.SMALL:
                tempHitline = MonoBehaviour.Instantiate(hitlineSmall, transform, instantiateInWorldSpace);
                break;

            default:
                Debug.Log("Default case returned from factory");
                break;
        }

        return tempHitline;
    }
}
