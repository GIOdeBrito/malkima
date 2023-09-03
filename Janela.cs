using System;
using System.Numerics;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace Malkima
{
	internal class Janela
	{
		public static void CreateTestWindow (Vector2 v, string nome)
		{
			// Padrão X 400, Y 300
			GWindow win = new GWindow(v, nome);

			int vX = (int)v.X;
			int vY = (int)v.Y;

			Button b = win.CreateButton("Fechar",new Point(vX/2 - 180/2, vY/2 - 30/2),new Size(180,30));

			b.Click += (s, e) => win.DestroyWindow();
		}

		public static void WindowDetails ()
		{
			int x = 300;
			int y = 300;
			
			GWindow win = new GWindow(new Vector2(x,y), "Detalhes");
			Label title = win.CreateLabel("Malkima",new Point(x/2 - 100,40),new Size(200,50));

			title.Font.Dispose();
			title.Font = Fontes.AplicarFontes(32);

			win.CreatePicture(@"dir6/gb.png",new Point(x/2 - 25, y/2 - 25),new Size(50,50));
			
			string text =	$@"Desenvolvido por Giordano de Brito
							Versão da aplicação {Inicio.version}
							{Inicio.date}";

			Label description = null;
			description = win.CreateLabel(text,new Point(x/2 - 150, y/2 + 40), new Size(300,60));
			
			description.Font.Dispose();
			description.Font = Fontes.AplicarFontes(10);
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
				ico = Utis.LoadImage(GUI.RetornarCapa(item.id));
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

			GWindow win = new GWindow(new Vector2(290,380), "Adicioanr jogo");

			int x = (int) win.GetVector().X;
			int y = (int) win.GetVector().Y;
			
			Func<string[],List<string>> criarTabela = (str) =>
			{
				List<string> lista = new List<string>();

				foreach(string s in str)
				{
					lista.Add(s);
				}

				return lista;
			};

			/* Creating window elements */

			Button b_save = null;
			b_save = win.CreateButton("Salvar", new Point(x/2 - 50, y - 40), new Size(100,30));

			Button cover = null;
			cover = win.CreateButton(string.Empty,new Point(x/2 - 50, y/2 - 50),new Size(100,120));
			cover.BackgroundImage = ico;
			cover.BackgroundImageLayout = ImageLayout.Stretch;
			
			TextBox name = null;
			name = win.CreateTextBox(arq_nome,new Point(25,45),new Size(x - 50,30));
			name.TextAlign = HorizontalAlignment.Center;

			TextBox _params = null;
			_params = win.CreateTextBox(parametros,new Point(25,y/2 + 90),new Size(x - 50,30));

			ComboBox category = null;
			category = win.CreateDropdown(string.Empty,new Point(25, 90),new Size(x - 50,30));
			category.DataSource = criarTabela(Menu.tipos);

			// Define a categoria selecionada na caixa
			category.SelectedIndex = categoria;

			// Botão de salvar; Guarda a imagem e os dados no disco
			b_save.Click += (s,e) =>
			{
				string id_atual = id;
				ImageFormat formato = cover.BackgroundImage.RawFormat;

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
					Bitmap bmp = Utis.ConverterParaPng((Bitmap) cover.BackgroundImage);
					bmp = Utis.Redimensionar(bmp, new Vector2(256,512));
					cover.BackgroundImage.Dispose();
					cover.BackgroundImage = bmp;
				}

				cover.BackgroundImage.Save($"{dir}/capa_nova.png", ImageFormat.Png);

				arq_nome = name.Text;
				parametros = _params.Text;
				categoria = category.SelectedIndex;

				JogoItem neo_item = new JogoItem()
				{
					id = id_atual,
					nome = arq_nome,
					exec = exec,
					inic = parametros,
					categoria = category.SelectedIndex.ToString(),
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

					Gerir.DefinirLista(new XMLJogo() { itens = its.ToList() });
				}

				win.DestroyWindow();
				GUI.AtualizarPainel(true);
			};

			cover.Click += (s,e) =>
			{
				string capa = Utis.ColetarArquivo(1);

				if(string.IsNullOrEmpty(capa))
				{
					return;
				}

				if(cover.BackgroundImage != null)
				{
					cover.BackgroundImage.Dispose();
					cover.BackgroundImage = null;
				}

				cover.BackgroundImage = Utis.LoadImage(capa);
			};
		}
	}

	public class GWindow
	{
        private Form _window;
		private Vector2 _vector;
		
		public GWindow (Vector2 v = default(Vector2), string name = "Window")
        {
			_vector = v;
			_window = new Form ()
			{
				Name = name.ToLower(),
				Text = name,
				Size = new Size((int)v.X,(int)v.Y),
				FormBorderStyle = FormBorderStyle.None,
				StartPosition = FormStartPosition.CenterScreen,
				BackColor = Color.FromArgb(40,40,40),
			};

			DefineWindow();
			AddCloseButton();
        }

		public override string ToString()
		{
			string s = "GIO";
			return s;
		}

		public Vector2 GetVector ()
		{
			return _vector;
		}

		public Form GetForm ()
		{
			return _window;
		}

		private void DefineWindow ()
		{
			Form1.UsarForma().Enabled = false;
			_window.FormClosed += (s,e) => DestroyWindow();
			_window.Focus();
			_window.Show();
		}

		public void AddCloseButton ()
		{
			PictureBox close = new PictureBox ()
			{
				Name = "b_fechar",
				BackColor = Color.Transparent,
				Size = new Size(16,16),
				Location = new Point((int)_vector.X - 20,6),
				BackgroundImage = Utis.ColouredImage("dir6/graficos/sair.png",Cor.UsarCor(0)),
				Cursor = Cursors.Hand,
				BackgroundImageLayout = ImageLayout.Stretch,
			};

			close.Click += (s,e) => DestroyWindow();
			close.MouseEnter += (s,e) => close.BackColor = Cor.UsarCor(2);
			close.MouseLeave += (s,e) => close.BackColor = Color.Transparent;

			_window.Controls.Add(close);
			close.Parent.Controls.SetChildIndex(close, 10);
		}

		/* Create Elements */

		public Label CreateLabel (string text, Point location, Size sz)
		{
			Label label = new Label ()
			{
				Text = text,
				Location = location,
				Size = sz,
				Font = Fontes.AplicarFontes(),
				ForeColor = Cor.UsarCor(0),
				TextAlign = ContentAlignment.MiddleCenter,
			};

			_window.Controls.Add(label);

			return label;
		}

		public Button CreateButton (string name, Point location, Size sz)
		{
			Button b = new Button()
			{
				Name = "b_" + name.ToLower(),
				Text = name,
				Size = sz,
				Location = location,
				ForeColor = Cor.UsarCor(0),
				FlatStyle = FlatStyle.Flat,
				Font = Fontes.AplicarFontes(),
				Cursor = Cursors.Hand,
			};

			_window.Controls.Add(b);

			return b;
		}

		public PictureBox CreatePicture (string path, Point location, Size sz)
		{
			PictureBox pb = new PictureBox()
			{
				Location = location,
				Size = sz,
				BackgroundImage = Utis.LoadImage(path),
				BackgroundImageLayout = ImageLayout.Stretch,
			};

			_window.Controls.Add(pb);

			return pb;
		}

		public TextBox CreateTextBox (string name, Point location, Size sz)
		{
			TextBox tb = new TextBox()
			{
				Name = "tb_"+name.ToLower(),
				Text = name,
				Location = location,
				Size = sz,
				ForeColor = Cor.UsarCor(0),
				BackColor = Color.FromArgb(40,40,40),
				Font = Fontes.AplicarFontes(),
				TextAlign = HorizontalAlignment.Left,
			};

			_window.Controls.Add(tb);

			return tb;
		}

		public ComboBox CreateDropdown (string name, Point location, Size sz)
		{
			ComboBox cb = new ComboBox()
			{
				Name = name,
				Size = sz,
				Location = location,
				FlatStyle = FlatStyle.Flat,
				DropDownStyle = ComboBoxStyle.DropDownList,
				ForeColor = Cor.UsarCor(0),
				BackColor = Color.FromArgb(40,40,40),
				Font = Fontes.AplicarFontes(),
			};
			
			_window.Controls.Add(cb);

			return cb;
		}

		/* Ultima destruction */

		public void DestroyWindow ()
		{
			ClearWindow();
			
			try
			{
				_window.Dispose();
				_window = null;
			}
			catch
			{
				Console.WriteLine("Não pôde descartar a Janela");
			}
			finally
			{
				Form1.UsarForma().Enabled = true;
				Form1.UsarForma().Focus();
			}
		}

		private void ClearWindow ()
		{
			GUI.DescartarElemento(_window);
		}
	}
}
