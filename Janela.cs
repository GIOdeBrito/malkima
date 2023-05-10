using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Data;

namespace Malkima
{
	internal class Janela
	{
		private static Form janelaAtual = null;

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

			Form1.UsarForma().Enabled = false;

			janelaAtual.Focus();
			janelaAtual.Show();
		}

		public static void VerJogo (int jogo_index = -1)
		{
			if(jogo_index < 0)
			{
				return;
			}
			
			JogoItem jogo = Gerir.UsarItemJogo()[jogo_index];
			CriarJanelaBase(new Vector2(300,200), $"Jogo :: {jogo.nome}");
			MakeCloseButton();

			var elementos = new List<dynamic>
			{
				new PictureBox()
				{
					BackgroundImage = Utis.GIOImagem($@"dir6/jogos/{jogo.id}/capa.png"),
					BackgroundImageLayout = ImageLayout.Center,
					Location = new Point(0,20),
					Size = new Size(90,90),
				},
				/*new Label()
				{
					Text = jogo.nome,
					Location = new Point(90,10),
					Size = new Size(600,100),
					TextAlign = ContentAlignment.MiddleLeft,
					ForeColor = Color.FromArgb(200,255,255,255),
					Font = Fontes.AplicarFontes(22),
				},*/
				new Button()
				{
					Text = "Jogar",
					Location = new Point(90,40),
					Size = new Size(140,55),
					TextAlign = ContentAlignment.MiddleCenter,
					ForeColor = Color.White,
					BackColor = Color.FromArgb(200,204,18,18),
					Font = Fontes.AplicarFontes(16),
					Cursor = Cursors.Hand,
				},
				new Button()
				{
					Text = "Editar",
					Location = new Point(janelaAtual.Width - 120,220),
					Size = new Size(100,30),
					TextAlign = ContentAlignment.MiddleCenter,
					ForeColor = Color.White,
					BackColor = Color.FromArgb(200,50,50,50),
					Font = Fontes.AplicarFontes(11),
					Cursor = Cursors.Hand,
				},
				new Button()
				{
					Text = "Remover",
					Location = new Point(20,220),
					Size = new Size(100,30),
					TextAlign = ContentAlignment.MiddleCenter,
					ForeColor = Color.White,
					BackColor = Color.FromArgb(200,50,50,50),
					Font = Fontes.AplicarFontes(11),
					Cursor = Cursors.Hand,
				},
			};

			((Button)elementos[1]).Click += (s,e) =>
			{
				DestruirJanelaBase();
				Gerir.IniciarAplicacao(jogo.exec,jogo.inic);				
			};
			((Button)elementos[2]).Click += (s,e) =>
			{
				DestruirJanelaBase();
				AdicionarJogo(jogo);
			};
			((Button)elementos[3]).Click += (s,e) =>
			{
				DestruirJanelaBase();
				Gerir.RemoverItemJogo(jogo.id);
			};

			foreach(Button botao in elementos.OfType<Button>())
			{
				GUI.CriarBotaoRedondo(botao,6);
			}

			foreach(var elem in elementos)
			{
				janelaAtual.Controls.Add(elem);
			}
		}

