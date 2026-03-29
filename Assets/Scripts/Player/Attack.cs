using System.Collections;
using UnityEngine;

// TODO: Change Position of Attack in the editor
public class Attack : MonoBehaviour
{
    // TODO: Refactor
    [SerializeField] private Player player;
    
    [Header("Shoot Plane")]
    [SerializeField] private float fixedX = 8f;

    [Header("Crosshair")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Camera cam;
    [SerializeField] private float shootCooldown = 0.2f;
    private SpriteRenderer crosshairRenderer;
    
    
    // left, right, up, down에 대한 restriction이 필요하고
    [Header("Bounds (YZ Plane)")]
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;
    
    private GameObject currentCrossHair;
    
    private bool canShoot = false;

    // [SerializeField] bool isDebugging = false;
    // For DEBUG, can erase later
    // private void Start()
    // {
    //     StartAttack(-5f);
    // }

    private void OnEnable()
    {
        GameEvents.OnSubRoundEnd += OnAttackEnd;
    }
    private void OnDisable()
    {
        GameEvents.OnSubRoundEnd -= OnAttackEnd;
    }

    public void StartAttack(float fixedX)
    {
        this.fixedX = fixedX;
        gameObject.SetActive(true);

        currentCrossHair = Instantiate(crosshair, transform);
        crosshairRenderer = currentCrossHair.GetComponent<SpriteRenderer>();
        canShoot = false;
        StartShootCooldown();
    }


    
    private Coroutine cooldownRoutine;

    private void Update()
    {
        if (canShoot)
        {
            UpdateCrosshair();
            
            if (Input.GetMouseButtonDown(0))
            {
                TryShoot();
            }
        }


    }
    private void StartShootCooldown()
    {
        if (cooldownRoutine != null)
            StopCoroutine(cooldownRoutine);

        cooldownRoutine = StartCoroutine(ShootCooldownRoutine());
    }
    
    private IEnumerator ShootCooldownRoutine()
    {

        SetCrosshairOpacity(0.7f);

        yield return new WaitForSeconds(shootCooldown);

        SetCrosshairOpacity(1f);

        canShoot = true;
    }
    
    private void SetCrosshairOpacity(float alpha)
    {
        Color c = crosshairRenderer.color;
        c.a = alpha;
        crosshairRenderer.color = c;
    }
    
    public void TryShoot()
    {
        canShoot = false;

        Vector3 startPos = transform.position;        // shooter (world)
        Vector3 endPos = GetShootPosition();          // crosshair (world)

        Debug.Log($"[DEBUG] start: {startPos}, end: {endPos}");

        player.SpawnAttackCard(startPos, endPos);
    }
    
    private void OnAttackEnd()
    {
        DisableAttack();
        DestroyCrosshair();
    }

    private void DestroyCrosshair()
    {
        Destroy(currentCrossHair);
        currentCrossHair = null;
    }

    private void DisableAttack()
    {
        gameObject.SetActive(false);
    }
    private void UpdateCrosshair()
    {
        Vector3 mouse = Input.mousePosition;

        float distance = Mathf.Abs(cam.transform.position.x - fixedX);
        mouse.z = distance;

        Vector3 worldPos = cam.ScreenToWorldPoint(mouse);

        worldPos.x = fixedX;
        
        worldPos.y = Mathf.Clamp(worldPos.y, minY, maxY);
        worldPos.z = Mathf.Clamp(worldPos.z, minZ, maxZ);

        // ✅ world → local
        Vector3 localPos = transform.InverseTransformPoint(worldPos);
        currentCrossHair.transform.localPosition = localPos;
    }

    private Vector3 GetShootPosition()
    {
        Vector3 targetPos = currentCrossHair.transform.position;
        Debug.Log("[DEBUG]  targetPos: " + targetPos);
        targetPos.x *= 2f;
        return targetPos;
    }
    
}