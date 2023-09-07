using System;
using System.Xml.Serialization;

namespace Malkima
{
	internal class Gerir
	{
		private static JogoItem[] _jogos = null;
		private static int _categoria_atual = -1;

		public static void GravarXMLJogos ()
		{
			DadosXML.GravarXML(@"dir6/xml/jogos.xml", new XMLJogo()
			{
				itens = _jogos.ToList(),
			});
		}

		public static JogoItem GetGameItem (string id = "id?")
		{
			return Array.Find(_jogos, item => item.id == id) ?? null;
		}

		public static JogoItem[] UsarItemJogo ()
		{
			return _jogos;
		}

		public static int UsarCategoria ()
		{
			return _categoria_atual;
		}

		public static void DefinirLista (XMLJogo dados)
		{
			_jogos = dados.itens.ToArray();
			GravarXMLJogos();
		}

		public static void DefinirCategoria (int num = -1)
		{
			if(num < 0)
			{
				return;
			}

			_categoria_atual = num;
		}

		public static void NovoItemJogo (JogoItem item)
		{
			List<JogoItem> lista = new List<JogoItem>();

			if(!(_jogos is null))
			{
				lista = _jogos.ToList();
			}
			
			lista.Add(item);
			_jogos = lista.ToArray();
			lista.Clear();

			GravarXMLJogos();
			GUI.AtualizarPainel(true);
		}

		public static void RemoverItemJogo (string id = "?id?")
		{
			int index = Array.FindIndex(_jogos, item => item.id == id);

			if(index < 0)
			{
				return;
			}

			List<JogoItem> lista = _jogos.ToList();
			lista.RemoveAt(index);

			_jogos = lista.ToArray();
			lista.Clear();

			Utis.RemoverDiretorio(@$"dir6/jogos/{id}", true);
			GravarXMLJogos();
			GUI.AtualizarPainel(true);
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
	public string categoria { get; set; }
}

[XmlRoot("G_Version")]
public class XMLVersion
{
	[XmlElement("ProgramName")]
	public string name;
	[XmlElement("Version")]
	public string version;
	[XmlElement("Date")]
	public string date;
}


