using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class Npc : MonoBehaviour, IInteractable
{
    public string npcName;
    
    public NpcType currentNpcType;

    public NavMeshAgent npc_Agent;

    #region State Machine
    public StateMachine stateMachine;

    #region Walker States
    public Npc_W_MovingState n_w_MovingState;

    #endregion
    #region Seller States
    public Npc_S_MovingState n_s_MovingState;
    public Npc_S_TradingState n_s_TradingState;
    public Npc_S_ReturnState n_s_ReturnState;

    #endregion
    #region Variables
    public float sellerStopDistance;

    public int spawnedPoint;
    public Transform npcHolderPos;
    public Transform npcDropPos;
    public List<GameObject> npcHolderItemsList = new List<GameObject>();
    public MeshRenderer npcStateRenderer; //Temporary
    public Transform NPCSpawnPoint;

    public Canvas npcCanvas;
    #endregion
    #endregion
    #region Stats

    public float npcSly;
    #endregion
    #region Awake Start Update
    private void Awake()
    {
        InitializeState();
    }
    #endregion
    #region Functions
    public void InitializeState()
    {
        if (stateMachine == null) { stateMachine = new StateMachine(); }
    }
    public void NewStatesForNpcType(NpcType currentNpcType)
    {
        Debug.Log(currentNpcType.ToString());
        switch (currentNpcType)
        {   

            case NpcType.Walker:
                if (n_w_MovingState == null) { n_w_MovingState = new Npc_W_MovingState(stateMachine, this); }
                break;
            case NpcType.Seller:
                if (n_w_MovingState == null) { n_w_MovingState = new Npc_W_MovingState(stateMachine, this); }
                if (n_s_MovingState == null) { n_s_MovingState = new Npc_S_MovingState(stateMachine, this); }
                if (n_s_TradingState == null) { n_s_TradingState = new Npc_S_TradingState(stateMachine, this);}
                if(n_s_ReturnState== null) { n_s_ReturnState = new Npc_S_ReturnState(stateMachine, this); }
                break;
            case NpcType.Customer:

                break;
            case NpcType.Thief:

                break;
        }
    }
    void InitializeStateAccordingToNpc()
    {
        NewStatesForNpcType(currentNpcType);
        switch (currentNpcType)
        {
            case NpcType.Walker:
                stateMachine.InitializeState(n_w_MovingState);
                break;
            case NpcType.Seller:
                stateMachine.InitializeState(n_s_MovingState);
                break;
            case NpcType.Customer:
                break;
            case NpcType.Thief:
                break;
            case NpcType.Initialize:
                KillStates();
                break;
        }
    }
    public void ChangeNpcType(NpcType npcType)
    {
        currentNpcType = npcType;
        InitializeStateAccordingToNpc();
    }
    public void OnDespawn()
    {
        ChangeNpcType(NpcType.Initialize);
    }
    public void KillStates()
    {
        n_w_MovingState = null;
        n_s_MovingState = null;
        n_s_TradingState = null;
    }
    public void OnInteract()
    {
        stateMachine.currentState.OnInteract();
    }

    public GameObject ReturnHudObject()
    {
        GameObject _hudObject = ObjectsManager.Instance.SpawnInteractableHudObject();
        if (_hudObject.TryGetComponent<HudSystem>(out HudSystem hudSystem))
        {
            hudSystem.SetInteractableHud("Press E To Start Trade");
        }
        return _hudObject;
    }

    public bool HasHud()
    {
        return false;
    }

    public Canvas ReturnObjectCanvas()
    {
        return UIManager.Instance.screenCanvas;
    }

    public void CallBeginningFunction()
    {
        throw new System.NotImplementedException();
    }

    public void CallEndingFunction()
    {
        throw new System.NotImplementedException();
    }
}
#endregion
