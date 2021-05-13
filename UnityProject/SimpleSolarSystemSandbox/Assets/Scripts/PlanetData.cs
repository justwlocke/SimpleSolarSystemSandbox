using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Planet_", menuName = "PlanetData", order = 1)]
public class PlanetData : ScriptableObject
{
    public string planetName = "";

    public Sprite planetImage;

    [Tooltip("How long in earth days does it take to rotate around the sun")]
    public float orbitalPeriod;

    public float planetMass;

    public float planetGravity;

    public double meanDistanceFromSun;

    [Tooltip("The amount of people living on the planet")]
    public double population;
}
