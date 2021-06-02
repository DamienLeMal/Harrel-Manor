using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatParticleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> particlePrefabs = new List<GameObject>();
    [SerializeField] private float heightOffset = 0.1f;
    public float attackDuration = 3f;
    [SerializeField] private Transform particleParent;

    private Dictionary<GameObject, bool> sceneParticles = new Dictionary<GameObject, bool>();

    public void PlayParticle (int id, TileEntity targetTile) {
        Debug.Log("Play Particle");
        if (id >= particlePrefabs.Count) return;
        GameObject particle = PullParticle(id,targetTile);
        particle.transform.position = targetTile.transform.position + Vector3.up*heightOffset;
        particle.SetActive(true);
        StartCoroutine(ResetParticle(particle));
    }

    private GameObject PullParticle (int id, TileEntity targetTile) {
        bool particleAvailable = false;
        GameObject availableParticle = null;
        foreach (KeyValuePair<GameObject,bool> p in sceneParticles) {
            if (particleAvailable == true) continue;
            if (p.Value) continue;
            Debug.Log("Pull particle");
            particleAvailable = true;
            availableParticle = p.Key;
        }
        if (particleAvailable) {
            sceneParticles[availableParticle] = true;
            return availableParticle;
        }
        Debug.Log("Instanciate Particle");
        GameObject newParticle = Instantiate(particlePrefabs[id],targetTile.transform.position + Vector3.up * heightOffset,Quaternion.identity, particleParent);
        sceneParticles.Add(newParticle,true);
        return newParticle;
    }

    IEnumerator ResetParticle (GameObject particle) {
        yield return new WaitForSeconds(attackDuration);
        sceneParticles[particle] = false;
        particle.SetActive(false);
    }
}
