using UnityEngine;
using UnityEngine.UIElements;

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


    private Vector3 racineVersBase; // vu que la base tourne aussi, la racine c'est le bateau

    // Rotation de depart de chaque articulation, on la stocke directement sous forme de matrice pour l'utiliser dans le produit de matrice
    private Matrix4x4 rotationInitBase;
    private Matrix4x4 rotationInitEpaule;
    private Matrix4x4 rotationInitCoude;
    private Matrix4x4 rotationInitPoignet;


    // On recupere qu'une seule fois le vecteur contenant la position de Y dans le repere de x,
    // car le repere de x suit l'articulation x et le segment est rigide,
    // donc la position de Y dans le repere de x ne change pas meme si l'articulation x bouge
    void Start()
    {
        // On recupere la position de Y dans le repere monde (Y.transform.position) puis on calcule sa position dans le
        // repere de x avec x.transform.InverseTransformPoint
        // InverseTransformPoint divise par l'échelle de x, ducoup on remet bien cette echelle avec VEctor3.Scale
        baseVersEpaule = Vector3.Scale(baseRobot.transform.InverseTransformPoint(epaule.transform.position), baseRobot.transform.lossyScale);
        epauleVersCoude = Vector3.Scale(epaule.transform.InverseTransformPoint(coude.transform.position), epaule.transform.lossyScale);
        coudeVersPoignet = Vector3.Scale(coude.transform.InverseTransformPoint(poignet.transform.position), coude.transform.lossyScale);
        poignetVersEffecteur = Vector3.Scale(poignet.transform.InverseTransformPoint(effecteur.transform.position), poignet.transform.lossyScale);

        /*Debug.Log(baseVersEpaule);
        Debug.Log(epauleVersCoude);
        Debug.Log(coudeVersPoignet);
        Debug.Log(poignetVersEffecteur);*/

        racineVersBase = baseRobot.transform.localPosition;

        // On récupère la rotation de départ de chaque articulation, c'est a dire la rotation locale par rapport a son parent, et on la convertit en matrice
        rotationInitBase = Matrix4x4.Rotate(baseRobot.transform.localRotation);
        rotationInitEpaule = Matrix4x4.Rotate(epaule.transform.localRotation);
        rotationInitCoude = Matrix4x4.Rotate(coude.transform.localRotation);
        rotationInitPoignet = Matrix4x4.Rotate(poignet.transform.localRotation);
    }

    // Update is called once per frame
    void Update()
    {
        // Condition temporaire, on lance la cinematique directe avec un clique gauche de la sourie
        if (Input.GetMouseButtonDown(0))
        {
            // On récupère la position calculée et la position réelle de l'effecteur et on verifie si la distance réelle est trop eloignée de celle calculée
            Vector3 positionCalculee = Directe();
            Vector3 positionReel = effecteur.position;

            Debug.Log("position calulée : " + positionCalculee);
            Debug.Log("position réelle : " + positionReel);
            Debug.Log("distance : " + Vector3.Distance(positionCalculee, positionReel));
        }
    }

    // Matrice de rotation autour de l'axe X en radians
    // On part de l'identité et on ne modifie que les cases qui changent
    private static Matrix4x4 RotationX(float X)
    {
        Matrix4x4 nouvelleMatrice = Matrix4x4.identity;
        nouvelleMatrice[1,1] = Mathf.Cos(X);
        nouvelleMatrice[1,2] = -Mathf.Sin(X);
        nouvelleMatrice[2,1] = Mathf.Sin(X);
        nouvelleMatrice[2,2] = Mathf.Cos(X);
        return nouvelleMatrice;
    }

    // Matrice de rotation autour de l'axe Y en radians
    // On part de l'identité et on ne modifie que les cases qui changent
    private static Matrix4x4 RotationY(float Y)
    {
        Matrix4x4 nouvelleMatrice = Matrix4x4.identity;
        nouvelleMatrice[0, 0] = Mathf.Cos(Y);
        nouvelleMatrice[0, 2] = Mathf.Sin(Y);
        nouvelleMatrice[2, 0] = -Mathf.Sin(Y);
        nouvelleMatrice[2, 2] = Mathf.Cos(Y);
        return nouvelleMatrice;
    }

    private Vector3 Directe()
    {
        // On recupere l'angle actuel de chaque articulation par rapport à sa position à 0 (donnée en radian)
        angleBase = baseRobot.jointPosition[0];
        angleEpaule = epaule.jointPosition[0];
        angleCoude = coude.jointPosition[0];
        anglePoignet = poignet.jointPosition[0];

        // On crée les matrices locales pour chaque articulation (Translation * Rotation * Rotation initiale)
        // Avec la translation, on place l'articulation dans le repere de son parent, puis la rotation l'oriente
        Matrix4x4 tBase = Matrix4x4.Translate(racineVersBase) * RotationY(angleBase) * rotationInitBase;
        Matrix4x4 tEpaule = Matrix4x4.Translate(baseVersEpaule) * RotationX(angleEpaule)* rotationInitEpaule;
        Matrix4x4 tCoude = Matrix4x4.Translate(epauleVersCoude) * RotationX(angleCoude)* rotationInitCoude;
        Matrix4x4 tPoignet = Matrix4x4.Translate(coudeVersPoignet) * RotationX(anglePoignet)* rotationInitPoignet;

        // L'effecteur ne tourne pas par rapport au poignet, donc il n'y a pas de rotation a faire
        Matrix4x4 tEffecteur = Matrix4x4.Translate(poignetVersEffecteur);

        // La racine est le parent de la base, ici le bateau, c'est la première marice, elle place tout le robot dans le repere monde
        Transform racine = baseRobot.transform.parent;
        Matrix4x4 tRacine = Matrix4x4.Translate(racine.position) * Matrix4x4.Rotate(racine.rotation);

        // On enchaine les matrices de la racine jusqu'a l'effecteur
        // Chaque matrice utilise le repere precedent donc quand une articulation tourne, tous ses enfants tournent avec elle
        Matrix4x4 tTotal = tRacine * tBase * tEpaule * tCoude * tPoignet * tEffecteur;

        // On renvoie la position calculée de l'effecteur dans le monde, c'est la dernière colonne de la matrice totale
        return tTotal.GetPosition();
    }

}
