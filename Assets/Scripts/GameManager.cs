using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public System.Action onGameTick;

    public static readonly int TICKS_PER_SEC = 20;

    void Start()
    {
        StartCoroutine(GameTickPulse());
    }

    IEnumerator GameTickPulse()
    {
        yield return new WaitForSeconds(1f/TICKS_PER_SEC);
        onGameTick?.Invoke();
    }
}
