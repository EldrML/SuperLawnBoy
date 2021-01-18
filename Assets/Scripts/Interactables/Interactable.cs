using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] float gizmoRadius = 0.75f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }

}
