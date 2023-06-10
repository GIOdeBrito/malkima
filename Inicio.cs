using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Inicio
	{	
		static Inicio ()
		{
			Utis.CriarDiretorio(@"dir6/jogos");
		}

		public static void CarregarJogos ()
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
	}
}
