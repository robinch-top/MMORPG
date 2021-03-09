using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using ETModel;
using Mirror;
namespace MMOGame
{
    public static class GateHelper
    {
        public static async ETVoid Logout()
        {
            Manager.RPG.ClearPreviews();
            Net.Gate.Send(new Logout_C2R(){UserId = Net.UserId});
            await ETTask.CompletedTask;
        }

        public static async ETVoid GetCharacters()
        {
            // 请求角色列表
            GetCharacters_G2C message = (GetCharacters_G2C)await Net.Gate.Call(new GetCharacters_C2G());
            
            // 还没有创建角色跳过
            if(message.Characters.count>0){
                List<Character> characters = new List<Character>();
                foreach(ETModel.CharacterInfo info in message.Characters){
                    Character character = CharacterByData(info);
                    characters.Add(character);
                }
                
                // 调用选角界面全部角色预览
                Manager.RPG.LoadAllPreview(characters.ToArray());
            }
        }

        public static async ETVoid CreateCharacter(string name,string cl,int index)
        {
            // Max表示希望限制创建多少个角色，如果不传Max值最多可以创建100角色
            CharacterMessage_G2C message = (CharacterMessage_G2C)await Net.Gate.Call(new CreateNewCharacter_C2G(){
                UserID = Net.UserId,Class = cl,Index = index,Name = name,Max = 4});
                            
            Character chara = CharacterByData(message.Character);
            
            // 调用刷新选角界面的新预览角色
            Manager.RPG.RefreshPreview(chara);
            Game.EventSystem.Run(EventIdType.CharaCreateFinish);
        }

        public static Character CharacterByData(ETModel.CharacterInfo info){
            Character chara = new Character(){
                UserId = Net.UserId,
                CharaId = info.CharaId,
                Name = info.Name,
                Class = info.Class,
                level = info.Level,
                experience = info.Experience,
                money = info.Money,
                mail = info.Mail,
                map = info.Map,
                region = info.Region,
                x = info.X,
                y = info.Y,
                z = info.Z,
                index = info.Index,
                equipDatas = Data2Equipments(To.List<ETModel.EquipInfo>(info.Equipments))
            };
            return chara;
        }

        public static EquipInfo[] Data2Equipments(List<ETModel.EquipInfo> list){
            List<EquipInfo> equiplist = new List<EquipInfo>();
            foreach(ETModel.EquipInfo item in list){
                equiplist.Add(ToStruct(item));
            }
            return equiplist.ToArray();
        }

        public static EquipInfo ToStruct(ETModel.EquipInfo info){
            EquipInfo item = new EquipInfo(){
                CatType = info.CatType,
                itemID = info.ItemID,
                endurance = info.Endurance,
                intellect = info.Intellect,
                strength = info.Strength,
                agility = info.Agility,
                amount = info.Amount
            };
            return item;
        }
    }
}