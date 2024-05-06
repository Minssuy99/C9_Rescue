# 💊 C9_Resque

</br>

## 🖥️ 프로젝트 소개
C# 기초를 이용한 스파르타 던전 텍스트 게임 입니다.
<br>

## 🕐 개발 기간
* 24.04.29 ~ 24.05.07

### 👨‍💻 팀 구성
 - **팀장** : 김민성
 - **팀원** : 정래규
 - **팀원** : 홍성우
 - **팀원** : 황승준

### ⚙️ 개발 환경
- **언어** : `C#`
- **IDE** : `Visual Studio 2022`

## 💷 기능 구현
### 📕 메인 화면
<details>
<summary>📷 Image</summary>
 
![메인화면](https://github.com/Minssuy99/C9_Rescue/assets/101568505/ea3a3913-f16f-4ceb-9cf1-7758fd2a1605)

 
</details>

<details>
<summary>💻 Code</summary>
 
 ```csharp
  public void MainMenu()
 {
     Console.Clear();
     Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■");
     Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
     Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
     Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■");
     Console.WriteLine();
     Console.WriteLine("1. 상태보기");
     Console.WriteLine("2. 인벤토리");
     Console.WriteLine("3. 장비 상점");
     Console.WriteLine("4. 물약 상점");
     Console.WriteLine("5. 던전입장");
     Console.WriteLine("6. 여관");
     Console.WriteLine("7. 퀘스트");
     Console.WriteLine();

     int choice = ConsoleUtility.PromotMenuChoice(1, 7);

     switch (choice)
     {
         case 1:
             StatusMenu(); //상태보기
             break;
         case 2:
             InventoryMenu(); //인벤토리
             break;
         case 3:
             StoreMenu(); //장비상점
             break;
         case 4:
             PotionStoreMenu(); // 물약상점
             break;
         case 5:
             DungeonChoiceMenu(); // 던전입장
             break;
         case 6:
             RestMenu(); // 여관
             break;
         case 7:
             QuestMenu(); // 퀘스트
             break;
     }
     MainMenu();
 }
 ```

</details>

</br>

### 📕 상태보기
<details>
<summary>📷 Image</summary>

![상태보기](https://github.com/Minssuy99/C9_Rescue/assets/101568505/f5f08f3e-9c1e-4041-a36b-0aa4a092c3b6)


</details>

<details>
<summary>💻 Code</summary>

 ```csharp
private void StatusMenu()
{
    Console.Clear();

    ConsoleUtility.ShowTitle("■ 상태보기 ■");
    Console.WriteLine("캐릭터의 정보가 표기됩니다.");

    ConsoleUtility.PrintTextHighlights("Lv. ", player.Level.ToString("00")); //00은 두글자 제한
    Console.WriteLine();
    Console.WriteLine($"{player.Name}({player.Job})");

    //능력치 강화된 부분 추가하기
    int bonusAtk = inventory.Select(item => item.IsEquipped ? item.Atk : 0).Sum();
    int bonusDef = inventory.Select(item => item.IsEquipped ? item.Def : 0).Sum();
    int bonusHp = inventory.Select(item => item.IsEquipped ? item.Hp : 0).Sum();
    int bonusMp = inventory.Select(item => item.IsEquipped ? item.Mp : 0).Sum();


    ConsoleUtility.PrintTextHighlights("공격력 :", (player.Atk).ToString(), bonusAtk > 0 ? $"(+{bonusAtk})" : "");
    ConsoleUtility.PrintTextHighlights("방어력 :", (player.Def).ToString(), bonusDef > 0 ? $"(+{bonusDef})" : "");
    ConsoleUtility.PrintTextHighlights("체  력 :", $"{player.CurrentHp + bonusHp}/{(player.MaxHp + bonusHp)}".ToString(), bonusHp > 0 ? $"(+{bonusHp})" : "");
    ConsoleUtility.PrintTextHighlights("마  나 :", $"{player.CurrentMp + bonusMp}/{(player.MaxMp + bonusMp)}".ToString(), bonusMp > 0 ? $"(+{bonusMp})" : "");
    ConsoleUtility.PrintTextHighlights("경험치 :", $"{player.CurrentExp}/{player.MaxExp}");

    ConsoleUtility.PrintTextHighlights("Gold :", player.Gold.ToString());
    Console.WriteLine();

    Console.WriteLine("0. 뒤로가기");
    Console.WriteLine();


    switch (ConsoleUtility.PromotMenuChoice(0, 0))
    {
        case 0:
            MainMenu();
            break;
    }
}
```

</details>

</br>

### 📕 인벤토리
<details>
<summary>📷 Image</summary>

![인벤토리](https://github.com/Minssuy99/C9_Rescue/assets/101568505/72062dc0-10a1-4976-a167-b38683d8dbc4)

 
</details>

<details>
<summary>💻 Code</summary>

 ```csharp
private void InventoryMenu()
{
    Console.Clear();

    ConsoleUtility.ShowTitle("■ 인벤토리 ■");
    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
    Console.WriteLine();
    Console.WriteLine("[아이템 목록]");
    Console.WriteLine();

    Console.WriteLine("[장비 아이템]");
    Console.WriteLine();
    for (int i = 0; i < inventory.Count; i++)
    {
        inventory[i].PrintItemStatDescription();
    }

    Console.WriteLine("[소비 아이템]");
    Console.WriteLine();
    for (int i = 0; i < potioninventory.Count; i++)
    {
        potioninventory[i].PrintPotionStatDescription(); //나가기가 0번이라서 +1해줘서 띄워줌
    }

    Console.WriteLine();
    Console.WriteLine("0. 나가기");
    Console.WriteLine("1. 장착 관리");
    Console.WriteLine("2. 아이템 사용");
    Console.WriteLine();

    switch (ConsoleUtility.PromotMenuChoice(0, 2))
    {
        case 0:
            MainMenu();
            break;
        case 1:
            EquipMenu();
            break;
        case 2:
            ItemMenu();
            break;
    }
}
```

</details>


</br>

### 📕 장비상점
<details>
<summary>📷 Image</summary>

![장비상점](https://github.com/Minssuy99/C9_Rescue/assets/101568505/0d185f30-044d-4a14-a2b5-f4c86b23685f)

 
</details>

<details>
<summary>💻 Code</summary>

```csharp
 private void StoreMenu()
 {
     Console.Clear();

     ConsoleUtility.ShowTitle("■ 장비 상점 ■");
     Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
     Console.WriteLine();
     Console.WriteLine("[보유 골드]");
     ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
     Console.WriteLine();
     Console.WriteLine("[아이템 목록]");

     for (int i = 0; i < storeInventory.Count; i++)
     {
         storeInventory[i].PrintStoreItemDescription();
     }

     Console.WriteLine();
     Console.WriteLine("0. 나가기");
     Console.WriteLine("1. 아이템 구매");
     Console.WriteLine("2. 아이템 판매");
     Console.WriteLine();

     switch (ConsoleUtility.PromotMenuChoice(0, 2))
     {
         case 0:
             player.Items = inventory;
             MainMenu();
             break;
         case 1:
             PurchaseMenu();
             break;
         case 2:
             SellMenu();
             break;
     }
 }
```
</details>

</br>

### 📕 물약상점
<details>
<summary>📷 Image</summary>

![물약상점](https://github.com/Minssuy99/C9_Rescue/assets/101568505/7d5effe8-bcdd-42ef-8f14-2d6cf3f59923)


</details>

<details>
<summary>💻 Code</summary>

 ```csharp
private void PotionStoreMenu()
{
    Console.Clear();

    ConsoleUtility.ShowTitle("■ 물약 상점 ■");
    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
    Console.WriteLine();
    Console.WriteLine("[보유 골드]");
    ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
    Console.WriteLine();
    Console.WriteLine("[아이템 목록]");

    for (int i = 0; i < potion.Count; i++)
    {
        potion[i].PrintStorePotionDescription();
    }

    Console.WriteLine();
    Console.WriteLine("0. 나가기");
    Console.WriteLine("1. 물약 구매");
    Console.WriteLine("2. 포션 판매");
    Console.WriteLine();

    switch (ConsoleUtility.PromotMenuChoice(0, 2))
    {
        case 0:
            MainMenu();
            break;
        case 1:
            PotionPurchaseMenu();
            break;
        case 2:
            PotionSellMenu();
            break;
    }
}
```

</details>

</br>

### 📕 던전입장
<details>
<summary>📷 Image</summary>

![던전](https://github.com/Minssuy99/C9_Rescue/assets/101568505/47d05ae7-fd7d-4531-b8f2-573ba5195b95)


</details>

<details>
<summary>💻 Code</summary>

 ```chsarp
public void StartBattle(int random)
{
    while (true)
    {
        Console.Clear();
        ConsoleUtility.ShowTitle("■ Battle!! ■");
        Console.WriteLine();

        ShowMonsterStatus(random);

        ShowPlayerStatus();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[행동 선택]"); //s2를 강조함
        Console.ResetColor();
        Console.WriteLine("1. 공격");
        Console.WriteLine("2. 스킬");
        Console.WriteLine("3. 아이템");
        Console.WriteLine();
        int cho1 = ConsoleUtility.PromotMenuChoice(1, 3);
        if (cho1 == 1)
        {
            Console.WriteLine();
            ConsoleUtility.PrintTextHighlights("", "[몬스터 선택]");
            Console.WriteLine();
            Console.WriteLine("0. 뒤로 가기");

            int choice = GetPlayerChoice(random);
            if (choice == 0)
            {
                GameManager.Instance.StartBattle(random);
            }

            int selectedMonsterIndex = choice - 1;
            Monster selectedMonster = battleMonster[selectedMonsterIndex];

            if (!ValidateMonsterChoice(selectedMonsterIndex))
                continue;

            PlayerAttack(selectedMonster);

            if (CheckAllMonstersDead())
            {
                HandleAllMonstersDefeat();
                ShowResultMenu(random);
                break;
            }

            MonsterAttack();

            if (!player.IsAlive())
            {
                HandlePlayerDefeat();
                break;
            }

            Console.WriteLine("아무키나 누르세요...");
            Console.ReadKey();
        }
        else if (cho1 == 2)
        {
            if (GameManager.Instance.skills.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("사용할 수 있는 스킬이 없습니다.");
                Console.WriteLine();
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
            }
            else
            {
                for (int i = 0; i < skills.Count; i++)
                {
                    skills[i].PrintSkillStatDescription(i);
                }

            }

            Skill.UseSkill(random);

            if (CheckAllMonstersDead())
            {
                HandleAllMonstersDefeat();
                ShowResultMenu(random);
                break;
            }

            MonsterAttack();

            if (!player.IsAlive())
            {
                HandlePlayerDefeat();
                break;
            }

            Console.WriteLine("아무키나 누르세요...");
            Console.ReadKey();
        }
        else if (cho1 == 3)
        {
            DungeonItemMenu(random);
            if (CheckAllMonstersDead())
            {
                HandleAllMonstersDefeat();
                ShowResultMenu(random);
                break;
            }

            MonsterAttack();

            if (!player.IsAlive())
            {
                HandlePlayerDefeat();
                break;
            }

            Console.WriteLine("아무키나 누르세요...");
            Console.ReadKey();
        }
    }
}
```
</details>

</br>

### 📕 여관
<details>
<summary>📷 Image</summary>

![여관](https://github.com/Minssuy99/C9_Rescue/assets/101568505/4a052681-dee5-4f35-8310-903a0f5fb3fd)


 
</details>

<details>
<summary>💻 Code</summary>

 ```csharp
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

       public void LoadData(ref Player player, ref List<Item> inventory, ref List<Quest> quests, ref List<Skill> skills)
       {
           string _userDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
           PlayerDataLoad(ref player, _userDocumentsFolder, ref inventory, ref skills);
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

       private void PlayerDataLoad(ref Player player, string _userDocumentsFolder, ref List<Item> inventory, ref List<Skill> skills)
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
               if(player.skills != null)
               {
                   skills = player.skills;
               }
           }
           else
           {
               Console.WriteLine("저장된 플레이어 데이터가 없습니다.");
               checkPlayer = false; 
               Thread.Sleep(300);
           }
       }
   }
```

</details>

</br>

### 📕 퀘스트
<details>
<summary>📷 Image</summary>

![퀘스트](https://github.com/Minssuy99/C9_Rescue/assets/101568505/942aeac1-09d6-40a2-9603-2987a9580b43)

</details>

<details>
<summary>💻 Code</summary>

 ```csharp
private Quest GenerateQuest()
{
    Quest.QuestType questType = (Quest.QuestType)battleRandom.Next(0, 3);
    Quest.QuestDifficulty difficulty = (Quest.QuestDifficulty)battleRandom.Next(0, 3);
    string title = "";
    string description = "";
    int questTarget = 0;
    int rewardGold = 0;
    string targetMonsterName = "";

    switch (questType)
    {
        case Quest.QuestType.Hunt:
            int huntQuestIndex = battleRandom.Next(0, 2);
            switch (huntQuestIndex)
            {
                case 0:
                    title = "칼날부리 사냥";
                    description = "칼날부리들을 처치하여 마을을 안전하게 만드세요.";
                    questTarget = difficulty switch
                    {
                        Quest.QuestDifficulty.Easy => battleRandom.Next(1, 5),
                        Quest.QuestDifficulty.Normal => battleRandom.Next(5, 10),
                        Quest.QuestDifficulty.Hard => battleRandom.Next(10, 15),
                        _ => 0,
                    };
                    rewardGold = difficulty switch
                    {
                        Quest.QuestDifficulty.Easy => 500,
                        Quest.QuestDifficulty.Normal => 1000,
                        Quest.QuestDifficulty.Hard => 1500,
                        _ => 0,
                    };
                    targetMonsterName = "칼날부리";
                    break;
                case 1:
                    title = "고블린 사냥";
                    description = "고블린을 처치하여 마을을 안전하게 만드세요.";
                    questTarget = difficulty switch
                    {
                        Quest.QuestDifficulty.Easy => battleRandom.Next(1, 5),
                        Quest.QuestDifficulty.Normal => battleRandom.Next(5, 10),
                        Quest.QuestDifficulty.Hard => battleRandom.Next(10, 15),
                        _ => 0,
                    };
                    rewardGold = difficulty switch
                    {
                        Quest.QuestDifficulty.Easy => 500,
                        Quest.QuestDifficulty.Normal => 1000,
                        Quest.QuestDifficulty.Hard => 1500,
                        _ => 0,
                    };
                    targetMonsterName = "고블린";
                    break;
            }
            break;
        case Quest.QuestType.Collect:
            title = "따뜻한 겨울나기";
            description = "옷을 만들기 위해 필요한 늑대의 털을 가져다주세요.";
            questTarget = difficulty switch
            {
                Quest.QuestDifficulty.Easy => battleRandom.Next(1, 3),
                Quest.QuestDifficulty.Normal => battleRandom.Next(3, 6),
                Quest.QuestDifficulty.Hard => battleRandom.Next(6, 10),
                _ => 0,
            };
            rewardGold = difficulty switch
            {
                Quest.QuestDifficulty.Easy => 300,
                Quest.QuestDifficulty.Normal => 600,
                Quest.QuestDifficulty.Hard => 1000,
                _ => 0,
            };
            targetMonsterName = "어스름 늑대";
            break;
        case Quest.QuestType.Equip:
            title = "장비 장착을 해보자!";
            description = "[낡은 검]무기를 획득하여 장착하세요.";
            questTarget = 1;
            rewardGold = 600;
            break;
    }

    return new Quest(title, description, questType, difficulty, questTarget, rewardGold, targetMonsterName);
}
```

</details>












