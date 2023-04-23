using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Malkima
{
	internal class Gerir
	{
		public static void AdicionarItem ()
		{
			Panel p = Inicio.UsarPainel();
			List<Button> b = new List<Button>();

			Action<Button,Color> realcar = (botao, cor) =>
			{	
				botao.ForeColor = cor;
			};
			
			const int c_x = 92;
			const int c_y = 140;
			int x = 0;
			int y = 0;
			int cont = 0;

			for(int i = 0; i < 45; i++)
			{
				if(cont > 13)
				{
					x = 0;
					y += c_y;
					cont = 0;
				}
				
				b.Add(new Button() {

					Text = "Jogo AAAAAAAAAAAAAAAAAAAAAAAAAAA",
					Location = new Point(x, y),
					Size = new Size(c_x, c_y),
					FlatStyle = FlatStyle.Flat,
					ForeColor = Color.White,
					TextAlign = ContentAlignment.BottomCenter,
				});

				p.Controls.Add(b[i]);

				cont++;
				x += c_x;
			}

			foreach(Button b1 in b)
			{
				b1.MouseEnter += (s,e) => { realcar(b1,Color.Blue); };
				b1.MouseLeave += (s,e) => { realcar(b1,Color.White); };
			}

			b.Clear();
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
}