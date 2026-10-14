using UnityEngine;

public class CinematiqueDirecte : MonoBehaviour
{
    [Header("Articulations")]
    public ArticulationBody baseRobot;
    public ArticulationBody epaule;
    public ArticulationBody coude;
    public ArticulationBody poignet;
    
    [Header("Effecteur")]
    public Transform effecteur;

    // Vecteur de la forme xVersY,la position de Y dans le repere de x
    private Vector3 baseVersEpaule;
    private Vector3 epauleVersCoude;
    private Vector3 coudeVersPoignet;
    private Vector3 poignetVersEffecteur;

    // Angle actuel de chaque articulation par rapport à sa position à 0
    // 0 est defini par rapport au parent (donc independant de l'orientation du robot), en radians
    private float angleBase;
    private float angleEpaule;
    private float angleCoude;
    private float anglePoignet;
    
    // On recupere qu'une seule fois le vecteur contenant la position de Y dans le repere de x,
    // car le repere de x suit l'articulation x et le segment est rigide,
    // donc la position de Y dans le repere de x ne change pas meme si l'articulation x bouge
    void Start()
    {
        // On recupere la position de Y dans le repere monde (Y.transform.position) puis on calcul sa position dans le
        // repere de x avec x.transform.InverseTransformPoint
        baseVersEpaule = baseRobot.transform.InverseTransformPoint(epaule.transform.position);
        epauleVersCoude = epaule.transform.InverseTransformPoint(coude.transform.position);
        coudeVersPoignet = coude.transform.InverseTransformPoint(poignet.transform.position);
        poignetVersEffecteur = poignet.transform.InverseTransformPoint(effecteur.transform.position);
        
        Debug.Log(baseVersEpaule);
        Debug.Log(epauleVersCoude);
        Debug.Log(coudeVersPoignet);
        Debug.Log(poignetVersEffecteur);
    }

    // Update is called once per frame
    void Update()
    {
        // Condition temporaire, on lance la cinematique directe avec un clique gauche de la sourie
        if (Input.GetMouseButtonDown(0))
        {
            Directe();
        }
    }

    private void Directe()
    {
        // On recupere l'angle actuel de chaque articulation par rapport à sa position à 0 (donnée en radian)
        angleBase = baseRobot.jointPosition[0];
        angleEpaule = epaule.jointPosition[0];
        angleCoude = coude.jointPosition[0];
        anglePoignet = poignet.jointPosition[0];
        
        Debug.Log(angleBase);
        Debug.Log(angleEpaule);
        Debug.Log(angleCoude);
        Debug.Log(anglePoignet);
    }
}
