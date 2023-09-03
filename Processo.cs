using System;
using System.Diagnostics;

namespace Malkima
{
	internal class Processo
	{		
		public static void IniciarAplicacao (string exec, string param = null)
		{
			Console.WriteLine($"{exec} | {param}");

			if(exec == null)
			{
				exec = "cmd.exe";
			}

			Form1.HideForm();

			ProcessStartInfo startInfo = new ProcessStartInfo()
			{
				FileName = exec,
			};

			if(param != null)
			{
				startInfo.Arguments = @"/c " + param;
			}

			Process _processo = new Process()
			{
				StartInfo = startInfo,
			};
			
			_processo.Start();
			// Executa o código além após o processo encerrar
			_processo.WaitForExit();
			_processo.Dispose();

			Form1.ShowForm();
		}
	}
}
