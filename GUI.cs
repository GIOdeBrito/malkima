using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class GUI
	{
		private static Button[] _listados = null;

		public static void AtualizarPainel ()
		{
			LimparPainelListados();
			ListarItensPainel(Gerir.UsarCategoria());
		}
		
		public static void ListarItensPainel (int categoria = -1)
		{
			Gerir.DefinirCategoria(categoria);
			LimparPainelListados();
			
			Panel p = Inicio.UsarPainel();
			List<Button> b = new List<Button>();
			JogoItem[] itens = Gerir.UsarItemJogo();

			if(itens is null)
			{
				return;
			}

			JogoItem[] jogos = Array.FindAll(itens, item => item.cate == categoria.ToString());

			if(jogos.Length < 1)
			{
				return;
			}

			int x = 20, y = 20;
			Vector2 tamanho = new Vector2(100, 140);
			int contador = 0;

			for(int i = 0; i < jogos.Length; i++)
			{
				if(contador > 0)
				{
					x += 20 + (int)tamanho.X;
				}
				if(contador > 6)
				{
					x = 20;
					y += 20 + (int)tamanho.Y;
				}
				
				b.Add(new Button()
				{
					Name = jogos[i].id,
					BackgroundImage = Utis.GIOImagem(@$"dir6/jogos/{jogos[i].id}/capa.png"),
					BackgroundImageLayout = ImageLayout.Center,
					ForeColor = Color.Black,
					FlatStyle = FlatStyle.Flat,
					Size = new Size((int)tamanho.X,(int)tamanho.Y),
					Location = new Point(x,y),
					Cursor = Cursors.Hand,
					Enabled = true,
				});

				p.Controls.Add(b[i]);

				contador++;

				Button b_jogo = b[i];

				b_jogo.Click += (s,e) =>
				{
					string nome = b_jogo.Name;
					string exec = Array.Find(itens, item => item.id == nome).id;
					int index = Array.FindIndex(itens, item => item.id == exec);
					//Janela.AdicionarJogo(itens[index]);

					Janela.VerJogo(index);
				};
			}

			_listados = b.ToArray();
		}

		public static void LimparPainelListados ()
		{
			if(_listados == null || _listados.Length <= 0)
			{
				return;
			}
			
			for(int i = 0; i < _listados.Length; i++)
			{
				_listados[i].BackgroundImage.Dispose();
				_listados[i].Dispose();
			}

			foreach(Button b in Inicio.UsarPainel().Controls.OfType<Button>().ToList())
			{
				Inicio.UsarPainel().Controls.Remove(b);
			}

			Array.Clear(_listados, 0, _listados.Length);

			_listados = null;
		}

		public static void CriarBotaoRedondo (dynamic elemento, int radius = 10)
		{
			if(elemento is Button)
			{
				elemento.FlatStyle = FlatStyle.Flat;
				elemento.FlatAppearance.BorderSize = 0;
				elemento.ForeColor = Color.White;
				//elemento.BackColor = Color.White;
				//elemento.FlatAppearance.MouseDownBackColor = Color.Gray;
				//elemento.FlatAppearance.MouseOverBackColor = Color.LightGray;
			}
	
			using(GraphicsPath path = new GraphicsPath())
			{
				path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
				path.AddLine(radius, 0, elemento.Width - radius, 0);
				path.AddArc(elemento.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
				path.AddLine(elemento.Width, radius, elemento.Width, elemento.Height - radius);
				path.AddArc(elemento.Width - radius * 2, elemento.Height - radius * 2, radius * 2, radius * 2, 0, 90);
				path.AddLine(elemento.Width - radius, elemento.Height, radius, elemento.Height);
				path.AddArc(0, elemento.Height - radius * 2, radius * 2, radius * 2, 90, 90);
				path.AddLine(0, elemento.Height - radius, 0, radius);
				elemento.Region = new Region(path);
			}
		}
	}
}
