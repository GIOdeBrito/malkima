using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Cor
	{
		private static Color[] cores;

		static Cor ()
		{
			cores = new Color[]
			{
				Color.FromArgb(255,255,255),
				Color.FromArgb(235,128,40),
				Color.FromArgb(230,18,18),
				Color.FromArgb(200,30,30,30),
			};
		}

		public static Color UsarCor (int index = -1)
		{
			return cores[index];
		}
	}
}
