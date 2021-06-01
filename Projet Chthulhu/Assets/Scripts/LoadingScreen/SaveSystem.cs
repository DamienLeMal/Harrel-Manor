using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem current;
    public static int loadCount = 0;
    public List<GameObject> pickUp = new List<GameObject>();
    private CombatManager manager;
    private PlayerEntity player;
    [SerializeField] private List<WeaponData> weaponDataId = new List<WeaponData>();

    private void Awake() {
        current = this;
        if (loadCount == 0) PlayerPrefs.DeleteAll();
        
    }
    private void Start() {
        manager = CombatManager.current;
        player = manager.player;
        SaveGame(player.transform.position, player.transform.rotation.eulerAngles);
        LoadGame();
        
    }
    public void SaveGame (Vector3 spawnPos, Vector3 spawnRot) {

        PlayerPrefs.SetFloat("posX",spawnPos.x);
        PlayerPrefs.SetFloat("posY",spawnPos.y);
        PlayerPrefs.SetFloat("posZ",spawnPos.z);
        PlayerPrefs.SetFloat("rotX",spawnRot.x);
        PlayerPrefs.SetFloat("rotY",spawnRot.y);
        PlayerPrefs.SetFloat("rotZ",spawnRot.z);

        PlayerPrefs.SetInt("s_str",player.str);
        PlayerPrefs.SetInt("s_dex",player.dex);
        PlayerPrefs.SetInt("s_spd",player.spd);
        PlayerPrefs.SetInt("s_lck",player.lck);
        PlayerPrefs.SetInt("s_intl",player.intl);
        PlayerPrefs.SetInt("s_con",player.con);
        PlayerPrefs.SetInt("s_agi",player.agi);
        PlayerPrefs.SetInt("s_mnt",player.mnt);
        PlayerPrefs.SetInt("s_hp",player.hp);

        PlayerPrefs.SetInt("s_wp1",999);
        PlayerPrefs.SetInt("s_wp2",999);
        PlayerPrefs.SetInt("s_wp3",999);

        if (player.weaponInventory.Count >= 1) if (player.weaponInventory[0] != null) PlayerPrefs.SetInt("s_wp1",weaponDataId.IndexOf(player.weaponInventory[0]));
        if (player.weaponInventory.Count >= 2) if (player.weaponInventory[1] != null) PlayerPrefs.SetInt("s_wp2",weaponDataId.IndexOf(player.weaponInventory[1]));
        if (player.weaponInventory.Count >= 3) if (player.weaponInventory[2] != null) PlayerPrefs.SetInt("s_wp3",weaponDataId.IndexOf(player.weaponInventory[2]));

        PlayerPrefs.Save();
    }

    public void LoadGame () {
        loadCount++;

        player.transform.position = new Vector3 (PlayerPrefs.GetFloat("posX"),PlayerPrefs.GetFloat("posY"),PlayerPrefs.GetFloat("posZ"));
        player.transform.eulerAngles = new Vector3 (PlayerPrefs.GetFloat("rotX"),PlayerPrefs.GetFloat("rotY"),PlayerPrefs.GetFloat("rotZ"));

        player.str = PlayerPrefs.GetInt("s_str");
        player.dex = PlayerPrefs.GetInt("s_dex");
        player.spd = PlayerPrefs.GetInt("s_spd");
        player.lck = PlayerPrefs.GetInt("s_lck");
        player.intl = PlayerPrefs.GetInt("s_intl");
        player.con = PlayerPrefs.GetInt("s_con");
        player.agi = PlayerPrefs.GetInt("s_agi");
        player.mnt = PlayerPrefs.GetInt("s_mnt");
        player.hp = PlayerPrefs.GetInt("s_hp");

        if (loadCount > 0) player.weaponInventory.Clear();

        if (PlayerPrefs.GetInt("s_wp1") < weaponDataId.Count && PlayerPrefs.GetInt("s_wp1") >= 0) player.weaponInventory.Add(weaponDataId[PlayerPrefs.GetInt("s_wp1")]);
        if (PlayerPrefs.GetInt("s_wp2") < weaponDataId.Count && PlayerPrefs.GetInt("s_wp2") >= 0) player.weaponInventory.Add(weaponDataId[PlayerPrefs.GetInt("s_wp2")]);
        if (PlayerPrefs.GetInt("s_wp3") < weaponDataId.Count && PlayerPrefs.GetInt("s_wp3") >= 0) player.weaponInventory.Add(weaponDataId[PlayerPrefs.GetInt("s_wp3")]);

        player.NewMaxStat();
    }

}