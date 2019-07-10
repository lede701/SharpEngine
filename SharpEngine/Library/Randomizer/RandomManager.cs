using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEngine.Library.Randomizer
{
	public class RandomManager
	{
		private Random _rnd;
		private static RandomManager _instance;
		public static RandomManager Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new RandomManager();
				}

				return _instance;
			}
		}

		private RandomManager()
		{
			_rnd = new Random(Guid.NewGuid().GetHashCode());
		}

		public static Random Random
		{
			get
			{
				return _instance._rnd;
			}
		}

	}
}
