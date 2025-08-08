using UnityEngine;
using System.Collections.Generic;
namespace Spine.Unity
{
    public class FashionGameIntegration : MonoBehaviour
    {
        [Header("Game Integration")]
        public int fashionItemsPerLevel = 3;
        public float fashionDropChance = 0.3f;
        public FashionRarity maxDropRarity = FashionRarity.Rare;

        [Header("Level Rewards")]
        public List<LevelFashionReward> levelRewards = new List<LevelFashionReward>();

        [Header("Match-3 Fashion Events")]
        public bool enableFashionMatches = true;
        public int fashionMatchBonus = 100; // Bonus points for fashion-themed matches

        private void Start()
        {
            // Subscribe to game events
            if (LevelManager.THIS != null)
            {
                // Hook into level completion events if available
            }

            SetupLevelRewards();
        }

        void SetupLevelRewards()
        {
            // Thiết lập phần thưởng fashion items cho các level
            if (levelRewards.Count == 0)
            {
                CreateDefaultLevelRewards();
            }
        }

        void CreateDefaultLevelRewards()
        {
            // Level 1-5: Common items
            for (int i = 1; i <= 5; i++)
            {
                AddLevelReward(i, FashionRarity.Common, 1, 50);
            }

            // Level 6-10: Uncommon items
            for (int i = 6; i <= 10; i++)
            {
                AddLevelReward(i, FashionRarity.Uncommon, 1, 100);
            }

            // Level 11-20: Rare items
            for (int i = 11; i <= 20; i++)
            {
                AddLevelReward(i, FashionRarity.Rare, 1, 200);
            }

            // Special level rewards
            AddLevelReward(25, FashionRarity.Epic, 1, 500);
            AddLevelReward(50, FashionRarity.Legendary, 1, 1000);
        }

        void AddLevelReward(int level, FashionRarity rarity, int itemCount, int coinReward)
        {
            LevelFashionReward reward = new LevelFashionReward();
            reward.level = level;
            reward.fashionRarity = rarity;
            reward.itemCount = itemCount;
            reward.coinReward = coinReward;
            levelRewards.Add(reward);
        }

        public void OnLevelCompleted(int levelNumber, int score, int stars)
        {
            // Gọi method này khi player hoàn thành level
            GiveLevelRewards(levelNumber, stars);
            GiveRandomFashionItems(stars);

            Debug.Log($"Level {levelNumber} completed with {stars} stars!");
        }

        void GiveLevelRewards(int levelNumber, int stars)
        {
            var reward = levelRewards.Find(x => x.level == levelNumber);
            if (reward != null)
            {
                // Give fashion items
                List<FashionItem> randomItems = new List<FashionItem>();
                if (FashionManager.Instance != null && FashionManager.Instance.fashionDatabase != null)
                {
                    randomItems = FashionManager.Instance.fashionDatabase.GetRandomItems(
                        reward.itemCount, reward.fashionRarity);

                    foreach (var item in randomItems)
                    {
                        FashionManager.Instance.AddItemToInventory(item);
                    }
                }

                // Give coins
                if (FashionManager.Instance != null)
                {
                    int totalCoins = reward.coinReward * stars; // Bonus for more stars
                    FashionManager.Instance.AddCoins(totalCoins);

                    Debug.Log($"Received {randomItems.Count} fashion items and {totalCoins} coins!");
                }
            }
        }

        void GiveRandomFashionItems(int stars)
        {
            if (FashionManager.Instance == null || FashionManager.Instance.fashionDatabase == null) return;

            // Chance to get bonus fashion items based on stars
            int bonusItems = 0;
            float dropChance = fashionDropChance;

            for (int i = 0; i < stars; i++)
            {
                if (Random.Range(0f, 1f) < dropChance)
                {
                    bonusItems++;
                    dropChance *= 0.5f; // Giảm chance cho item tiếp theo
                }
            }

            if (bonusItems > 0)
            {
                var bonusFashionItems = FashionManager.Instance.fashionDatabase.GetRandomItems(bonusItems, maxDropRarity);
                foreach (var item in bonusFashionItems)
                {
                    FashionManager.Instance.AddItemToInventory(item);
                }

                Debug.Log($"Bonus! Received {bonusItems} extra fashion items!");
            }
        }

        public void OnSpecialMatch(List<Item> matchedItems)
        {
            // Gọi method này khi có special match (4+, 5+, L-shape, T-shape)
            if (!enableFashionMatches) return;

            // Check if this is a fashion-themed match
            if (IsFashionMatch(matchedItems))
            {
                GiveFashionMatchBonus();
            }
        }