		public static void AdicionarJogo (JogoItem item = null)
		{
			string c = string.Empty;
			string id = string.Empty;
			string arq_nome = string.Empty;
			string parametros = string.Empty;
			int categoria = 0;
			Image ico = null;

			if(item != null)
			{
				id = item.id;
				arq_nome = item.nome;
				parametros = item.inic;
				categoria = Int16.Parse(item.cate);
				ico = Utis.GIOImagem($@"dir6/jogos/{item.id}/capa.png");
			}
			else
			{
				c = Utis.ColetarArquivo();

				if(string.IsNullOrEmpty(c))
				{
					return;
				}

				arq_nome = Path.GetFileNameWithoutExtension(c);
				Icon icono = Icon.ExtractAssociatedIcon(c);
				ico = icono.ToBitmap();
				icono.Dispose();
			}
			
			CriarJanelaBase(v: new Vector2(290,400),nome: "Adicionar jogo");
			MakeCloseButton();

			int x = janelaAtual.Width, y = janelaAtual.Height;
			Func<string[],List<string>> criarTabela = (str) =>
			{
				List<string> lista = new List<string>();

				foreach(string s in str)
				{
					lista.Add(s);
				}

				return lista;
			};

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
					BackgroundImage = ico,
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
					Text = parametros,
					Location = new Point(25,y/2 + 90),
					Size = new Size(x - 50,30),
					ForeColor = Color.White,
					BackColor = Color.FromArgb(40,40,40),
					Font = Fontes.AplicarFontes(),
					TextAlign = HorizontalAlignment.Left,
				},
				new ComboBox()
				{
					Name = "categoria",
					Size = new Size(x - 50,30),
					Location = new Point(25, 90),
					DataSource = criarTabela(Inicio.tipos),
					FlatStyle = FlatStyle.Flat,
					DropDownStyle = ComboBoxStyle.DropDownList,
					ForeColor = Color.White,
					BackColor = Color.FromArgb(40,40,40),
					Font = Fontes.AplicarFontes(),
				},
			};

			foreach(var elemento in elementos)
			{
				janelaAtual.Controls.Add(elemento);
			}

			((Button)elementos[0]).Click += (s,e) =>
			{
				string id_atual = id;

				Utis.CriarDiretorio(@"dir6/jogos");

				if(string.IsNullOrEmpty(id))
				{
					id_atual = arq_nome.Replace(" ","");
				}

				string dir = $@"dir6/jogos/{id_atual}";

				Utis.CriarDiretorio(dir);
				Utis.RemoverArquivo($"{dir}/capa.png");

				((Button)elementos[1]).BackgroundImage.Save($"{dir}/capa.png");

				arq_nome = ((TextBox)elementos[2]).Text;
				parametros = ((TextBox)elementos[3]).Text;
				categoria = ((ComboBox)elementos[4]).SelectedIndex;

				JogoItem neo_item = new JogoItem()
				{
					id = id_atual,
					nome = arq_nome,
					exec = c,
					inic = parametros,
					cate = ((ComboBox)elementos[4]).SelectedIndex.ToString(),
				};

				if(item is null)
				{
					Gerir.NovoItemJogo(neo_item);
				}
				else
				{
					JogoItem[] its = Gerir.UsarItemJogo();
					int i = Array.FindIndex(its, item => item.id == id_atual);
					its[i] = neo_item;

					Gerir.DefinirLista(new XMLJogo()
					{
						itens = its.ToList(),
					});
				}

				DestruirJanelaBase();
				GUI.AtualizarPainel();
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

				b.BackgroundImage = Utis.GIOImagem(capa);
			};

			janelaAtual.FormClosing += (s,e) =>
			{
				((PictureBox)elementos[4]).BackgroundImage.Dispose();
				((Button)elementos[1]).BackgroundImage.Dispose();
				elementos.Clear();
			};
		}

		private static void MakeCloseButton ()
		{
			PictureBox close = new PictureBox ()
			{
				Name = "fechar",
				BackColor = Color.Transparent,
				Size = new Size(16,16),
				Location = new Point(janelaAtual.Width - 20,6),
				BackgroundImage = Utis.ImagemComCor("dir6/graficos/sair.png",Color.White),
				Cursor = Cursors.Hand,
				BackgroundImageLayout = ImageLayout.Stretch,
			};

			close.Click += (s,e) => { DestruirJanelaBase(); };
			close.MouseEnter += (s,e) =>
			{
				close.BackColor = Color.Red;
			};
			close.MouseLeave += (s,e) =>
			{	
				close.BackColor = Color.Transparent;
			};

			janelaAtual.Controls.Add(close);
			close.Parent.Controls.SetChildIndex(close,-1);
		}

		private static void DestruirJanelaBase ()
		{
			janelaAtual.Dispose();
			janelaAtual = null;
			Form1.UsarForma().Enabled = true;
		}
	}
}
