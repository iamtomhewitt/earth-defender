using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlanetOrbit : MonoBehaviour 
{
    public Vector3 rotationSpeed;
    public Vector3 rotationAngle;

    public bool orbits = false;

    public Transform whatToOrbit;
    public float orbitSpeed;
    	
    float count;

	void Update ()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);

        if (orbits)
        {
            transform.RotateAround(whatToOrbit.position, rotationAngle, orbitSpeed);
        }
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlanetOrbit))]
public class PlanetEditor : Editor
{
    bool orbits = false;

    PlanetOrbit planet = null;

    void OnEnable()
    {
        planet = (PlanetOrbit)target;
    }


    public override void OnInspectorGUI()
    {
        planet.rotationSpeed = EditorGUILayout.Vector3Field("Rotation Speed", planet.rotationSpeed);
        planet.rotationAngle = EditorGUILayout.Vector3Field("Rotation Angle", planet.rotationAngle);

        EditorGUILayout.Space();

        planet.orbits = EditorGUILayout.Toggle("Orbits planet?", planet.orbits);

        if (planet.orbits)
        {
            GUILayout.Label("What does it orbit?", GUILayout.Width(200));
            planet.whatToOrbit = EditorGUILayout.ObjectField(planet.whatToOrbit, typeof(Transform), true) as Transform;

            GUILayout.Label("Orbit Speed", GUILayout.Width(100));
            planet.orbitSpeed = EditorGUILayout.FloatField(planet.orbitSpeed, null);
        }
    }

}
#endif