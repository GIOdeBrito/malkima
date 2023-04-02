using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Gerir
	{
		public static void AdicionarItem ()
		{
			Panel p = Inicio.UsarPainel();

			List<Button> b = new List<Button>();


			for(int i = 0; i < 45; i++)
			{
				b.Add(new Button() {

					Text = "AAAAAAAAAA",
					Location = new Point(100,i*10),
					Size = new Size(90,20),
				});
				p.Controls.Add(b[i]);
				//Console.WriteLine(b.Text);
			}
		}
	}
}
