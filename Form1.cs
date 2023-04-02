namespace Malkima
{
	public partial class Form1 : Form
	{
		public static Form f = null;
		private static System.Windows.Forms.Timer tmr;

		public Form1()
		{
			InitializeComponent();

			f = this;

			Temporizador();
		}

		private void Temporizador ()
		{
			tmr = new System.Windows.Forms.Timer();

			// Milisegundos
			tmr.Interval = 15;

			// A cada tick esta funcao sera executada
			tmr.Tick += (s,e) =>
			{
				Fundo.Chover();
			};

			tmr.Stop();
			tmr.Start();
		}

		public static Form UsarForma()
		{
			return f;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Cores.LerCores();
			Cores.LerTemas();
			Fontes.CarregarFontes();

			Inicio.Inicializar();
			Fundo.IniciarChuva();
		}
	}
}