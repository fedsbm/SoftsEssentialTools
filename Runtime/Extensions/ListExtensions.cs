using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Packages.SoftsEssentialKit.Runtime.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Packages.SoftsEssentialKit.Runtime.Extensions
{
    public static class ListExtensions
    {
        private static Dictionary<string, List<int>> indexHistory = new Dictionary<string, List<int>>();

        public static List<T> _GetAllPublicConstantValues<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                .Select(x => (T) x.GetRawConstantValue())
                .ToList();
        }

        public static float _GetTotal(this List<float> list)
        {
            float totalWeight = 0;
            foreach (var value in list)
            {
                totalWeight += value;
            }

            return totalWeight;
        }

        public static int _GetRandomIndexByWeight(this List<float> weights)
        {
            float totalWeight = 0f;
            for (int i = 0; i < weights.Count; i++)
            {
                totalWeight += weights[i];
            }

            float r = Random.Range(0, totalWeight);
            float weightSum = 0;
            int chosenIndex = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                if (r >= weightSum && r < weightSum + weights[i])
                {
                    chosenIndex = i;
                }

                weightSum += weights[i];
            }

            return chosenIndex;
        }

        /// <summary>
        /// Get a random value
        /// </summary>
        /// <returns></returns>
        public static T _Random<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                return default(T);
            }

            return array[Random.Range(0, array.Length)];
        }

        /// <summary>
        /// Get a random value
        /// </summary>
        /// <returns></returns>
        public static T _Random<T>(this List<T> array)
        {
            if (array == null || array.Count == 0)
            {
                return default(T);
            }

            return array[Random.Range(0, array.Count)];
        }

        /// <summary>
        /// Get random value with history of half of the list total count, this history can be saved using _SaveHistory()
        /// and _LoadHistory() functions.
        /// Conflicts risk: Because it's a static function, lists of the same kind would point for the same history var,
        /// use a unique history id to avoid this problem.
        /// </summary>
        /// <param name="list">The list referenced in this extension</param>
        /// <param name="historyID">The key to save the array, avoid using empty ids due risk of conflicts</param>
        /// <returns></returns>
        public static T _RandomHistory<T>(this List<T> list, string historyID)
        {
            return _RandomHistory(list.ToArray(), historyID);
        }

        /// <summary>
        /// Get random value with history of half of the array total length, this history can be saved using _SaveHistory()
        /// and _LoadHistory() functions.
        /// Conflicts risk: Because it's a static function, lists of the same kind would point for the same history var,
        /// use a unique history id to avoid this problem.
        /// </summary>
        /// <param name="array">The array referenced in this extension</param>
        /// <param name="historyID">The key to save the array, avoid using empty ids due risk of conflicts</param>
        /// <returns></returns>
        public static T _RandomHistory<T>(this T[] array, string historyID)
        {
            // Get previously used clips
            List<int> arrayHistory = indexHistory.ContainsKey(historyID) ? indexHistory[historyID] : null;

            // if null or zero, load from prefs
            if (arrayHistory._IsNullOrEmpty())
            {
                _LoadRndHistory(array, historyID);
                arrayHistory = indexHistory[historyID];
            }

            // Trim list to half array length
            while (arrayHistory.Count > array.Length / 2)
            {
                arrayHistory.RemoveAt(0);
            }

            // Get all valid indexes
            List<int> validIndexes = new List<int>();
            for (int x = 0; x < array.Length; x++)
            {
                if (arrayHistory.Contains(x))
                {
                    continue;
                }

                validIndexes.Add(x);
            }

            // Choose a valid index to use
            int chosenIdex = validIndexes._Random();

            // Save locally used index -> To use cross sessions use _SaveRndHistory
            arrayHistory.Add(chosenIdex);
            indexHistory[historyID] = arrayHistory;
            return array[chosenIdex];
        }

        /// <summary>
        /// If found index history for the provided key id, it saves as PlayerPrefs and return true, otherwise returns false
        /// </summary>
        /// <param name="list">The list referenced in this extension</param>
        /// <param name="historyID">The key to save the array</param>
        /// <returns></returns>
        public static bool _SaveRndHistory<T>(this List<T> list, string historyID)
        {
            return list == null ? new T[0]._SaveRndHistory(historyID) : list.ToArray()._SaveRndHistory(historyID);
        }

        /// <summary>
        /// If found index history for the provided key id, it saves as PlayerPrefs and return true, otherwise returns false
        /// </summary>
        /// <param name="array">The array referenced in this extension</param>
        /// <param name="historyID">The key to save the array</param>
        /// <returns></returns>
        public static bool _SaveRndHistory<T>(this T[] array, string historyID)
        {
            if (!indexHistory.ContainsKey(historyID))
            {
                return false;
            }

            PlayerPrefs.SetString(historyID,
                indexHistory[historyID]._IsNullOrEmpty() ? "" : MiscellaneousUtils.ArrayToCsv(indexHistory[historyID]));
            return true;
        }

        /// <summary>
        /// Mark a certain element as used on the history, return true if the item is now on the historic.
        /// This not save the history automatically, to be used cross game session call _SaveRndHistory
        /// </summary>
        /// <param name="list">The list referenced in this extension</param>
        /// <param name="element"> The element to be marked as used</param>
        /// <param name="historyID">The key to save the array</param>
        /// <returns></returns>
        public static bool _MarkHistoryAsUsed<T>(this List<T> list, T element, string historyID)
        {
            return _MarkHistoryAsUsed(list.ToArray(), element, historyID);
        }

        /// <summary>
        /// Mark a certain element as used on the history, return true if the item is now on the historic.
        /// This not save the history automatically, to be used cross game session call _SaveRndHistory
        /// </summary>
        /// <param name="list">The array referenced in this extension</param>
        /// <param name="element"> The element to be marked as used</param>
        /// <param name="historyID">The key to save the array</param>
        /// <returns></returns>
        public static bool _MarkHistoryAsUsed<T>(this T[] array, T element, string historyID)
        {
            if (array._IsNullOrEmpty() || !array.Contains(element)) return false;

            // Get previously used clips
            List<int> arrayHistory = indexHistory.ContainsKey(historyID) ? indexHistory[historyID] : null;

            // if null or zero, load from prefs
            if (arrayHistory._IsNullOrEmpty())
            {
                _LoadRndHistory(array, historyID);
                arrayHistory = indexHistory[historyID];
            }

            int elementIndex = array.ToList().IndexOf(element);

            if (arrayHistory.Contains(elementIndex)) return true;

            // Save locally used index -> To use cross sessions use _SaveRndHistory
            arrayHistory.Add(elementIndex);
            indexHistory[historyID] = arrayHistory;
            return true;
        }

        /// <summary>
        /// If found index history for the provided key id, it will be cleared
        /// </summary>
        /// <param name="list">The list referenced in this extension</param>
        /// <param name="historyID">The key to save the array</param>
        /// <returns></returns>
        public static bool _ClearRndHistory<T>(this List<T> list, string historyID)
        {
            if (!indexHistory.ContainsKey(historyID))
            {
                return false;
            }

            indexHistory[historyID].Clear();
            return true;
        }

        /// <summary>
        /// If found index history for the provided key id, it will be cleared
        /// </summary>
        /// <param name="array">The array referenced in this extension</param>
        /// <param name="historyID">The key to save the array</param>
        /// <returns></returns>
        public static bool _ClearRndHistory<T>(this T[] array, string historyID)
        {
            if (!indexHistory.ContainsKey(historyID))
            {
                return false;
            }

            indexHistory[historyID].Clear();
            return true;
        }

        /// <summary>
        /// Try to load index history for this list with given ID, if the saved data exists returns true, otherwise false
        /// </summary>
        /// <param name="array">The array referenced in this extension</param>
        /// <param name="historyID">The key to save the array</param>
        /// <returns></returns>
        public static bool _LoadRndHistory<T>(this List<T> array, string historyID)
        {
            return LoadRndHistory(historyID);
        }

        /// <summary>
        /// Try to load index history for this array with given ID, if the saved data exists returns true, otherwise false
        /// </summary>
        /// <param name="array">The array referenced in this extension</param>
        /// <param name="historyID">The key to save the array</param>
        /// <returns></returns>
        public static bool _LoadRndHistory<T>(this T[] array, string historyID)
        {
            return LoadRndHistory(historyID);
        }

        private static bool LoadRndHistory(string historyId)
        {
            if (indexHistory.ContainsKey(historyId))
            {
                indexHistory.Remove(historyId);
            }

            var list = new List<int>(MiscellaneousUtils.intArrayFromCsv(PlayerPrefs.GetString(historyId, "")));
            indexHistory.Add(historyId, list);
            return indexHistory[historyId]._IsNullOrEmpty();
        }

        public static T _GetRandomKeyByWeight<T>(this T[] weightedKeys) where T : KeyAndWeight
        {
            float totalWeight = 0f;
            for (int x = 0; x < weightedKeys.Length; x++)
            {
                totalWeight += weightedKeys[x].Weight;
            }

            float r = Random.value * totalWeight;
            float weightSum = 0;
            for (int i = 0; i < weightedKeys.Length; i++)
            {
                if (r <= weightSum + weightedKeys[i].Weight)
                {
                    return weightedKeys[i];
                }

                weightSum += weightedKeys[i].Weight;
            }

            return null;
        }

        public static T _GetRandomKeyByWeight<T>(this List<T> weightedKeys) where T : KeyAndWeight
        {
            float totalWeight = 0f;
            for (int x = 0; x < weightedKeys.Count; x++)
            {
                totalWeight += weightedKeys[x].Weight;
            }

            float r = Random.value * totalWeight;
            float weightSum = 0;
            for (int i = 0; i < weightedKeys.Count; i++)
            {
                if (r <= weightSum + weightedKeys[i].Weight)
                {
                    return weightedKeys[i];
                }

                weightSum += weightedKeys[i].Weight;
            }

            return null;
        }

        public static void RemoveEntriesOnCooldown<T>(this List<T> weightedKeys, string savePrefix)
            where T : KeyWeightAndCooldown
        {
            for (int x = weightedKeys.Count - 1; x >= 0; x--)
            {
                if (weightedKeys[x].Cooldown <= 0)
                {
                    continue;
                }

                int cooldown = PlayerPrefs.GetInt(savePrefix + weightedKeys[x].Key, 0);
                if (cooldown > 0)
                {
                    PlayerPrefs.SetInt(savePrefix + weightedKeys[x].Key, cooldown - 1);
                    weightedKeys.RemoveAt(x);
                }
            }
        }

        /*
     * Shuffle the content of a list
     */
        public static List<T> _Shuffle<T>(this List<T> sourceList)
        {
            List<T> randomizedList = new List<T>();
            for (int i = sourceList.Count; i > 0; i--)
            {
                int index = Random.Range(0, i);
                T randomValue = sourceList[index];
                randomizedList.Add(randomValue);
                sourceList.Remove(randomValue);
            }

            sourceList.AddRange(randomizedList);
            return sourceList;
        }

        public static string _ToCSV<T>(this T[] array)
        {
            return MiscellaneousUtils.ArrayToCsv(array);
        }

        public static string _ToCSV<T>(this List<T> array)
        {
            return MiscellaneousUtils.ArrayToCsv(array);
        }

        public static List<int> _GetIndexesList<T>(this List<T> array)
        {
            List<int> indexes = new List<int>();
            for (var i = 0; i < array.Count; i++)
            {
                indexes.Add(i);
            }

            return indexes;
        }

        /// <summary>
        /// Returns <c>true</c> if the list is either null or empty. Otherwise <c>false</c>.
        /// </summary>
        /// <param name="list">The list.</param>
        public static bool _IsNullOrEmpty<T>(this IList<T> list)
        {
            if (list != null)
                return list.Count == 0;
            return true;
        }
    }
}