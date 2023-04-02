using System;
using System.Collections.Generic;
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
			Bitmap img = new Bitmap(c);

			using(Graphics g = Graphics.FromImage(img))
			{
				ColorMap[] colorMap = new ColorMap[1];
				colorMap[0] = new ColorMap();
				colorMap[0].OldColor = Color.White;
				colorMap[0].NewColor = cor;
				ImageAttributes attr = new ImageAttributes();
				attr.SetRemapTable(colorMap);
				
				Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
				g.DrawImage(img, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
			}

			return img;
		}
	}
}
