using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malkima
{
	internal class Processo
	{
		private static Process _processo;
		
		public static void IniciarAplicacao (string exec, string param = null)
		{
			Console.WriteLine($"{exec} | {param}");

			if(_processo != null)
			{
				return;
			}

			if(exec == null)
			{
				exec = "cmd.exe";
			}

			Form1.Minimizar();

			ProcessStartInfo startInfo = new ProcessStartInfo()
			{
				FileName = exec,
			};

			if(param != null)
			{
				startInfo.Arguments = @"/c " + param;
			}

			_processo = new Process() { StartInfo = startInfo, };
			_processo.Start();
			
			// Executa o código além após o processo encerrar
			_processo.WaitForExit();
			
			_processo.Dispose();
			_processo = null;

			Form1.Revelar();
		}
	}
}
