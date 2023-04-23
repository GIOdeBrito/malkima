using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace Malkima
{
	internal class Janela
	{
		private static Form janelaAtual = null;

		public static void AdicionarJogo ()
		{
			string c = Utis.ColetarArquivo();

			if(string.IsNullOrEmpty(c))
			{
				return;
			}

			string arq_nome = Path.GetFileNameWithoutExtension(c);
			Icon ico = Icon.ExtractAssociatedIcon(c);
			
			CriarJanelaBase(v: new Vector2(290,400),nome: "Adicionar jogo");

			int x = janelaAtual.Width, y = janelaAtual.Height;

			var elementos = new List<dynamic>()
			{
				new Button()
				{
					Name = "adicionar",
					Text = "Salvar",
					Size = new Size(100,30),
					Location = new Point(x/2 - 50, y - 40),
					ForeColor = Color.White,
					FlatStyle = FlatStyle.Flat,
					Font = Fontes.AplicarFontes(),
					Cursor = Cursors.Hand,
				},
				new Button()
				{
					Name = "capa",
					Size = new Size(100,120),
					Location = new Point(x/2 - 50, y/2 - 50),
					ForeColor = Color.White,
					BackColor = Color.Transparent,
					FlatStyle = FlatStyle.Flat,
					Cursor = Cursors.Hand,
					BackgroundImageLayout = ImageLayout.Stretch,
				},
				new TextBox()
				{
					Name = "nome",
					Text = arq_nome,
					Location = new Point(25,45),
					Size = new Size(x - 50,30),
					ForeColor = Color.White,
					BackColor = Color.FromArgb(40,40,40),
					Font = Fontes.AplicarFontes(),
					TextAlign = HorizontalAlignment.Center,
				},
				new TextBox()
				{
					Name = "inic",
					Text = "",
					Location = new Point(25,y/2 + 90),
					Size = new Size(x - 50,30),
					ForeColor = Color.White,
					BackColor = Color.FromArgb(40,40,40),
					Font = Fontes.AplicarFontes(),
					TextAlign = HorizontalAlignment.Center,
				},
				new PictureBox()
				{
					Name = "icone",
					Location = new Point(x/2 - 32,70),
					Size = new Size(64,64),
					BackgroundImage = ico.ToBitmap(),
					BackgroundImageLayout = ImageLayout.Center,
				}
			};

			foreach(var elemento in elementos)
			{
				janelaAtual.Controls.Add(elemento);
			}

			((Button)elementos[0]).Click += (s,e) =>
			{
				if(!Directory.Exists(@"dir6/jogos"))
				{
					Directory.CreateDirectory(@"dir6/jogos");
				}

				string id = arq_nome.Replace(" ","");
				string dir = $@"dir6/jogos/{id}";

				Directory.CreateDirectory(dir);

				((Button)elementos[1]).BackgroundImage.Save($"{dir}/icone.png");
				((PictureBox)elementos[4]).BackgroundImage.Save($"{dir}/capa.png");

				DestruirJanelaBase();
			};

			((Button)elementos[1]).Click += (s,e) =>
			{
				string capa = Utis.ColetarArquivo(1);
				Button b = ((Button)elementos[1]);

				if(string.IsNullOrEmpty(c))
				{
					return;
				}

				if(b.BackgroundImage != null)
				{
					b.BackgroundImage.Dispose();
				}

				Console.WriteLine(capa);
				b.BackgroundImage = Bitmap.FromFile(capa);
			};

			janelaAtual.FormClosing += (s,e) =>
			{
				((PictureBox)elementos[4]).BackgroundImage.Dispose();
				((Button)elementos[1]).BackgroundImage.Dispose();
				elementos.Clear();
			};
		}

		private static void CriarJanelaBase (Vector2 v = default(Vector2), string nome = "Janela")
		{
			if(v == default(Vector2))
			{
				v = new Vector2(400,300);
			}

			int x = (int) v.X, y = (int) v.Y;
			
			janelaAtual = new Form ()
			{
				Name = nome,
				Text = nome,
				Size = new Size(x,y),
				FormBorderStyle = FormBorderStyle.None,
				StartPosition = FormStartPosition.CenterScreen,
				BackColor = Color.FromArgb(40,40,40),
			};
			janelaAtual.FormClosed += (s,e) =>
			{
				DestruirJanelaBase();
			};

			Color cor_borda = Color.White;

			var elementos = new List<dynamic>()
			{
				new PictureBox ()
				{
					Name = "fechar",
					BackColor = Color.Transparent,
					Size = new Size(16,16),
					Location = new Point(x - 20,6),
					BackgroundImage = Utis.ImagemComCor("dir6/graficos/fechar.png",Color.White),
					Cursor = Cursors.Hand,
					BackgroundImageLayout = ImageLayout.Stretch,
				},
			};

			((PictureBox)elementos[0]).Click += (s,e) => { DestruirJanelaBase(); };
			((PictureBox)elementos[0]).MouseEnter += (s,e) =>
			{
				((PictureBox)elementos[0]).BackColor = Color.Red;
			};
			((PictureBox)elementos[0]).MouseLeave += (s,e) =>
			{	
				((PictureBox)elementos[0]).BackColor = Color.Transparent;
			};

			foreach(var elemento in elementos)
			{
				janelaAtual.Controls.Add(elemento);
			}

			Form1.UsarForma().Enabled = false;
			elementos[0].Parent.Controls.SetChildIndex(elementos[0],-1);

			janelaAtual.Focus();
			janelaAtual.Show();
		}

		private static void DestruirJanelaBase ()
		{
			janelaAtual.Dispose();
			janelaAtual = null;
			Form1.UsarForma().Enabled = true;
		}
	}
}
