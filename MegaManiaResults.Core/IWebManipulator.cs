using System.Collections.Generic;

namespace MegaManiaResults.Core
{
	public interface IWebManipulator
	{
		Dictionary<int, int> Results { get; }

		int[] GetTopNumbers(int topQuantity = 30);

		void LoadResults();
	}
}