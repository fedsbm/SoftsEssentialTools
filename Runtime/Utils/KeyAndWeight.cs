using System;

namespace Packages.SoftsEssentialKit.Runtime.Utils
{
	[Serializable]
	public class KeyAndWeight 
	{
		public string Key = "";
		public float Weight = 1;
	}

	[Serializable]
	public class KeyWeightAndCooldown : KeyAndWeight 
	{
		public int Cooldown = 0;
	}
}