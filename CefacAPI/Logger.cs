using System.Reflection;

namespace CefacAPI
{
    public static class Logger
    {
        private static string caminhoExe = string.Empty;

        public static bool Log(string strMensagem, params object?[] args)
        {
            try
            {

                var fileDateTime = DateTime.Now.Date.Day + "_" + DateTime.Now.Date.Month + "_" + DateTime.Now.Date.Year;
                string strNomeArquivo = fileDateTime + "_debug.txt";
                caminhoExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string caminhoArquivo = Path.Combine(caminhoExe, strNomeArquivo);
                if (!File.Exists(caminhoArquivo))
                {
                    FileStream arquivo = File.Create(caminhoArquivo);
                    arquivo.Close();
                }
                using (StreamWriter w = File.AppendText(caminhoArquivo))
                {
                    AppendLog(string.Format(strMensagem, args), w);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static void AppendLog(string logMensagem, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entrada : ");
                txtWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine($"  :{logMensagem}");
                txtWriter.WriteLine("------------------------------------");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
