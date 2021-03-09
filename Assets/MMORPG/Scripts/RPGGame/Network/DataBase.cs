using UnityEngine;
using System.Collections.Generic;
namespace Mirror
{
	public static class DataBase
	{
        public static void LoadEquipment(Player player,Character chara){
        
            PlayerEquipment equipment = player.equipment;
            EquipInfo[] equipDatas = chara.equipDatas;

            equipment.LoadPlayerEquipped(equipDatas);
        }
        
        // 第三个参数preview 为true，用于预览这样不用加载背包与技能
        public static GameObject CharacterLoad(Character row, List<Player> prefabs,bool preview = false)
        {
            // 用表达示根据Class名找出预制体对象
            Debug.Log(row.Class);
            Player prefab = prefabs.Find(p => p.name == row.Class);
            if (prefab != null)
            {
                GameObject go = UnityEngine.Object.Instantiate(prefab.gameObject);
                Player player = go.GetComponent<Player>();
                player.name                                   = row.Name;
                Vector3 position                              = new Vector3(row.x, row.y, row.z);
                player.level.current                          = Mathf.Min(row.level, player.level.max); // limit to max level in case we changed it
                player.experience.current                     = row.experience;
                player.skills.skillExperience                 = row.skillExperience;
                player.gold                                   = row.money;

                // 加载装备数据到角色装备组件中
                LoadEquipment(player,row);

                // 如果是非预览，加载更多组件数据
                if(!preview){
                    //LoadInventory(player,loadDefault);
                    //LoadSkills(player,loadDefault);
                    // ...
                }
            
                player.health.current = row.health;
                player.mana.current = row.mana;

                return go;
            }
            else Debug.LogError("no prefab found for class: " + row.Class);
            return null;
        }
    }
}