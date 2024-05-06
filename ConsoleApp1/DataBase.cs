using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    internal class DataBase
    {
        public bool checkQuest;
        public bool checkPlayer;
        public void SavePlayer(Player player, List<Quest> quest)
        {
            string _fileName = "playerStat.json"; //Json
            string _QuestFileName = "QuestmData.json";

            // 데이터 경로 저장. (C드라이브, Documents)
            string _userDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //경로 내문서


            string _filePath = Path.Combine(_userDocumentsFolder, _fileName); //경로 생성
            string _QuestFilePath = Path.Combine(_userDocumentsFolder, _QuestFileName);


            string _playerJson = JsonConvert.SerializeObject(player, Formatting.Indented); //C#->JSON파일변환
            string _questJson = JsonConvert.SerializeObject(quest, Formatting.Indented); //C#->JSON파일변환

            File.WriteAllText(_filePath, _playerJson); //파일 생성
            File.WriteAllText(_QuestFilePath, _questJson);
            Console.WriteLine("저장이 완료되었습니다.");
        }

        public void LoadData(ref Player player, ref List<Item> inventory, ref List<Quest> quests)
        {

            string _userDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            PlayerDataLoad(ref player, _userDocumentsFolder, ref inventory);
            QuestDataLoad(ref quests, _userDocumentsFolder);
            Console.ReadKey();
        }

        private void QuestDataLoad(ref List<Quest> quests, string _userDocumentsFolder)
        {
            string _QuestFileName = "QuestmData.json";

            string _QuestFilePath = Path.Combine(_userDocumentsFolder, _QuestFileName);

            if (File.Exists(_QuestFilePath))
            {
                string _questrJson = File.ReadAllText(_QuestFilePath);

                quests = JsonConvert.DeserializeObject<List<Quest>>(_questrJson);//Json->C#파일변환
                checkQuest = true;
            }
            else
            {
                checkQuest = false;
                Thread.Sleep(300);
            }
        }

        private void PlayerDataLoad(ref Player player, string _userDocumentsFolder, ref List<Item> inventory)
        {
            string _playerFileName = "playerStat.json";

            string _playerFilePath = Path.Combine(_userDocumentsFolder, _playerFileName);

            if (File.Exists(_playerFilePath))
            {
                string _playerJson = File.ReadAllText(_playerFilePath);

                player = JsonConvert.DeserializeObject<Player>(_playerJson);//Json->C#파일변환
                
                if (player.Items != null)
                {
                    inventory = player.Items;
                }
                Console.WriteLine("플레이어 데이터를 불러왔습니다.");
                checkPlayer = true;

            }
            else
            {
                Console.WriteLine("저장된 플레이어 데이터가 없습니다.");
                checkPlayer = false; 
                Thread.Sleep(300);
            }
        }
    }
}
