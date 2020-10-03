using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Packages.SoftsEssentialKit.Runtime.Utils
{
	public class MiscellaneousUtils
	{
		private const char CSV_DELIMITER = ',';

		public static string ArrayToCsv<T>(T[] array)
		{
			StringBuilder sb = new StringBuilder();

			for(int x = 0; x < array.Length; x++)
			{
				sb.Append(array[x]);

				if(x < array.Length - 1)
				{
					sb.Append(CSV_DELIMITER);
				}
			}

			return sb.ToString();
		}

		public static string ArrayToCsv<T>(List<T> list)
		{
			StringBuilder sb = new StringBuilder();

			for(int x = 0; x < list.Count; x++)
			{
				sb.Append(list[x]);

				if(x < list.Count - 1)
				{
					sb.Append(CSV_DELIMITER);
				}
			}

			return sb.ToString();
		}

		public static int[] intArrayFromCsv(string csvString)
		{
			if(csvString == null || csvString.Length == 0)
			{
				return new int[0];
			}

			string[] elements = csvString.Split(CSV_DELIMITER);

			int[] returnArray = new int[elements.Length];

			for(int x = 0; x < returnArray.Length; x++)
			{
				if(!int.TryParse(elements[x],
					System.Globalization.NumberStyles.Any,
					System.Globalization.CultureInfo.InvariantCulture,
					out returnArray[x]))
				{
					returnArray[x] = 0;
				}
			}

			return returnArray;
		}

		public static string[] stringArrayFromCsv(string csvString)
		{
			if(csvString == null || csvString.Length == 0)
			{
				return new string[0];
			}
			return csvString.Split(CSV_DELIMITER);
		}

		public static float ZeroOneSinWave
		{
			get
			{
				return (SinWave + 1f) * 0.5f;
			}
		}

		public static float SinWave
		{
			get
			{
				return Mathf.Sin(Mathf.Deg2Rad * 90f * Time.realtimeSinceStartup);
			}
		}

		public static void AssetIsNotNull(object obj, string message = "Object was null!")
		{
			if(obj == null)
			{
				Debug.LogException(new System.Exception(message));
			}
		}
	}
}
