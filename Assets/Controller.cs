using UnityEngine;

public class ControleClavier : MonoBehaviour
{
    public float vitesse = 45f; // Vitesse de rotation en degrés par seconde

    [Header("Articulations")]
    public ArticulationBody baseRobot;
    public ArticulationBody epaule;
    public ArticulationBody coude;
    public ArticulationBody poignet;

    void FixedUpdate()
    {
        // 1. Base (Flèches Gauche / Droite)
        if (Input.GetKey(KeyCode.LeftArrow)) Tourner(baseRobot, -1f);
        if (Input.GetKey(KeyCode.RightArrow)) Tourner(baseRobot, 1f);

        // 2. Epaule (Flèches Haut / Bas)
        if (Input.GetKey(KeyCode.UpArrow)) Tourner(epaule, 1f);
        if (Input.GetKey(KeyCode.DownArrow)) Tourner(epaule, -1f);

        // 3. Coude (Touches Z et S)
        if (Input.GetKey(KeyCode.S)) Tourner(coude, 1f);
        if (Input.GetKey(KeyCode.W)) Tourner(coude, -1f);

        // 4. Poignet / Outil (Touches Q et D)
        if (Input.GetKey(KeyCode.A)) Tourner(poignet, 1f);
        if (Input.GetKey(KeyCode.D)) Tourner(poignet, -1f);
    }

    void Tourner(ArticulationBody joint, float direction)
    {
        if (joint == null) return;

        ArticulationDrive drive = joint.xDrive;
        
        // On incrémente ou décrémente la cible actuelle
        drive.target = Mathf.Clamp(
            drive.target + direction * vitesse * Time.fixedDeltaTime, drive.lowerLimit, drive.upperLimit);
        
        joint.xDrive = drive;
    }
}