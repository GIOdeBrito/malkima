namespace Malkima
{
	public partial class Form1 : Form
	{
		private static Form1 f;

		public Form1()
		{
			InitializeComponent();

			f = this;
		}

		public static Form1 UsarForma()
		{
			if(f == null)
			{
				throw new Exception("Form1 não existe");
			}

			return f;
		}

		public static void HideForm ()
		{
			f.WindowState = FormWindowState.Minimized;
		}

		public static void ShowForm ()
		{
			f.WindowState = FormWindowState.Normal;
		}

		public static void Fechar ()
		{
			Application.Exit();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Inicio.Initialize();
		}
	}
}