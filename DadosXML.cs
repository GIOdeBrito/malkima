using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Malkima
{
	internal class DadosXML
	{	
		public static dynamic LerXML (string c, Type tipo)
		{
			dynamic dados = null;
			
			try {

				XmlSerializer xml = new XmlSerializer(tipo);
				
				using(StreamReader leitor = new StreamReader(c))
				{
					dados = xml.Deserialize(leitor);
				}
			}
			catch {

				Console.WriteLine($"Erro ao ler: {c}");
			}

			return dados;
		}
	}
}


