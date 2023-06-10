using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Numerics;
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

			Bitmap bmp = new Bitmap(GIOImagem(c));

			using(Graphics g = Graphics.FromImage(bmp))
			{
				ColorMap[] colorMap = new ColorMap[1];
				colorMap[0] = new ColorMap();
				colorMap[0].OldColor = Cor.UsarCor(0);
				colorMap[0].NewColor = cor;
				
				ImageAttributes attr = new ImageAttributes();
				attr.SetRemapTable(colorMap);
				
				Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
				g.DrawImage(bmp, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
			}

			Bitmap neoBmp = new Bitmap(bmp);
			bmp.Dispose();
			
			return neoBmp;
		}

		public static Bitmap GIOImagem (string c)
		{
			if(!File.Exists(c))
			{
				throw new FileNotFoundException($"Arquivo {c} não pôde ser encontrado");
			}

			Bitmap img = null;

			using(FileStream stream = new FileStream(c, FileMode.Open))
			{
				Bitmap bmp = new Bitmap(stream);
				img = (Bitmap) Image.FromStream(stream);

				bmp.Dispose();
			}
			
			return img;
		}

		public static Bitmap ConverterParaPng (Bitmap bmp)
		{
			Bitmap imagem = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);

			using(Graphics graphics = Graphics.FromImage(imagem))
			{
				graphics.DrawImage(bmp, new Rectangle(0,0, bmp.Width,bmp.Height));
			}

			bmp.Dispose();

			return imagem;
		}

		public static Bitmap Redimensionar (Bitmap bmp, Vector2 v = default(Vector2))
		{
			if(v == default(Vector2))
			{
				v.X = 256;
				v.Y = 256;
			}
			
			Bitmap image = new Bitmap((int)v.X, (int)v.Y, PixelFormat.Format32bppArgb);

			using(Graphics graphics = Graphics.FromImage(image))
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(bmp, new Rectangle(0,0, (int)v.X,(int)v.Y));
			}

			bmp.Dispose();

			return image;
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

		public static void CriarDiretorio (string dir)
		{
			if(Directory.Exists(dir))
			{
				return;
			}

			Directory.CreateDirectory(dir);
		}

		public static void RemoverDiretorio (string dir, bool recur = false)
		{
			if(!Directory.Exists(dir))
			{
				return;
			}

			Directory.Delete(dir, recur);
		}

		public static void RemoverArquivo (string dir)
		{
			if(!File.Exists(dir))
			{
				return;
			}

			File.Delete(dir);
		}
	}
}
