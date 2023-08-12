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
using System.Drawing.Imaging;

namespace Malkima
{
	internal class Janela
	{
		private static Form _janelaAtual = null;

		private static void CriarJanelaBase (Vector2 v = default(Vector2), string nome = "Janela")
		{
			if(v == default(Vector2))
			{
				v = new Vector2(400,300);
			}

			int x = (int) v.X, y = (int) v.Y;
			
			_janelaAtual = new Form ()
			{
				Name = nome,
				Text = nome,
				Size = new Size(x,y),
				FormBorderStyle = FormBorderStyle.None,
				StartPosition = FormStartPosition.CenterScreen,
				BackColor = Color.FromArgb(40,40,40),
			};
			_janelaAtual.FormClosed += (s,e) =>
			{
				DestruirJanelaBase();
			};

			Form1.UsarForma().Enabled = false;

			_janelaAtual.Focus();
			_janelaAtual.Show();
		}

		public static void AdicionarJogo (JogoItem item = null)
		{
			string exec = string.Empty;
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
				exec = item.exec;
				categoria = Int16.Parse(item.categoria);
				ico = Utis.GIOImagem(GUI.RetornarCapa(item.id));
			}
			else
			{
				exec = Utis.ColetarArquivo();

				if(string.IsNullOrEmpty(exec))
				{
					return;
				}

				arq_nome = Path.GetFileNameWithoutExtension(exec);
				Icon icono = Icon.ExtractAssociatedIcon(exec);
				ico = icono.ToBitmap();
				icono.Dispose();
			}
			
			CriarJanelaBase(v: new Vector2(290,400),nome: "Adicionar jogo");
			MakeCloseButton();

			int x = _janelaAtual.Width, y = _janelaAtual.Height;
			
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
					ForeColor = Cor.UsarCor(0),
					FlatStyle = FlatStyle.Flat,
					Font = Fontes.AplicarFontes(),
					Cursor = Cursors.Hand,
				},
				new Button()
				{
					Name = "capa",
					Size = new Size(100,120),
					Location = new Point(x/2 - 50, y/2 - 50),
					ForeColor = Cor.UsarCor(0),
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
					ForeColor = Cor.UsarCor(0),
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
					ForeColor = Cor.UsarCor(0),
					BackColor = Color.FromArgb(40,40,40),
					Font = Fontes.AplicarFontes(),
					TextAlign = HorizontalAlignment.Left,
				},
				new ComboBox()
				{
					Name = "categoria",
					Size = new Size(x - 50,30),
					Location = new Point(25, 90),
					DataSource = criarTabela(Menu.tipos),
					FlatStyle = FlatStyle.Flat,
					DropDownStyle = ComboBoxStyle.DropDownList,
					ForeColor = Cor.UsarCor(0),
					BackColor = Color.FromArgb(40,40,40),
					Font = Fontes.AplicarFontes(),
				},
			};

			foreach(var elemento in elementos)
			{
				_janelaAtual.Controls.Add(elemento);
			}

			// Define a categoria selecionada na caixa
			((ComboBox)elementos[4]).SelectedIndex = categoria;

			// Botão de salvar; Guarda a imagem e os dados no disco
			((Button)elementos[0]).Click += (s,e) =>
			{
				string id_atual = id;
				Button b_capa = (Button)elementos[1];
				ImageFormat formato = b_capa.BackgroundImage.RawFormat;

				if(string.IsNullOrEmpty(id))
				{
					id_atual = arq_nome.Replace(" ","");
				}

				string dir = $@"dir6/jogos/{id_atual}";

				Utis.CriarDiretorio(dir);
				Utis.RemoverArquivo($"{dir}/capa_nova.png");

				/*
				Se tentar salvar um formato não png como png uma
				excessão será lançada. A imagem precisará ser
				convertida antes
				*/
				if(formato != ImageFormat.Png)
				{	
					Bitmap bmp = Utis.ConverterParaPng((Bitmap)b_capa.BackgroundImage);
					bmp = Utis.Redimensionar(bmp, new Vector2(256,512));
					b_capa.BackgroundImage.Dispose();
					b_capa.BackgroundImage = bmp;
				}

				b_capa.BackgroundImage.Save($"{dir}/capa_nova.png", ImageFormat.Png);

				arq_nome = ((TextBox)elementos[2]).Text;
				parametros = ((TextBox)elementos[3]).Text;
				categoria = ((ComboBox)elementos[4]).SelectedIndex;

				JogoItem neo_item = new JogoItem()
				{
					id = id_atual,
					nome = arq_nome,
					exec = exec,
					inic = parametros,
					categoria = ((ComboBox)elementos[4]).SelectedIndex.ToString(),
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
				GUI.AtualizarPainel(true);
			};

			((Button)elementos[1]).Click += (s,e) =>
			{
				string capa = Utis.ColetarArquivo(1);
				Button b_capa = (Button)elementos[1];

				if(string.IsNullOrEmpty(capa))
				{
					return;
				}

				if(b_capa.BackgroundImage != null)
				{
					b_capa.BackgroundImage.Dispose();
					b_capa.BackgroundImage = null;
				}

				b_capa.BackgroundImage = Utis.GIOImagem(capa);
			};

			_janelaAtual.FormClosing += (s,e) =>
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
				Location = new Point(_janelaAtual.Width - 20,6),
				BackgroundImage = Utis.ImagemComCor("dir6/graficos/sair.png",Cor.UsarCor(0)),
				Cursor = Cursors.Hand,
				BackgroundImageLayout = ImageLayout.Stretch,
			};

			close.Click += (s,e) => { DestruirJanelaBase(); };
			close.MouseEnter += (s,e) =>
			{
				close.BackColor = Cor.UsarCor(2);
			};
			close.MouseLeave += (s,e) =>
			{	
				close.BackColor = Color.Transparent;
			};

			_janelaAtual.Controls.Add(close);
			close.Parent.Controls.SetChildIndex(close, 10);
		}

		private static void DestruirJanelaBase ()
		{
			_janelaAtual.Dispose();
			_janelaAtual = null;

			Form1.UsarForma().Enabled = true;
			Form1.UsarForma().Focus();
		}
	}
}
