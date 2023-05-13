using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class GUI
	{
		private static Button[] _listados = null;

		public static void AtualizarPainel (bool forcabruta = false)
		{
			LimparPainelListados();
			ListarItensPainel(Gerir.UsarCategoria(), forcabruta);
		}
		
		public static void ListarItensPainel (int categoria = -1, bool forcabruta = false)
		{
			Panel painel = Inicio.UsarPainel();
			
			if(categoria == Gerir.UsarCategoria() && !forcabruta)
			{
				painel.Visible = !painel.Visible;
				return;
			}
			
			Gerir.DefinirCategoria(categoria);
			LimparPainelListados();
			
			List<Button> botoes = new List<Button>();
			JogoItem[] itens = Gerir.UsarItemJogo();

			if(itens is null)
			{
				return;
			}

			JogoItem[] jogos = Array.FindAll(itens, item => item.categoria == categoria.ToString());

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
				
				botoes.Add(new Button()
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

				painel.Controls.Add(botoes[i]);

				contador++;

				Button b_jogo = botoes[i];
				Panel b_editar = new Panel()
				{
					Name = "editar",
					Location = new Point((int)tamanho.X-16,1),
					Size = new Size(15,15),
					//BackColor = Color.Transparent,
					BackgroundImage = Utis.ImagemComCor(@"dir6/graficos/editar.png", Color.White),
					BackgroundImageLayout = ImageLayout.Stretch,
				};
				Panel b_excluir = new Panel()
				{
					Name = "deletar",
					Location = new Point((int)tamanho.X-16,20),
					Size = new Size(15,15),
					//BackColor = Color.Transparent,
					BackgroundImage = Utis.ImagemComCor(@"dir6/graficos/deletar.png", Color.Red),
					BackgroundImageLayout = ImageLayout.Stretch,
				};

				b_jogo.Controls.Add(b_editar);
				b_jogo.Controls.Add(b_excluir);

				string nome = b_jogo.Name;
				string exec = jogos[i].exec;
				string id = jogos[i].id;
				string param = jogos[i].inic;
				int index = Array.FindIndex(itens, item => item.id == id);

				b_jogo.Click += (s,e) =>
				{
					Console.WriteLine(param);
					Console.WriteLine(exec);

					Gerir.IniciarAplicacao(exec, param);

					/*if(((MouseEventArgs)e).Button == MouseButtons.Left)
					{
						Point local = ((MouseEventArgs)e).Location;
						if(VerificarEditarJogo(local))
						{
							Janela.AdicionarJogo(itens[index]);
							return;
						}
					}*/
				};
				b_editar.Click += (s,e) =>
				{
					Janela.AdicionarJogo(itens[index]);
				};
				b_excluir.Click += (s,e) =>
				{
					Gerir.RemoverItemJogo(id);
				};
			}

			painel.Visible = true;
			_listados = botoes.ToArray();
			botoes.Clear();
		}

		public static void LimparPainelListados ()
		{
			if(_listados == null || _listados.Length <= 0)
			{
				return;
			}
			
			for(int i = 0; i < _listados.Length; i++)
			{
				DescartarElemento(_listados[i]);
			}

			foreach(Button b in Inicio.UsarPainel().Controls.OfType<Button>().ToList())
			{
				Inicio.UsarPainel().Controls.Remove(b);
			}

			Array.Clear(_listados, 0, _listados.Length);

			_listados = null;
		}

		public static void DescartarElemento (Control elem)
		{
			foreach(Control filho in elem.Controls)
			{
				if(filho.BackgroundImage != null)
				{
					filho.BackgroundImage.Dispose();
				}
				
				filho.Dispose();
			}

			if(elem.BackgroundImage != null)
			{
				elem.BackgroundImage.Dispose();
			}

			elem.Dispose();
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
