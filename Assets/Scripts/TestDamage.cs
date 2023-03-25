using System.Collections;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    public DamageContainer container;

    [ContextMenu("Test Damage")]
    // Update is called once per frame
    void TestDamageFunc()
    {
        GetComponent<Entity>().Damage(container);
    }
}