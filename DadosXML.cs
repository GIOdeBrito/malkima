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
			
			try
			{
				XmlSerializer xml = new XmlSerializer(tipo);
				
				using(StreamReader leitor = new StreamReader(c))
				{
					dados = xml.Deserialize(leitor);
				}
			}
			catch(IOException ex)
			{
				Console.WriteLine($"Erro ao ler arquivo: {c}\n{ex.Message}");
			}
			catch(InvalidOperationException ex)
			{
				Console.WriteLine($"Erro ao deserializar arquivo: {c}\n{ex.Message}");
			}

			return dados;
		}

		public static void GravarXML (string c, object obj)
		{	
			try
			{
				XmlSerializer xml = new XmlSerializer(obj.GetType());
				var namespaces = new XmlSerializerNamespaces();
				namespaces.Add("GIO","Malkima");

				using(TextWriter sw = new StreamWriter(c))
				{
					xml.Serialize(sw, obj, namespaces);
				}
			}
			catch(IOException ex)
			{
				Console.WriteLine($"Erro ao escrever arquivo: {c}\n{ex.Message}");
			}
			catch(InvalidOperationException ex)
			{
				Console.WriteLine($"Erro ao escrever a classe XML: {c}\n{ex.Message}");
			}
		}
	}
}


