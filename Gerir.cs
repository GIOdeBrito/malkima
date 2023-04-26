using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Malkima
{
	internal class Gerir
	{
		private static JogoItem[] _jogos;
		private static Button[] _listados;
		private static int _categoria_atual = -1;

		public static void NovoItemJogo (JogoItem item)
		{
			List<JogoItem> l;

			try
			{
				l = _jogos.ToList();
			}
			catch
			{
				l = new List<JogoItem>();
			}
			
			l.Add(item);
			_jogos = l.ToArray();

			l.Clear();

			DadosXML.GravarXML(@"dir6/xml/jogos.xml", new XMLJogo()
			{
				itens = _jogos.ToList(),
			});
		}

		public static JogoItem[] UsarItemJogo ()
		{
			return _jogos;
		}

		public static void DefinirLista (XMLJogo dados)
		{
			_jogos = dados.itens.ToArray();
		}

		public static void RemoverItemJogo (string id = "?id?")
		{
			int index = Array.FindIndex(_jogos, item => item.id == id);

			if(index < 0)
			{
				return;
			}

			List<JogoItem> l = _jogos.ToList();
			l.RemoveAt(index);

			_jogos = l.ToArray();
			l.Clear();
		}
		
		public static void ListarItensPainel (int categoria = -1)
		{
			if(categoria == _categoria_atual)
			{
				return;
			}
			
			Panel p = Inicio.UsarPainel();
			List<Button> b = new List<Button>();
			JogoItem[] jogos = Array.FindAll(_jogos, item => item.cate == categoria.ToString());

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
					BackgroundImage = Image.FromFile(@$"dir6/jogos/{jogos[i].id}/capa.png"),
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
					string exec = Array.Find(_jogos, item => item.id == nome).exec;

				};
			}

			_categoria_atual = categoria;
		}

		public static void IniciarAplicacao (string exec, string param = null)
		{
			if(param != null)
			{
				ProcessStartInfo startInfo = new ProcessStartInfo()
				{
					FileName = "cmd.exe",
					Arguments = @"/c " + param,
				};
				Process p = new Process() { StartInfo = startInfo, };
				
				p.Start();
				
				return;
			}
			
			Process.Start(exec);
		}
	}
}

[XmlRoot("G_Jogo")]
public class XMLJogo
{
	[XmlElement("JogoItem")]
	public List<JogoItem> itens;
}

public class JogoItem
{
	public string id { get; set; }
	public string nome { get; set; }
	public string exec { get; set; }
	public string inic { get; set; }
	public string cate { get; set; }
}