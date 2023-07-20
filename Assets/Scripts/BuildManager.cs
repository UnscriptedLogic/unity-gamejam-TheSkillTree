using UnityEngine;
using UnityEngine.UI;
using UnscriptedLogic;

public class BuildManager : MonoBehaviour
{
    private InputManager inputManager;

    [Header("Buildings")]
    [SerializeField] private float maxNodeDistance = 4;
    [SerializeField] private LayerMask towerLayer;

    [Header("Components")]
    [SerializeField] private Camera cam;
    [SerializeField] private BottomHUD bottomHUDScript;

    private bool isBuilding;
    private Node previouslySelectedNode;
    private GameObject nodePreviewPlacement;
    private GameObject nodeToPlacePrefab;

    private NodeSO currentNodeSO;
    private Vector3 worldMousePos;
    private LayerMask buildableLayer;
    private ICanBuildOff canBuildOffable;

    private void Start()
    {
        inputManager = InputManager.instance;
        inputManager.OnClick += OnClick;
        inputManager.OnCancel += OnCancel;
    }

    private void Update()
    {
        if (isBuilding)
        {
            worldMousePos = cam.ScreenToWorldPoint(inputManager.MousePosition);
            Vector3 offset = new Vector3(worldMousePos.x, worldMousePos.y, 0f) - previouslySelectedNode.transform.position;
            nodePreviewPlacement.transform.position = previouslySelectedNode.transform.position + Vector3.ClampMagnitude(offset, maxNodeDistance);

            nodePreviewPlacement.GetComponent<LineRenderer>().SetPosition(0, nodePreviewPlacement.transform.position);
            nodePreviewPlacement.GetComponent<LineRenderer>().SetPosition(1, previouslySelectedNode.transform.position);
        }
    }

    private void OnCancel()
    {
        if (isBuilding) 
        { 
            isBuilding = false;
            Destroy(nodePreviewPlacement);
        }
    }

    private void OnClick()
    {
        if (inputManager.IsMouseOverUI())
        {
            return;
        }

        if (isBuilding)
        {
            Collider2D towerObstruction = Physics2D.OverlapCircle(nodePreviewPlacement.transform.position, 1.5f, towerLayer);
            if (towerObstruction != null)
            {
                return;
            }

            Collider2D requiredLayer = Physics2D.OverlapCircle(nodePreviewPlacement.transform.position, 1f);
            LayerMask layer = requiredLayer.gameObject.layer;
            if (layer != buildableLayer)
                return;

            GameObject nodeObject = Instantiate(nodeToPlacePrefab, nodePreviewPlacement.transform.position, nodePreviewPlacement.transform.rotation);
            nodeObject.GetComponent<Node>().SetLink(previouslySelectedNode);
            Destroy(nodePreviewPlacement);

            CurrencyManager.instance.CurrencyHandler.Modify(ModifyType.Subtract, currentNodeSO.Cost);

            canBuildOffable.Connections++;
            nodeToPlacePrefab = null;
            nodePreviewPlacement = null;
            isBuilding = false;
            return;
        }
            
        worldMousePos = cam.ScreenToWorldPoint(inputManager.MousePosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(worldMousePos, cam.transform.forward);

        if (hitInfo.collider == null)
        {
            bottomHUDScript.ResetBasicDetails();
            bottomHUDScript.ClearOptions();
            return;
        }

        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            bottomHUDScript.ResetBasicDetails();
            bottomHUDScript.ClearOptions();
            return;
        }

        previouslySelectedNode = hitInfo.collider.GetComponent<Node>();
        
        if (hitInfo.collider.TryGetComponent(out IInspectable inspectable))
        {
            SetInspectableDetails(inspectable);
        }

        if (hitInfo.collider.TryGetComponent(out ICanBuildOff buildOffable))
        {
            Buildable(buildOffable, hitInfo.collider.gameObject);
        }

        if (hitInfo.collider.TryGetComponent(out IUpgradable upgradable))
        {
            for (int i = 0; i < upgradable.Upgrades.Count; i++)
            {
                Upgrade upgrade = upgradable.Upgrades[i];
                bottomHUDScript.CreateUpgradeButton(upgrade, () =>
                {
                    upgrade.UpgradeMethod(previouslySelectedNode);
                });
            }
        }
    }

    private void SetInspectableDetails(IInspectable inspectable)
    {
        bottomHUDScript.ClearOptions();
        bottomHUDScript.SetBasicDetails(inspectable.GetInspectDesc(), inspectable.Icon);
    }

    private void Buildable(ICanBuildOff buildOffable, GameObject buildoff)
    {
        Button[] optionButtons = new Button[buildOffable.Buildables.Length];
        for (int i = 0; i < buildOffable.Buildables.Length; i++)
        {
            NodeSO nodeSO;
            nodeSO = buildOffable.Buildables[i];
            LayerMask layer = LayerMask.NameToLayer(nodeSO.BuildableLayer);
            optionButtons[i] = bottomHUDScript.CreateOptionButton(nodeSO, () =>
            {
                nodeToPlacePrefab = nodeSO.NodePrefab;
                nodePreviewPlacement = CreatePreviewObject(nodeSO.NodePrefab);
                buildableLayer = layer;
                canBuildOffable = buildOffable;
                isBuilding = true;
                currentNodeSO = nodeSO;
                bottomHUDScript.ClearOptions();

                SetInspectableDetails(buildoff.GetComponent<IInspectable>());
                Buildable(buildOffable, buildoff);
            });

            optionButtons[i].interactable = false;

            if (buildOffable.Connections >= buildOffable.MaxConnections + GlobalStatManager.instance.StorageUnit)
                continue;

            if (!CurrencyManager.instance.CurrencyHandler.HasAmount(nodeSO.Cost))
                continue;

            optionButtons[i].interactable = true;
        }
    }

    private GameObject CreatePreviewObject(GameObject objectToPreview)
    {
        GameObject preview = Instantiate(objectToPreview);
        Node node = preview.GetComponent<Node>();
        Destroy(node);

        Destroy(node.GetComponent<BoxCollider2D>());
        return preview;
    }
}
