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
        public void SavePlayer(Player player)
        {
            string _fileName = "playerStat.json"; //Json파일 생성
                                                  // 데이터 경로 저장. (C드라이브, Documents)
            string _userDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //경로 내문서
            string _filePath = Path.Combine(_userDocumentsFolder, _fileName); //경로 생성

            string _playerJson = JsonConvert.SerializeObject(player, Formatting.Indented);
            File.WriteAllText(_filePath, _playerJson);
            Console.WriteLine("저장이 완료되었습니다.");
        }

        public bool LoadData(ref Player player, ref List<Item> inventory)
        {
            string _playerFileName = "playerStat.json";
           // string _itemFileName = "itemData.json";
            // 데이터 경로 불러오기. (C드라이브, Documents)
            string _userDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // 플레이어 데이터 로드
            string _playerFilePath = Path.Combine(_userDocumentsFolder, _playerFileName);
            if (File.Exists(_playerFilePath))
            {
                string _playerJson = File.ReadAllText(_playerFilePath);
                player = JsonConvert.DeserializeObject<Player>(_playerJson);
                if (player.Items != null)
                {
                    inventory = player.Items;
                }
                Console.WriteLine("플레이어 데이터를 불러왔습니다.");
                return true;
            }
            else
            {
                Console.WriteLine("저장된 플레이어 데이터가 없습니다.");
                return false;
                Thread.Sleep(300);
            }
            Console.ReadKey();
        }
    }
}
