using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Inicio
	{	
		private static Panel painel = null;
		
		public static void Inicializar ()
		{
			Form f = Form1.UsarForma();
			Tuple<int,int> xy = Utis.TelaXY();
			int x = xy.Item1;
			int y = xy.Item2;

			Color cor_borda = Cores.UsarCor(Cores.UsarTema().borda);

			f.BackColor = Color.Black;

			DefinirMenu();

			int p_x = x - 120;
			int p_y = y/2 - 100;
			
			Button painelRaiz = new Button()
			{
				Bounds = new Rectangle(50, y/2, p_x, p_y),
				ForeColor = cor_borda,
				FlatStyle = FlatStyle.Flat,
				Enabled = false,
			};

			painel = new Panel()
			{
				Location = new Point(55, y/2 + 5),
				Size = new Size(p_x - 9, p_y - 9),
				BackColor = Color.Black,
				//BackColor = Color.Red,
				Enabled = true,
			};

			painel.AutoScroll = true;

			//painelRaiz.Controls.Add(painel);
			f.Controls.Add(painelRaiz);
			f.Controls.Add(painel);
			painelRaiz.SendToBack();
		}

		public static Panel UsarPainel ()
		{
			return painel;
		}

		public static void DefinirMenu ()
		{
			Form f = Form1.UsarForma();
			Tuple<int,int> xy = Utis.TelaXY();
			int x = xy.Item1;
			int y = xy.Item2;

			Color cor_texto = Cores.UsarCor(Cores.UsarTema().texto);
			Color cor_icone = Cores.UsarCor(Cores.UsarTema().icone);
			
			var itens = new dynamic[] {
				new Label()
				{
					Name = "jogos_texto",
					ForeColor = cor_texto,
					Text = "Jogos",
					Location = new Point(100, y/2 - 15),
					Size = new Size(100,30),
					Font = Fontes.AplicarFontes(20),
					TextAlign = ContentAlignment.MiddleCenter,
				},
				new PictureBox()
				{
					Name = "icone",
					Location = new Point(10,0),
					Size = new Size(128,128),
					BackColor = Color.Transparent,
					BackgroundImageLayout = ImageLayout.Stretch,
					BackgroundImage = Utis.ImagemComCor(@$"dir6/malka.png", cor_icone),
				},
				new PictureBox()
				{
					Name = "adicionar",
					Location = new Point(x/2, 10),
					Size = new Size(64,64),
					BackColor = Color.Transparent,
					BackgroundImageLayout = ImageLayout.Stretch,
					BackgroundImage = Utis.ImagemComCor(@$"dir6/graficos/controle.png", cor_icone),
				},
			};

			foreach(var it in itens)
			{
				f.Controls.Add(it);
			}

			DefinirControles(itens);
		}

		private static void DefinirControles (dynamic[] itens)
		{
			PictureBox controle = Array.Find(itens, item => item.Name == "adicionar");
	
			Action<PictureBox,Color> mudarFundo = (elem,cor) => {

				elem.BackColor = cor;
			};

			controle.MouseEnter += (s,e) => { mudarFundo(controle,Color.Gray); };
			controle.MouseLeave += (s,e) => { mudarFundo(controle,Color.Transparent); };
			controle.Click += (s,e) => {

				Gerir.AdicionarItem();
			};
		}
	}
}
