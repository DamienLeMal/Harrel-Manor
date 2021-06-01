using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatParticleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> particlePrefabs = new List<GameObject>();
    [SerializeField] private float heightOffset = 0.1f;

    private Dictionary<GameObject, bool> sceneParticles = new Dictionary<GameObject, bool>();

    public void PlayParticle (int id, TileEntity targetTile) {
        GameObject particle = PullParticle(id,targetTile);
        StartCoroutine(ResetParticle(particle));
    }

    private GameObject PullParticle (int id, TileEntity targetTile) {
        bool particleAvailable = false;
        GameObject availableParticle = null;
        foreach (KeyValuePair<GameObject,bool> p in sceneParticles) {
            if (particleAvailable == true) continue;
            if (p.Value == false) particleAvailable = true;
            availableParticle = p.Key;
        }
        if (particleAvailable) {
            sceneParticles[availableParticle] = true;
            StartCoroutine(ResetParticle(availableParticle));
            return availableParticle;
        }
        GameObject newParticle = Instantiate(particlePrefabs[id],targetTile.transform.position + Vector3.up * heightOffset,Quaternion.identity);
        sceneParticles.Add(newParticle,true);
        return newParticle;
    }

    IEnumerator ResetParticle (GameObject particle) {
        yield return new WaitForSeconds(1f);
        sceneParticles[particle] = false;
    }
}
