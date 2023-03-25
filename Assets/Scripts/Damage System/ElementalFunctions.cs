using System.Collections.Generic;
using UnityEngine;

public static class ElementalFunctions
{
    public static readonly int S_MaxImpactValue = 10;

    public static readonly float P_CritDamageMulti = 2;

    public static readonly float F_TimeBetweenBurnTicks = .2f;
    public static readonly int F_MaxBurnTicks = 10;
    public static readonly int F_MinBurnDamage = 1;
    public static int F_GetBurnDamage(DamageContainer damage)
    {
        float dmg = (float)damage.damage / 10;
        if (dmg < 1)
            dmg = F_MinBurnDamage;

        return Mathf.RoundToInt(dmg);
    }

    public static readonly float I_MaxFreezeTime = 10;
    /// <summary>
    /// Gets the additional damaged delt by shattering the Ice
    /// </summary>
    public static float I_GetShatterDamage(float damage, float impact)
    {
        if (impact < 1)
            return 0;
        //y = 10 * ln(2x + 5) - 16
        return (10 * Mathf.Log(2 * damage + 5) - 16) * impact;
    }
    #region Lightning
    public static readonly int L_MaxLightningChain = 3;
    public static readonly float L_LightningChainRange = 4;
    public static readonly float L_LightningDamageMulti = .6f;

    public static void L_ChainLightning(Entity rootEntity, DamageContainer damageContainer)
    {
        var d = CustomFunctions.RoundToIntWithClamp(damageContainer.damage * L_LightningDamageMulti, 1);
        var chainDmg = new DamageContainer(d, ElementalType.Lightning);

        var ignoredEntities = new List<Entity>();
        var currentEntity = rootEntity;

        ignoredEntities.Add(rootEntity);


        for (int i = 0; i < L_MaxLightningChain; i++)
        {
            //ignore current entity
            ignoredEntities.Add(currentEntity);
            
            //find next entity 
            var newEntity = GetOverlapedEntity(currentEntity.position);

            if (newEntity == null)
                break;
            Debug.Log($"Found entity {newEntity}");

            //damage it
            newEntity.Damage(chainDmg);
            Debug.Log($"Chained damage to {newEntity}");
            
            //set the new entity to the current entity
            currentEntity = newEntity;

            //loop
        }


        Entity GetOverlapedEntity(Vector2 root)
        {
            var cols = Physics2D.OverlapCircleAll(root, L_LightningChainRange);
            foreach (var c in cols)
            {
                var e = c.GetComponent<Entity>();
                if (e && !ignoredEntities.Contains(e))
                    return e;
            }
            return null;
        }

        /*var cols = Physics2D.OverlapCircleAll(rootEntity.transform.position, L_LightningChainRange);
        var entities = new List<Entity>();
        foreach (var c in cols)
        {
            var e = c.GetComponent<Entity>();
            if (e)
                entities.Add(e);
        }

        if (entities.Count == 0)
            return;

        Vector2 rootPos = rootEntity.transform.position;
        var d = RoundToIntWithClamp(damageContainer.damage * L_LightningDamageMulti, 1);
        var chainDmg = new DamageContainer(d, ElementalType.Lightning);
        
        int iter = L_MaxLightningChain <= entities.Count ? L_MaxLightningChain : entities.Count;
        for (int i = 0; i < iter; i++)
        {
            Debug.DrawRay(rootPos, rootPos + Vector2.up, Color.magenta, .5f);

            entities.Sort((a, b) => (a.SqrDistFromTarget(rootPos) > b.SqrDistFromTarget(rootPos) ? 1 : 0));
            entities[0].Damage(chainDmg);

            rootPos = entities[0].position;

            entities.RemoveAt(0);
            if (entities.Count == 0)
                break;
        }*/
    }
    #endregion
}