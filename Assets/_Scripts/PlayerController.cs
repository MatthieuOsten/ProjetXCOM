using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Team
{
    [SerializeField] protected GridManager _selectedGrid;
    Controller _inputManager;
    [SerializeField] bool SelectMode;
    [SerializeField] Case SelectedCaseA, SelectedCaseB;
    [SerializeField] ActorTest SelectedActor;

    /*
        Regarde ce que la souris touche
    */

    public override void Awake()
    {
        _selectedGrid = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        base.Awake();
    }

    public override void Start()
    {
        _inputManager = new Controller();
        _inputManager.TestGrid.Enable();
    }

    Vector3 MouseToWorldPosition()
    {
        RaycastHit RayHit;
        Ray ray;
        GameObject ObjectHit;
        Vector3 Hitpoint = Vector3.zero;
        ray = Camera.main.ScreenPointToRay(_inputManager.TestGrid.MousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RayHit))
        {
            ObjectHit = RayHit.transform.gameObject;
            Hitpoint = new Vector3((int)RayHit.point.x, (int)RayHit.point.y, (int)RayHit.point.z);
            if (ObjectHit != null)
                Debug.DrawLine(Camera.main.transform.position, Hitpoint, Color.blue, 0.5f);

        }

        return Hitpoint;
    }

    void WatchCursor()
    {
        Vector3 mousePos = MouseToWorldPosition();
        int x = (int)mousePos.x / _selectedGrid.CellSize;
        int y = (int)mousePos.z / _selectedGrid.CellSize;

        Case AimCase = GridManager.GetValidCase(GridManager.GetCase(_selectedGrid, x, y));
        SelectModeWatcher(AimCase);
    }

    void SelectModeWatcher(Case AimCase)
    {
        if (SelectMode)
        {
            if (_inputManager.TestGrid.Action.ReadValue<float>() == 1)
            {
                if (SelectedCaseA == null)
                {
                    SelectedCaseA = AimCase;
                    SelectedCaseA.Highlighted = true;
                    SelectedCaseA.ChangeMaterial(_selectedGrid.Data.caseHighlight);

                    if (SelectedActor == null)
                        SelectedActor = SelectedCaseA._actor;

                    return;
                }
                else
                {
                    SelectedCaseB = AimCase;
                    SelectedCaseB.Highlighted = true;
                    SelectedCaseB.ChangeMaterial(_selectedGrid.Data.caseHighlight);
                    //UIManager.CreateHintString(AimCase.gameObject, "XCOM HINTSTRING OUAAHHH");
                }

                if (SelectedActor != null)
                {
                    SelectedActor.Destination = SelectedCaseB;
                }
                else
                {
                    PathFinding.FindPath(SelectedCaseA, SelectedCaseB);
                }

            }
            if (_inputManager.TestGrid.Echap.ReadValue<float>() == 1)
            {
                SelectedCaseA.Highlighted = false;
                SelectedCaseA = null;
                SelectedCaseB.Highlighted = false;
                SelectedCaseB = null;
                SelectedActor = null;
            }
        }
    }

    public override void Update()
    {
        WatchCursor();
        base.Update();
    }
}
