namespace Malkima
{
	public partial class Form1 : Form
	{
		private static Form1 f;
		private static System.Windows.Forms.Timer tmr;

		public Form1()
		{
			InitializeComponent();

			f = this;

			//Temporizador();
		}

		public static Form1 UsarForma()
		{
			if (f == null)
			{
				throw new Exception("Form1 não existe");
			}

			return f;
		}

		private void Temporizador()
		{
			tmr = new System.Windows.Forms.Timer();

			// Milisegundos
			tmr.Interval = 15;

			// A cada tick esta funcao sera executada
			tmr.Tick += (s, e) =>
			{
				Fundo.Chover();
			};

			tmr.Stop();
			tmr.Start();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Inicio.Inicializar();
			//Fundo.IniciarChuva();
		}
	}
}