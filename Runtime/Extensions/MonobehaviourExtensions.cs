using System;
using System.Collections;
using Packages.SoftsEssentialKit.Runtime.Utils;
using UnityEngine;

namespace Packages.SoftsEssentialKit.Runtime.Extensions
{
	public static class MonobehaviourExtensions
	{
		/******************************************************************************************************************
	 ********************************************* INTERPOLATOR ******************************************************* 
	 ******************************************************************************************************************/

		#region Easings.Functions.Interpolator

		public static void _Interpolate(this MonoBehaviour monoBehaviour, float from, float to, Action<float> interaction,
			float duration, Easings.Functions interpolator = Easings.Functions.Linear, Action<float> onFinish = null)
		{
			monoBehaviour.StartCoroutine(ExecuteInterpolation(interpolator, duration, interaction, from, to,
				onFinish, Mathf.Lerp));
		}

		public static void _Interpolate(this MonoBehaviour monoBehaviour, Vector2 from, Vector2 to,
			Action<Vector2> interaction, float duration, Easings.Functions interpolator = Easings.Functions.Linear,
			Action<Vector2> onFinish = null)
		{
			monoBehaviour.StartCoroutine(ExecuteInterpolation(interpolator, duration, interaction, from, to,
				onFinish, Vector2.Lerp));
		}

		public static void _Interpolate(this MonoBehaviour monoBehaviour, Vector3 from, Vector3 to,
			Action<Vector3> interaction, float duration, Easings.Functions interpolator = Easings.Functions.Linear,
			Action<Vector3> onFinish = null)
		{
			monoBehaviour.StartCoroutine(ExecuteInterpolation(interpolator, duration, interaction, from, to,
				onFinish, Vector3.Lerp));
		}

		private static IEnumerator ExecuteInterpolation<T>(Easings.Functions interpolator, float duration,
			Action<T> interaction, T from, T to, Action<T> OnFinish, Func<T, T, float, T> formula)
		{
			float sT = Time.time;
			float eT = sT + duration;

			while(Time.time < eT)
			{
				float t = (Time.time - sT) / duration;
				float interpolatedValue = Easings.Interpolate(t, interpolator);
				var value = formula(from, to, interpolatedValue) ;
				interaction(value);
				yield return new WaitForEndOfFrame();
			}

			interaction(to);
			OnFinish?.Invoke(to);
		}

		#endregion


		/// <summary>
		/// Plays an action after a given number of seconds, use this instead of Invoke anytime you need to use params
		/// ex:
		/// Action action = () => { [LOGIC TO BE PLAYED AFTER DELAY]); };
		/// this._PlayDelayed(action, 1f);
		/// WARNING: Always consider to stop coroutines if you want to avoid overlaping
		/// </summary>
		/// <param name="monoBehaviour"></param>
		/// <param name="action"> Action to be performanced after delay</param>
		/// <param name="delay"> Delay in seconds before executing action</param>
		public static void _PlayDelayed(this MonoBehaviour monoBehaviour, Action action, float delay)
		{
			if(delay <= 0f)
			{
				action();
				return;
			}

			if(monoBehaviour == null)
			{
				return;
			}

			monoBehaviour.StartCoroutine(PlayDelayed(action, delay));
		}

		/// <summary>
		/// Stop the last Play Delayed for this monoBehaviour if it is still awaiting to execute the action
		/// </summary>
		/// <param name="monoBehaviour"></param>
		/// <param name="action"></param>
		/// <param name="delay"></param>
		public static void _StopPlayDelayed(this MonoBehaviour monoBehaviour, Action action, float delay)
		{
			monoBehaviour.StopCoroutine(PlayDelayed(action, delay));
		}

		private static IEnumerator PlayDelayed(Action action, float delay)
		{
			yield return new WaitForSecondsRealtime(delay);
			action();
		}
	}
}