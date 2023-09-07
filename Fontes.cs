using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Fontes
	{
		private static PrivateFontCollection fontes = new PrivateFontCollection();

		static Fontes ()
		{
			LoadFonts();
		}

		public static Font AplicarFontes (int sz = 11)
		{
			return new Font(fontes.Families[0], sz);
		}

		public static void LoadFonts ()
		{
			string[] fontspath =
			{
				@"fontes/acephimere.otf"
			};

			foreach(string f in fontspath)
			{
				fontes.AddFontFile(f);
			}
		}

		public static string[] PanFonts ()
		{
			List<string> names = new List<string>();
			
			foreach(FontFamily f in fontes.Families)
			{
				//Console.WriteLine(f.Name);
				names.Add(f.Name);
			}

			return names.ToArray();
		}
	}
}
