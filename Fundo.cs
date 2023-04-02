using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Fundo
	{
		private static PictureBox[] chuva;

		public static void IniciarChuva ()
		{
			Form f = Form1.UsarForma();
			Tuple<int,int> tela = Utis.TelaXY();

			Color cor_particula = Cores.UsarCor(Cores.UsarTema().particula);
			Image img = Utis.ImagemComCor(@"dir6/graficos/goticula.png",cor_particula);
			
			List<PictureBox> lista = new List<PictureBox>();

			for(int i = 0; i < 10; i++)
			{
				lista.Add(new PictureBox() {
					
					Location = new Point(0,0),
					BackgroundImage = img,
					Size = new Size(32,32),
				});
			}

			chuva = lista.ToArray();
			lista.Clear();

			for(int i = 0; i < chuva.Length; i++)
			{
				f.Controls.Add(chuva[i]);
				RedefinirPingo(i);
			}
		}

		public static void Chover ()
		{
			Tuple<int,int> tela = Utis.TelaXY();

			for(int i = 0; i < chuva.Length; i++)
			{
				int x = chuva[i].Location.X;
				int y = chuva[i].Location.Y;

				if(x > tela.Item1 || y > tela.Item2)
				{
					RedefinirPingo(i);
					return;
				}

				x += 5;
				y += 3;
				
				chuva[i].Location = new Point(x,y);
			}
		}

		private static void RedefinirPingo (int i = -1)
		{
			if(i < 0)
			{
				return;
			}

			Tuple<int,int> tela = Utis.TelaXY();

			int x = Utis.InteiroAleatorio(0,tela.Item1)*-1;
			int y = Utis.InteiroAleatorio(0,tela.Item2)*-1;

			chuva[i].Location = new Point(x,y);
		}
	}
}
