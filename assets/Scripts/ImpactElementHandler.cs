using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactElementHandler : MonoBehaviour
{
    public Weapon.ImpactInfo[] ImpactElemets = new Weapon.ImpactInfo[0];

    //// Taken from Weapon class
    //// TODO: muzzle flash   *****************************************************
    //[System.Serializable]
    //public class ImpactInfo
    //{
    //    public MaterialType.MaterialTypeEnum MaterialType;
    //    public GameObject ImpactEffect;
    //}

    //GameObject GetImpactEffect(GameObject impactedGameObject)
    //{
    //    var materialType = impactedGameObject.GetComponent<MaterialType>();
    //    if (materialType == null)
    //        return null;
    //    foreach (var impactInfo in ImpactElemets)
    //    {
    //        if (impactInfo.MaterialType == materialType.TypeOfMaterial)
    //            return impactInfo.ImpactEffect;
    //    }
    //    return null;
    //}
    //// TODO: Muzzle flash end *******************************************************

}
