using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityEnvironmentType
{
    Land,
    Water
}

public class EnvironmentEntityState : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameEvent OnEntityEnvironmentChange;
    protected static int initialID = 0;
    public int entityID {private set; get;}
    private EntityEnvironmentType currentState = EntityEnvironmentType.Land;

    void Awake()
    {
        entityID = initialID;
        initialID++;
    }

    public void SetEnvironmentState(EntityEnvironmentType state)
    {
        currentState = state;
        OnEntityEnvironmentChange.Raise(this, entityID);
    }

    public EntityEnvironmentType GetCurrentState()
    {
        return currentState;
    }
}
