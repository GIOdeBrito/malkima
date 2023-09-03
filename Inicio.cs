using System;

namespace Malkima
{
	internal class Inicio
	{	
		public static string version = "1.0";
		public static string date = "19/09/2001";
		
		static Inicio ()
		{
			Console.WriteLine("Inicializando programa...");
			Utis.CriarDiretorio(@"dir6/jogos");
			LoadGames();
			GetVersion();
		}

		public static void Initialize ()
		{
			Menu.StartMenu();
		}

		public static void LoadGames ()
		{
			XMLJogo dados = (XMLJogo) DadosXML.LerXML(@"dir6/xml/jogos.xml",typeof(XMLJogo));

			if(dados == null)
			{
				return;
			}

			foreach(JogoItem item in dados.itens)
			{
				Console.WriteLine(item.nome);
			}

			Gerir.DefinirLista(dados);
			dados.itens.Clear();
		}

		public static void GetVersion ()
		{
			XMLVersion ver = DadosXML.LerXML(@"dir6/xml/version.xml",typeof(XMLVersion));
			version = ver.version;
			date = ver.date;
		}
	}
}
