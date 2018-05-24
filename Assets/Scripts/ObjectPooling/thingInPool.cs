using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class thingInPool : PoolObj {

    public Rigidbody Body { get; private set; }

    MeshRenderer[] meshRenderers;

    public void SetMaterial(Material m)
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = m;
        }
    }

    void Awake()
    {
        Body = GetComponent<Rigidbody>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnTriggerEnter(Collider enteredCollider)
    {
        if (enteredCollider.CompareTag("Kill Zone"))
        {
            ReturnToPool();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ye");
        ReturnToPool();
    }

    public void StopRigidBody(float T)
    {
        Invoke("RigidStop", T);
    }

    private void RigidStop()
    {
        Rigidbody R = GenericObj as Rigidbody;
        R.isKinematic = true;
    }
}
