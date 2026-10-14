using UnityEngine;
using UnityEngine.UIElements;

public class Grappin : MonoBehaviour
{
    [SerializeField] private LayerMask layerObjets;
    [SerializeField] private Transform pointAccroche;

    private GameObject objetActuel;

    // Update is called once per frame
    void Update()
    {
        if (objetActuel == null)
        {
            Collider[] objets = Physics.OverlapSphere(transform.position, 3f, layerObjets);

            foreach (Collider col in objets)
            {
                if (col.CompareTag("Attrapable"))
                {
                    Attraper(col.gameObject);
                    break;
                }
            }
        }
        else
        {
            objetActuel.transform.position = pointAccroche.position;
            objetActuel.transform.rotation = pointAccroche.rotation;
        }
    }
    
    void Attraper(GameObject objet) {
        objetActuel = objet;
        
        Rigidbody rb = objet.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;

        Animator animator = objet.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
    }
}