        bool IsFashionMatch(List<Item> items)
        {
            // Implement logic để check nếu match này có liên quan đến fashion
            // Có thể dựa vào màu sắc, pattern, hoặc special items

            // Ví dụ: nếu match có 5+ items cùng màu "pink" hoặc "purple" (màu thời trang)
            if (items.Count >= 5)
            {
                int fashionColorCount = 0;
                foreach (var item in items)
                {
                    // Assume color 3 = pink, color 4 = purple (fashion colors)
                    if (item.color == 3 || item.color == 4)
                    {
                        fashionColorCount++;
                    }
                }

                return fashionColorCount >= items.Count * 0.8f; // 80% fashion colors
            }

            return false;
        }

        void GiveFashionMatchBonus()
        {
            if (FashionManager.Instance != null)
            {
                FashionManager.Instance.AddCoins(fashionMatchBonus);

                // Small chance to get a fashion item
                if (Random.Range(0f, 1f) < 0.1f) // 10% chance
                {
                    var bonusItem = FashionManager.Instance.fashionDatabase.GetRandomItems(1, FashionRarity.Uncommon);
                    if (bonusItem.Count > 0)
                    {
                        FashionManager.Instance.AddItemToInventory(bonusItem[0]);
                        Debug.Log($"Fashion Match Bonus! Received {bonusItem[0].itemName}!");
                    }
                }
            }
        }

        public void OnDailyLogin()
        {
            // Daily login rewards cho fashion system
            if (FashionManager.Instance == null) return;

            int lastLoginDay = PlayerPrefs.GetInt("LastLoginDay", 0);
            int currentDay = System.DateTime.Now.DayOfYear;

            if (lastLoginDay != currentDay)
            {
                PlayerPrefs.SetInt("LastLoginDay", currentDay);

                // Give daily fashion rewards
                int dailyCoins = Random.Range(50, 200);
                FashionManager.Instance.AddCoins(dailyCoins);

                // Chance for free fashion item
                if (Random.Range(0f, 1f) < 0.2f) // 20% chance
                {
                    var dailyItem = FashionManager.Instance.fashionDatabase.GetRandomItems(1, FashionRarity.Uncommon);
                    if (dailyItem.Count > 0)
                    {
                        FashionManager.Instance.AddItemToInventory(dailyItem[0]);
                        Debug.Log($"Daily Login Bonus! Received {dailyItem[0].itemName}!");
                    }
                }

                PlayerPrefs.Save();
            }
        }

        public void OnGameStart()
        {
            // Check daily login when game starts
            OnDailyLogin();

            // Check for special events, promotions, etc.
            CheckSpecialEvents();
        }

        void CheckSpecialEvents()
        {
            // Implement special events như fashion week, holiday events, etc.
            // Có thể increase drop rates, special items, limited time offers

            // Example: Weekend fashion event
            System.DayOfWeek today = System.DateTime.Now.DayOfWeek;
            if (today == System.DayOfWeek.Saturday || today == System.DayOfWeek.Sunday)
            {
                fashionDropChance *= 1.5f; // 50% higher drop rate on weekends
                Debug.Log("Weekend Fashion Event Active! Higher fashion drop rates!");
            }
        }

        // Method để integrate với story mode nếu có
        public void OnStoryProgress(int chapterNumber, int storyProgress)
        {
            // Unlock special fashion items based on story progress
            if (FashionManager.Instance != null && FashionManager.Instance.fashionDatabase != null)
            {
                var storyItems = GetStoryUnlockItems(chapterNumber, storyProgress);
                foreach (var item in storyItems)
                {
                    item.isUnlocked = true;
                    FashionManager.Instance.AddItemToInventory(item);
                    Debug.Log($"Story unlock! Received {item.itemName}!");
                }
            }
        }

        List<FashionItem> GetStoryUnlockItems(int chapter, int progress)
        {
            // Return fashion items that should be unlocked at this story point
            var unlockItems = new List<FashionItem>();

            // Example logic
            if (chapter == 1 && progress == 100) // Complete chapter 1
            {
                // Unlock special dress
                var specialItem = FashionManager.Instance.fashionDatabase.GetItemById(1004); // Evening dress
                if (specialItem != null)
                {
                    unlockItems.Add(FashionManager.Instance.fashionDatabase.CreateItemCopy(specialItem));
                }
            }

            return unlockItems;
        }
    }

    [System.Serializable]
    public class LevelFashionReward
    {
        public int level;
        public FashionRarity fashionRarity;
        public int itemCount;
        public int coinReward;
    }
}
