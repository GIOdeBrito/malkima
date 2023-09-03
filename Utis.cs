using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Windows.Forms;

namespace Malkima
{
	internal class Utis
	{	
		public static Tuple<int,int> ScreenSize ()
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

		public static Image ColouredImage (string path, Color colour)
		{
			if(!File.Exists(path))
			{
				throw new FileNotFoundException($"Arquivo {path} não pôde ser encontrado");
			}

			Bitmap bmp = new Bitmap(LoadImage(path));

			using(Graphics g = Graphics.FromImage(bmp))
			{
				ColorMap[] colorMap = new ColorMap[1];
				colorMap[0] = new ColorMap();
				colorMap[0].OldColor = Cor.UsarCor(0);
				colorMap[0].NewColor = colour;
				
				ImageAttributes attr = new ImageAttributes();
				attr.SetRemapTable(colorMap);
				
				Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
				g.DrawImage(bmp, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
			}

			Bitmap neoBmp = new Bitmap(bmp);
			bmp.Dispose();
			
			return neoBmp;
		}

		public static Bitmap LoadImage (string path)
		{
			if(!File.Exists(path))
			{
				throw new FileNotFoundException($"Arquivo {path} não pôde ser encontrado");
			}

			Bitmap img = null;

			using(FileStream stream = new FileStream(path, FileMode.Open))
			{
				img = (Bitmap) Image.FromStream(stream);
			}
			
			return img;
		}

		public static Bitmap ConverterParaPng (Bitmap bmp)
		{
			Bitmap img = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);

			using(Graphics graphics = Graphics.FromImage(img))
			{
				graphics.DrawImage(bmp, new Rectangle(0,0, bmp.Width,bmp.Height));
			}

			bmp.Dispose();

			return img;
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

		public static string ColetarArquivo (int filter_type = -1)
		{
			Func<int,string> defineFilter = (type) =>
			{
				string filter = string.Empty;
				
				switch(type)
				{
					case 1: { filter = "Capa (*.jpg);(*.png)|*.jpg;*.png"; } break;
					default: { filter = "Aplicação (*.exe)|*.exe"; } break;
				}
				
				return $"{filter}|Todos (*.*)|*.*";
			};

			string filtro = defineFilter(filter_type);
			string path = string.Empty;
			
			using(OpenFileDialog explorer = new OpenFileDialog()
			{
				//InitialDirectory = @"C:\",
				RestoreDirectory = true,
				Title = "Adicionar arquivo",
				//DefaultExt = "exe",
				Filter = filtro,
			})
			{
				DialogResult res = explorer.ShowDialog();

				if(res == DialogResult.Cancel)
				{
					Console.WriteLine("Sem arquivo");
					return null;
				}

				path = explorer.FileName;
			}

			return path;
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

		public static Control GetElement (string name)
		{
			Form1 f = Form1.UsarForma();
			
			Control elem = null;
			elem = f.Controls.Find(name,true).FirstOrDefault();

			if(elem is null)
			{
				throw new Exception("Elemento não encontrado no Form");
			}

			return elem;
		}
	}
}
