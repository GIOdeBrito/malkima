using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Utis
	{
		public static Tuple<int,int> TelaXY ()
		{
			int x = Form1.UsarForma().Width;
			int y = Form1.UsarForma().Height;

			return Tuple.Create(x, y);
		}

		public static int InteiroAleatorio (int minimo, int maximo)
		{
			int num = new Random().Next(minimo, maximo);
			return num;
		}

		public static Image ImagemComCor (string c, Color cor)
		{
			if(!File.Exists(c))
			{
				throw new FileNotFoundException($"Arquivo {c} não pôde ser encontrado");
			}

			var bmp = new Bitmap(c);

			using(Graphics g = Graphics.FromImage(bmp))
			{
				ColorMap[] colorMap = new ColorMap[1];
				colorMap[0] = new ColorMap();
				colorMap[0].OldColor = Color.White;
				colorMap[0].NewColor = cor;
				
				ImageAttributes attr = new ImageAttributes();
				attr.SetRemapTable(colorMap);
				
				Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
				g.DrawImage(bmp, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
			}
			
			return bmp;
		}

		public static void CriarBotaoRedondo (dynamic elemento, int radius = 10)
		{
			if(elemento is Button)
			{
				elemento.FlatStyle = FlatStyle.Flat;
				elemento.FlatAppearance.BorderSize = 0;
				elemento.ForeColor = Color.White;
				//elemento.BackColor = Color.White;
				//elemento.FlatAppearance.MouseDownBackColor = Color.Gray;
				//elemento.FlatAppearance.MouseOverBackColor = Color.LightGray;
			}
	
			using(GraphicsPath path = new GraphicsPath())
			{
				path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
				path.AddLine(radius, 0, elemento.Width - radius, 0);
				path.AddArc(elemento.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
				path.AddLine(elemento.Width, radius, elemento.Width, elemento.Height - radius);
				path.AddArc(elemento.Width - radius * 2, elemento.Height - radius * 2, radius * 2, radius * 2, 0, 90);
				path.AddLine(elemento.Width - radius, elemento.Height, radius, elemento.Height);
				path.AddArc(0, elemento.Height - radius * 2, radius * 2, radius * 2, 90, 90);
				path.AddLine(0, elemento.Height - radius, 0, radius);
	
				elemento.Region = new Region(path);
			}
		}

		public static string ColetarArquivo (int filtro_tipo = -1)
		{
			string c = string.Empty;

			Func<int,string> definir_filtro = (tipo) =>
			{
				string filtro = string.Empty;
				
				switch(tipo)
				{
					case 1: { filtro = "Capa (*.jpg);(*.png)|*.jpg;*.png"; } break;
					default: { filtro = "Aplicação (*.exe)|*.exe"; } break;
				}
				
				return $"{filtro}|Todos (*.*)|*.*";
			};

			string filtro = definir_filtro(filtro_tipo);
			
			using(OpenFileDialog explorador = new OpenFileDialog()
			{
				InitialDirectory = @"C:\",
				RestoreDirectory = true,
				Title = "Adicionar arquivo",
				//DefaultExt = "exe",
				Filter = filtro,
			})
			{
				DialogResult res = explorador.ShowDialog();

				if(res == DialogResult.Cancel)
				{
					Console.WriteLine("Sem arquivo");
					return null;
				}

				c = explorador.FileName;
				explorador.Dispose();
			}

			return c;
		}
	}
}
