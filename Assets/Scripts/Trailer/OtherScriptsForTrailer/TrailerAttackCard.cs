using UnityEngine;

public class TrailerAttackCard : MonoBehaviour
{
    private Transform startPos;
    private Transform endPos;

    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private GameObject card;
    private bool isInitialized = false;
    private float startTime;
    public void Init(Transform _startPos, Transform _endPos)
    {
        startPos = _startPos;
        endPos = _endPos;

        transform.position = startPos.position;
        
        Vector3 dir = endPos.position - transform.position;
        dir.y = 0f;
        float yAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, yAngle, 0f);
        startTime = Time.time;
        isInitialized = true;
        card.SetActive(true);
    }

    private void Update()
    {
        if (!isInitialized) return;

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (endPos.position - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;

        // 계속 바라보게 (선택)
        Vector3 dir = endPos.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            float yAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, yAngle, 0f);
        }

        // 도착 체크
        if (Vector3.Distance(transform.position, endPos.position) < 0.1f)
        {
            transform.position = endPos.position;
            isInitialized = false;
            float elapsed = Time.time - startTime;
            Debug.Log($"[TrailerCard] Travel Time: {elapsed:F2} seconds");
        }
    }
}