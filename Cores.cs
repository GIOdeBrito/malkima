using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Malkima
{
	internal class Cores
	{
		private const string pasta = "dir6/xml";
		private static Color[] cores;
		private static Tema tema;

		static Cores ()
		{
			//LerCores();
			//LerTemas();
		}

		public static void LerCores ()
		{ 
			XMLCor dados = (XMLCor) DadosXML.LerXML($"{pasta}/cores.xml",typeof(XMLCor));

			List<Color> lcores = new List<Color>();

			foreach(Cor cor in dados.itens)
			{
				lcores.Add(Color.FromArgb(cor.R,cor.G,cor.B));
			}

			cores = lcores.ToArray();
			lcores.Clear();
		}

		public static void LerTemas (int atual = 1)
		{ 
			XMLTema dados = (XMLTema) DadosXML.LerXML($"{pasta}/temas.xml",typeof(XMLTema)); 
			tema = dados.itens[atual];
		}

		public static Color UsarCor (int i = -1)
		{
			if(i < 0)
			{
				i = 0;
			}

			return cores[i];
		}

		public static Tema UsarTema ()
		{
			return tema;
		}
	}
}

[XmlRoot("G_Cor")]
public class XMLCor
{
	[XmlElement("Cor")]
	public List<Cor> itens;
}

public class Cor
{
	public int R { get; set; }
	public int G { get; set; }
	public int B { get; set; }
}

[XmlRoot("G_Tema")]
public class XMLTema
{
	[XmlElement("Tema")]
	public List<Tema> itens;
}

public class Tema
{
	public string nome { get; set; }
	public int icone { get; set; }
	public int borda { get; set; }
	public int texto { get; set; }
	public int particula { get; set; }
}
