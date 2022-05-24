using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[EditorTool("Edit Grid Case")]
public class GridEditorTool : EditorTool
{
    /*
    
        Code basé sur ca : https://github.com/bzgeb/PlaceObjectsTool/blob/main/Assets/PlaceObjectsTool/Editor/PlaceObjectsTool.cs
    */

    [SerializeField]  Texture2D _toolIcon;

    readonly GUIContent _iconContent = new GUIContent
    {
      
        text = "Edit Grid Case",
        tooltip = "Edit each case of the grid"
    };

    void OnEnable()
    {
        _iconContent.image = _toolIcon;
    }

    EnumField _toolRootElement;
    CaseState _caseStateToPaint;

    bool _receivedClickDownEvent;
    bool _receivedClickUpEvent;


    [Header("Previous setting")]
    Quaternion _oldRotation;
    bool _oldOrthographic;

    public override GUIContent toolbarIcon => _iconContent;

    public override void OnActivated()
    {
        Selection.activeObject = GameObject.FindGameObjectWithTag("GridManager");
    

        _iconContent.image = _toolIcon;
        //Permet de crée le menu en bas à gauche
        _toolRootElement = new EnumField(CaseState.Empty);
        _toolRootElement.style.width = 200;
        var backgroundColor = EditorGUIUtility.isProSkin
            ? new Color(0.21f, 0.21f, 0.21f, 0.8f)
            : new Color(0.8f, 0.8f, 0.8f, 0.8f);
        _toolRootElement.style.backgroundColor = backgroundColor;
        _toolRootElement.style.marginLeft = 10f;
        _toolRootElement.style.marginBottom = 10f;
        _toolRootElement.style.paddingTop = 5f;
        _toolRootElement.style.paddingRight = 5f;
        _toolRootElement.style.paddingLeft = 5f;
        _toolRootElement.style.paddingBottom = 5f;
        var titleLabel = new Label("Paint Case State");
        titleLabel.style.unityTextAlign = TextAnchor.UpperCenter;

        _caseStateToPaint = (CaseState)_toolRootElement.value;

        _toolRootElement.Add(titleLabel);

        var sv = SceneView.lastActiveSceneView;
        sv.rootVisualElement.Add(_toolRootElement);
        sv.rootVisualElement.style.flexDirection = FlexDirection.ColumnReverse;

        //on recupere les setting de sceneview davant pour les reattribuer à la fin
        _oldRotation = sv.rotation;
        _oldOrthographic = sv.orthographic;

        // Force la camera a être en vue de dessus
        sv.orthographic = true;
        sv.rotation = Quaternion.AngleAxis(90, Vector3.right); // vue de hautSs

        SceneView.beforeSceneGui += BeforeSceneGUI;
    }

    public override void OnWillBeDeactivated()
    {
        _toolRootElement?.RemoveFromHierarchy();
        SceneView.beforeSceneGui -= BeforeSceneGUI;
         var sv = SceneView.lastActiveSceneView;
        sv.rotation = _oldRotation;
        sv.orthographic = _oldOrthographic;
    }

    void BeforeSceneGUI(SceneView sceneView)
    {
        if (!ToolManager.IsActiveTool(this))
            return;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            ShowMenu();
            Event.current.Use();
        }

        
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            _receivedClickDownEvent = true;
            Event.current.Use();
        }

        if (_receivedClickDownEvent && Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            _receivedClickDownEvent = false;
            _receivedClickUpEvent = true;
            Event.current.Use();
        }
        
    }

    public override void OnToolGUI(EditorWindow window)
    {
        // Alors je fais ce truc bledard car la case s'updatai pas
        //if(Selection.activeTransform.TryGetComponent<Case>(out Case oof) || Selection.activeTransform.TryGetComponent<GridManager>(out GridManager oofa))
            Selection.activeObject = GameObject.FindGameObjectWithTag("GridManager");
        
        //Si on est pas dans la sceneView, l'outil s'arrete
        if (!(window is SceneView))
            return;
        //Si on est pas dans cette outil, on stop
        if (!ToolManager.IsActiveTool(this))
            return;

        if(Selection.transforms[0].tag != "GridManager")
            return;
      

        //Affiche un cercle sur la scene pour voir ce que l'on vise
        Handles.DrawWireDisc(GetCurrentMousePositionInScene(), Vector3.up, 1f  );

        //Si l'user reste appuyer cela va editer la case cible
        if (_receivedClickDownEvent)
        {
            _caseStateToPaint = (CaseState)_toolRootElement.value;
            foreach (var transform in Selection.transforms)
            {
                GridManager grid = transform.GetComponent<GridManager>();
                //Debug.Log(GetCurrentMousePositionInScene());
                grid.EditCase(GetCurrentMousePositionInScene() , _caseStateToPaint);

                // Alors je fais ce truc bledard car la case s'updatai pas
                Selection.activeObject = grid.GetCase((int)GetCurrentMousePositionInScene().x,(int)GetCurrentMousePositionInScene().y);

            }
            
            _receivedClickUpEvent = false;

           
        }

        window.Repaint(); // Permet de refresh la sceneView, afin que le rond du curseur bouge
      
    }

    Vector3 GetCurrentMousePositionInScene()
    {
        Vector3 mousePosition = Event.current.mousePosition;
        var placeObject = HandleUtility.PlaceObject(mousePosition, out var newPosition, out var normal);
        return placeObject ? newPosition : HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10);
    }

    void ShowMenu()
    {
        //var picked = HandleUtility.PickGameObject(Event.current.mousePosition, true);
        //if (!picked) return;

        var menu = new GenericMenu();
        //menu.AddItem(new GUIContent($"Pick {picked.name}"), false, () => { _caseStateToPaint = picked; });
        menu.ShowAsContext();
    }
}
