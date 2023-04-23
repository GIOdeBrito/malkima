using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Inicio
	{	
		private static Panel _categorias = null;
		private static Panel _painel = null;
		public static string[] tipos = {
			"Steam",
			"Emuladores",
			"Programas",
			"Outros",
		};
		
		public static void Inicializar ()
		{
			Form f = Form1.UsarForma();
			Tuple<int,int> xy = Utis.TelaXY();
			int x = xy.Item1;
			int y = xy.Item2;

			f.BackColor = Color.FromArgb(37,37,37);
			f.BackgroundImage = Image.FromFile(@"dir6/graficos/panorama.png");
			f.BackgroundImageLayout = ImageLayout.Stretch;

			int posx = x - (x * 1 / 100);
			int posy = y - (y * 16 / 100);

			_categorias = new Panel()
			{
				Size = new Size(posx / 6, y),
				Location = new Point(0, 0),
				BackColor = Color.FromArgb(200,30,30,30),
				Enabled = true,
			};
			_painel = new Panel()
			{
				Size = new Size(posx - (posx / 5), posy),
				Location = new Point(posx / 5, 60),
				BackColor = Color.FromArgb(200,30,30,30),
				Enabled = false,
			};

			_painel.AutoScroll = true;
			_painel.Visible = false;

			Utis.CriarBotaoRedondo(_categorias,1);
			Utis.CriarBotaoRedondo(_painel,10);

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
			Tuple<int,int> xy = Utis.TelaXY();
			int x = xy.Item1;
			int y = xy.Item2;

			//Color cor_texto = Cores.UsarCor(Cores.UsarTema().texto);
			Color cor_icone = Color.White;
			
			var itens = new List<dynamic>()
			{
				new PictureBox()
				{
					Name = "adicionar",
					Location = new Point(_categorias.Width/2 - 20, 10),
					Size = new Size(40,40),
					BackColor = Color.Transparent,
					BackgroundImageLayout = ImageLayout.Stretch,
					BackgroundImage = Utis.ImagemComCor(@"dir6/graficos/controle.png", cor_icone),
				},
				new PictureBox()
				{
					Name = "sair",
					Location = new Point(x - 29, 5),
					Size = new Size(24,24),
					BackColor = Color.Transparent,
					BackgroundImageLayout = ImageLayout.Stretch,
					BackgroundImage = Utis.ImagemComCor(@"dir6/graficos/sair.png", cor_icone),
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

			DadosXML.GravarXML(@"dir6/test.xml",new JogoItem()
			{
				nome = "gio",
				id = "bruno",
			});

			itens.Clear();
		}

		private static void CriarCategorias ()
		{
			List<Button> b = new List<Button>();

			foreach(var tipo in tipos)
			{
				int posy = 60;

				if(b.Count > 0)
				{
					posy = b[b.Count - 1].Location.Y + 35;
				}
				
				b.Add(new Button()
				{
					Text = tipo,
					Name = tipo.ToLower(),
					//BackColor = Color.FromArgb(20,100,5),
					Size = new Size(_categorias.Width - 20, 30),
					Location = new Point(10,posy),
					Cursor = Cursors.Hand,
					Enabled = true,
					Font = Fontes.AplicarFontes(),
				});
			}

			b[0].Click += (s,e) =>
			{
				//_painel.Visible = !_painel.Visible;
				Gerir.IniciarAplicacao("cmd.exe","ipconfig");
			};

			foreach(var botao in b)
			{
				Utis.CriarBotaoRedondo(botao,1);
				_categorias.Controls.Add(botao);
				_categorias.Controls.SetChildIndex(botao, _categorias.Controls.Count - 1);
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

			img[1].Click += (s,e) => { Fechar(); };
		}

		public static void Minimizar ()
		{
			Form1.UsarForma().WindowState = FormWindowState.Minimized;
		}

		public static void Fechar ()
		{
			System.Windows.Forms.Application.Exit();
		}
	}
}
