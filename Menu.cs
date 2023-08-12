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

		public static void CriarMenu ()
		{
			/*
			Cria o menu principal da aplicação
			*/
			
			Inicio.CarregarJogos();
			
			Form f = Form1.UsarForma();
			Tuple<int,int> xy = Utis.TelaXY();
			int x = xy.Item1, y = xy.Item2;

			f.BackColor = Color.FromArgb(37,37,37);
			f.BackgroundImage = Utis.GIOImagem(@"dir6/graficos/panorama.png");
			f.BackgroundImageLayout = ImageLayout.Stretch;

			int posx = x - (x * 1 / 100);
			int posy = y - (y * 16 / 100);

			_categorias = new Panel()
			{
				Name = "categorias",
				Size = new Size(posx / 6, y),
				Location = new Point(0, 0),
				BackColor = Cor.UsarCor(3),
				Enabled = true,
			};
			_painel = new Panel()
			{
				Name = "jogos painel",
				Size = new Size(posx - (posx / 5), posy),
				Location = new Point(posx / 5, 60),
				BackColor = Cor.UsarCor(3),
				Enabled = true,
			};

			_painel.AutoScroll = true;
			_painel.Visible = false;

			GUI.CriarBotaoRedondo(_categorias,1);
			GUI.CriarBotaoRedondo(_painel,10);

			f.Controls.Add(_categorias);
			f.Controls.Add(_painel);
			_categorias.SendToBack();

			CriarCategorias();
			DefinirMenu();
		}

		public static Panel UsarPainel ()
		{
			return _painel;
		}

		private static void DefinirMenu ()
		{
			Form f = Form1.UsarForma();
			Tuple<int,int> di_tela = Utis.TelaXY();
			int x = di_tela.Item1;
			
			var itens = new List<dynamic>()
			{
				new PictureBox()
				{
					Name = "adicionar",
					Location = new Point(_categorias.Width/2 - 20, 10),
					Size = new Size(40,40),
					BackColor = Color.Transparent,
					BackgroundImageLayout = ImageLayout.Stretch,
					BackgroundImage = Utis.ImagemComCor(@"dir6/graficos/controle.png", Cor.UsarCor(1)),
				},
				new PictureBox()
				{
					Name = "sair",
					Location = new Point(x - 29, 5),
					Size = new Size(24,24),
					BackColor = Color.Transparent,
					BackgroundImageLayout = ImageLayout.Stretch,
					BackgroundImage = Utis.GIOImagem(@"dir6/graficos/sair.png"),
					Cursor = Cursors.Hand,
				},
			};

			foreach(var it in itens)
			{
				if(it.Name == "adicionar")
				{
					_categorias.Controls.Add(it);
					continue;
				}

				f.Controls.Add(it);
			}

			DefinirControles(itens.ToArray());
			itens.Clear();
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

		private static void DefinirControles (dynamic[] itens)
		{
			PictureBox[] img = 
			{
				Array.Find(itens, item => item.Name == "adicionar"),
				Array.Find(itens, item => item.Name == "sair"),
			};
			Action<PictureBox,Color> mudarFundo = (elem,cor) =>
			{
				elem.BackColor = cor;
			};

			img[0].MouseEnter += (s,e) => { mudarFundo(img[0],Color.Gray); };
			img[0].MouseLeave += (s,e) => { mudarFundo(img[0],Color.Transparent); };
			img[0].Click += (s,e) => { Janela.AdicionarJogo(); };
			img[1].Click += (s,e) => { Form1.Fechar(); };
		}
	}
}
