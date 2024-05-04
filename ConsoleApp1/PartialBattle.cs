using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

public partial class GameManager
{
    private Monster GetRandomMonster()
    {
        Random rand = new Random();
        return monster[rand.Next(monster.Count)];
    }

    private void ShowMonsterStatus(int random)
    {
        for (int i = 0; i < random; i++)
        {
            if (!battleMonster[i].IsAlive())
                ConsoleUtility.PrintTextDeath("", $"[{i + 1}] Lv.{battleMonster[i].Level} {battleMonster[i].Name} HP {battleMonster[i].CurrentHp}");
            else
                Console.WriteLine($"[{i + 1}] Lv.{battleMonster[i].Level} {battleMonster[i].Name} HP {battleMonster[i].CurrentHp}");
        }
        Console.WriteLine();
    }

    private void ShowPlayerStatus()
    {
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name}({player.Job})");
        Console.WriteLine($"HP {player.CurrentHp} / {player.MaxHp}");
        Console.WriteLine($"MP {player.CurrentMp} / {player.MaxMp}");
        Console.WriteLine();
    }

    private int GetPlayerChoice(int random)
    {
        return ConsoleUtility.PromotMenuChoice(1, random);
    }

    private bool ValidateMonsterChoice(int selectedMonsterIndex)
    {
        if (!battleMonster[selectedMonsterIndex].IsAlive())
        {
            Console.WriteLine("선택한 몬스터가 이미 죽었습니다. 다른 몬스터를 선택하세요.");
            Console.ReadKey();
            return false;
        }
        return true;
    }

    private void PlayerAttack(Monster selectedMonster)
    {
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights("", $"{player.Name}가 공격!");

        double minAtk = Math.Ceiling(player.Atk * 0.9); // 최소 공격력 (원래 공격력의 90%)
        double maxAtk = Math.Ceiling(player.Atk * 1.1); // 최대 공격력 (원래 공격력의 110%)
        if (battleRandom.Next(100) < 10)
        {
            Console.WriteLine("회피!");
            Console.WriteLine($"몬스터 {selectedMonster.Name}이(가) 공격을 회피했습니다.");
        }
        else
        {
            int attackDamage = battleRandom.Next((int)minAtk, (int)maxAtk);
            if (IsCriticalHit())
                InflictCriticalDamage(selectedMonster, attackDamage);
            else
                InflictRegularDamage(selectedMonster, attackDamage);

            if (!selectedMonster.IsAlive())
            {
                HandleMonsterDefeat(selectedMonster);
            }
        }
        Thread.Sleep(1000);
    }

    private bool IsCriticalHit()
    {
        return battleRandom.Next(100) < 15;
    }

    private void InflictCriticalDamage(Monster selectedMonster, int attackDamage)
    {
        int criticalDamage = (int)(attackDamage * 1.6);
        Console.WriteLine("치명타 발생! 추가 데미지를 가합니다.");
        Console.WriteLine($"[데미지 : {criticalDamage}]");
        selectedMonster.MonterTakeDamage(criticalDamage);
    }

    private void InflictRegularDamage(Monster selectedMonster, int attackDamage)
    {
        Console.WriteLine($"[데미지 : {attackDamage}]");
        selectedMonster.MonterTakeDamage(attackDamage);
    }

    private void HandleMonsterDefeat(Monster selectedMonster)
    {
        Console.WriteLine($"몬스터 {selectedMonster.Name}를 물리쳤습니다!");
        Console.WriteLine($"경험치 {selectedMonster.Exp}를 얻었습니다!");
        player.GetExp(selectedMonster.Exp);
        player.LevelUp();

        MonsterDied(selectedMonster);

        ConsoleUtility.PrintTextHighlights("경험치 :", $"{player.CurrentExp}/{player.MaxExp}".ToString());
    }

    private bool CheckAllMonstersDead()
    {
        foreach (var monster in battleMonster)
        {
            if (monster.IsAlive())
                return false;
        }
        return true;
    }

    private void HandleAllMonstersDefeat()
    {
        Console.WriteLine();
        Console.WriteLine("모든 몬스터를 물리쳤습니다.");

        ResetMonsters();

        Thread.Sleep(1000);
        Console.WriteLine("아무키나 누르세요...");
        Console.ReadKey();
    }

    private void MonsterAttack()
    {
        Console.WriteLine();
        Console.WriteLine("몬스터 차례");

        double mminAtk = Math.Ceiling(player.Atk * 0.9); // 최소 공격력 (원래 공격력의 90%)
        double mmaxAtk = Math.Ceiling(player.Atk * 1.1); // 최대 공격력 (원래 공격력의 110%)

        foreach (Monster m in battleMonster)
        {
            if (m.IsAlive())
            {
                if (IsPlayerEvaded())
                {
                    Console.WriteLine("회피!");
                    Console.WriteLine($"{player.Name}이(가) 공격을 회피했습니다.");
                }
                else
                {
                    int attackDamage = battleRandom.Next((int)mminAtk, (int)mmaxAtk);

                    ConsoleUtility.PrintTextHighlights("", $"{m.Name}이(가) 공격합니다!");

                    if (IsMonsterCriticalHit())
                        InflictMonsterCriticalDamage(m, attackDamage);
                    else
                        InflictMonsterRegularDamage(m, attackDamage);

                    ShowPlayerHpAfterMonsterAttack();
                }
            }
        }
    }

    private bool IsPlayerEvaded()
    {
        return battleRandom.Next(100) < 10;
    }

    private bool IsMonsterCriticalHit()
    {
        return battleRandom.Next(100) < 15;
    }

    private void InflictMonsterCriticalDamage(Monster monster, int attackDamage)
    {
        int criticalDamage = (int)(attackDamage * 1.6);
        Console.WriteLine("치명타 발생! 추가 피해를 입었습니다.");
        Console.WriteLine($"[데미지 : {criticalDamage}]");
        player.PlayerTakeDamage(criticalDamage);
    }

    private void InflictMonsterRegularDamage(Monster monster, int attackDamage)
    {
        Console.WriteLine($"[데미지 : {attackDamage}]");
        player.PlayerTakeDamage(attackDamage);
    }

    private void ShowPlayerHpAfterMonsterAttack()
    {
        Console.WriteLine($"{player.Name}의 HP: {player.CurrentHp}");
        Console.WriteLine();
        Thread.Sleep(1000);
    }

    private void HandlePlayerDefeat()
    {
        Console.WriteLine("전투에서 패배했습니다.");
        Console.WriteLine("마을로 돌아갑니다.");

        ResetMonsters();
        player.InTower = false;
        player.playerdefeat();

        Thread.Sleep(1000);
        Console.ReadKey();
        MainMenu();
    }

    private void ResetMonsters()
    {
        foreach (var monster in battleMonster)
        {
            monster.Reset();
        }
    }


    // 몬스터가 죽었을 때 호출되는 메서드
    public void MonsterDied(Monster selectedMonster)
    {
        // 몬스터가 가지고 있는 아이템의 인덱스를 찾기, 찾지 못하면 -1 반환
        int itemIndex = dungeonItemList.FindIndex(item => item.Name == selectedMonster.DropItem);
        int potionIndex = potion.FindIndex(p => p.Name == selectedMonster.DropItem);

        // 해당 아이템을 찾았는지 확인 후 인벤토리에 추가
        if (itemIndex != -1)
        {
            if (battleRandom.Next(100) < 10) //확률적으로 아이템을 드랍함
            {
                Item droppedItem = dungeonItemList[itemIndex].CloneItem();
                weaponReward.Add(droppedItem);
                Console.WriteLine($"{selectedMonster.DropItem}를 얻었습니다!");
            }
        }

        if (potionIndex != -1)
        {
            if (battleRandom.Next(100) < 101) //확률적으로 아이템을 드랍함
            {
                Potion droppedItem = potion[potionIndex].ClonePotion();
                potionReward.Add(droppedItem);
                Console.WriteLine($"{selectedMonster.DropItem}를 얻었습니다!");
            }
        }
    }

    public void ShowResultMenu(int random)
    {
        Console.Clear();
        ConsoleUtility.ShowTitle("■ 전투 결과 ■");
        Console.WriteLine();

        Console.WriteLine($"던전에서 몬스터 {random}마리를 잡았습니다.");
        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name}({player.Job})");
        Console.WriteLine($"HP {player.CurrentHp} / {player.MaxHp}");
        Console.WriteLine();

        Console.WriteLine($"[전투 보상]");
        Console.WriteLine($"{random * 100}골드를 획득했습니다.");
        player.Gold += (random * 100);

        for (int i = 0; i < weaponReward.Count; i++)
        {
            inventory.Add(weaponReward[i]); // 아이템을 인벤토리에 추가
            Console.WriteLine($"{weaponReward[i].Name}를 얻었습니다!"); // 아이템 이름 출력
        }

        Dictionary<string, int> potionCounts = new Dictionary<string, int>();

        for (int i = 0; i < potionReward.Count; i++)
        {
            string potionName = potionReward[i].Name; //포션의 이름

            if (!potionCounts.ContainsKey(potionName))
            {
                potionCounts[potionName] = 1;
            }
            else
            {
                potionCounts[potionName]++;
            }

            if (!potioninventory.Any(p => p.Name == potionName)) //포션이 인벤토리에 없다면
            {
                Potion newPotion = potionReward[i].ClonePotion(); //복제해서
                newPotion.TogglePurchase(); //복제한걸 증가시켜줌
                potioninventory.Add(newPotion); //증가시킨 포션을 추가
            }
            else //이미 있는 포션이라면
            {
                potioninventory.First(p => p.Name == potionName).TogglePurchase(); //제일 첫번째 있는 이름같은 포션의 개수 증가
            }
        }

        foreach (var kvp in potionCounts)
        {
            Console.WriteLine($"{kvp.Key} {kvp.Value}개를 얻었습니다!");
        }

        //여기에 보상 목록 추가
        potionReward.Clear();
        weaponReward.Clear();
        Console.WriteLine();

        Thread.Sleep(1000);

        if (player.InTower)
        {
            DungeonMenu(++player.NowDongeon);
            Console.WriteLine("2층으로 갑니다.");

        }
        else
        {
            Console.WriteLine("던전 입구로 돌아갑니다.");
        }
        Console.WriteLine("아무키나 누르세요...");
        Console.ReadKey();
        DungeonChoiceMenu();
    }
}