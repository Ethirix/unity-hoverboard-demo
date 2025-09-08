using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Thruster : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public Collider Collider { get; private set; }
    
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }
}
