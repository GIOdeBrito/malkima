using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Malkima
{
	internal class Menu
	{
		private static Panel _categorias = null;
		private static Panel _painel = null;
		public static string[] tipos =
		{
			"Steam",
			"Emuladores",
			"Programas",
			"Outros",
		};

		public static void StartMenu ()
		{
			CreateMenu();
			SetFormAppearance();
			CreateTopBar();
			CriarCategorias();
			DefinirMenu();
		}

		private static void CreateMenu ()
		{
			/* Cria o menu principal da aplicação */

			Tuple<int,int> xy = Utis.ScreenSize();
			int x = xy.Item1;
			int y = xy.Item2;

			int posx = x - (x * 1 / 100);
			int posy = y - (y * 16 / 100);

			_categorias = new Panel()
			{
				Name = "categorias",
				Size = new Size(posx / 6, y),
				Location = new Point(0, 20),
				BackColor = Cor.UsarCor(3),
				Enabled = true,
			};
			_painel = new Panel()
			{
				Name = "jogos painel",
				Size = new Size(posx - (posx / 5), posy),
				Location = new Point(posx / 5, 65),
				BackColor = Cor.UsarCor(3),
				Enabled = true,
			};

			_painel.AutoScroll = true;
			_painel.Visible = false;

			GUI.CriarBotaoRedondo(_categorias,1);
			GUI.CriarBotaoRedondo(_painel,10);

			Form f = Form1.UsarForma();

			f.Controls.Add(_categorias);
			f.Controls.Add(_painel);
			_categorias.SendToBack();
		}

		public static Panel UsarPainel ()
		{
			return _painel;
		}

		private static void SetFormAppearance ()
		{
			Form f = Form1.UsarForma();
			
			f.BackColor = Color.FromArgb(37,37,37);
			f.BackgroundImage = Utis.LoadImage(@"dir6/graficos/panorama.png");
			f.BackgroundImageLayout = ImageLayout.Stretch;
		}

		private static void CreateTopBar ()
		{
			int screen_width = Utis.ScreenSize().Item1;
			
			Panel topbar = new Panel()
			{
				Name = "topbar",
				Location = new Point(0,0),
				Size = new Size(screen_width, 20),
				BackColor = Color.FromArgb(37,37,37),
			};

			Form1.UsarForma().Controls.Add(topbar);
			topbar.SendToBack();
		}

		private static void DefinirMenu ()
		{
			int x = Utis.ScreenSize().Item1;

			Func<Point,Size,PictureBox> createMenuIcon = (location, sz) =>
			{
				return new PictureBox()
				{
					Location = location,
					Size = sz,
					BackColor = Color.Transparent,
					BackgroundImageLayout = ImageLayout.Stretch,
				};
			};

			PictureBox addGameIcon = createMenuIcon(new Point(_categorias.Width/2 - 20, 10), new Size(40,40));
			addGameIcon.BackgroundImage = Utis.ColouredImage(@"dir6/graficos/controle.png", Cor.UsarCor(1));
			
			PictureBox details = createMenuIcon(new Point(x - 40, 0), new Size(20,20));
			details.BackgroundImage = Utis.LoadImage(@"dir6/graficos/detalhes.png");
			details.Cursor = Cursors.Hand;

			PictureBox quit = createMenuIcon(new Point(x - 20, 0), new Size(20,20));
			quit.BackgroundImage = Utis.LoadImage(@"dir6/graficos/sair.png");
			quit.Cursor = Cursors.Hand;

			Control topbar = Utis.GetElement("topbar");

			_categorias.Controls.Add(addGameIcon);
			topbar.Controls.Add(quit);
			topbar.Controls.Add(details);

			List<dynamic> items = new List<dynamic>()
			{
				addGameIcon,
				details,
				quit,
			};

			DefinirControles(items.ToArray());
			items.Clear();
		}

		private static void CriarCategorias ()
		{
			List<Button> botoes = new List<Button>();

			for(int i = 0; i < tipos.Length; i++)
			{
				int posy = 60;

				if(botoes.Count > 0)
				{
					posy = botoes[botoes.Count - 1].Location.Y + 35;
				}
				
				botoes.Add(new Button()
				{
					Text = tipos[i],
					Name = tipos[i].ToLower(),
					Size = new Size(_categorias.Width - 20, 30),
					Location = new Point(10,posy),
					Cursor = Cursors.Hand,
					Enabled = true,
					Font = Fontes.AplicarFontes(),
				});
			}

			foreach(var botao in botoes)
			{	
				GUI.CriarBotaoRedondo(botao, 1);
				_categorias.Controls.Add(botao);
				_categorias.Controls.SetChildIndex(botao, 2);

				botao.Click += (s,e) =>
				{	
					int index = Array.FindIndex(tipos, item => item == botao.Text);
					GUI.ListarItensPainel(index);
				};
			}
		}

		private static void DefinirControles (dynamic[] elems)
		{
			Action<PictureBox,Color> mudarFundo = (elem,cor) =>
			{
				elem.BackColor = cor;
			};

			PictureBox addgame = elems[0];
			PictureBox details = elems[1];
			PictureBox quit = elems[2];

			addgame.MouseEnter += (s,e) => mudarFundo(addgame,Color.Gray);
			addgame.MouseLeave += (s,e) => mudarFundo(addgame,Color.Transparent);
			addgame.Click += (s,e) => Janela.AdicionarJogo();

			details.MouseEnter += (s,e) => mudarFundo(details,Color.Gray);
			details.MouseLeave += (s,e) => mudarFundo(details,Color.Transparent);
			details.Click += (s,e) => Janela.WindowDetails();

			quit.MouseEnter += (s,e) => mudarFundo(quit,Color.Gray);
			quit.MouseLeave += (s,e) => mudarFundo(quit,Color.Transparent);
			quit.Click += (s,e) => Form1.Fechar();
		}
	}
}
