namespace WebAPIAutores.Servicios
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Archivo 1.txt";
        private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            /*
             en la funcion timer Se envia a funcion que quiero se ejecute
               el estado si lo tiene, en este ejemplo no lo tiene 
            diley incial
            cuanto tiempo se ejecuta el timer
             */
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // para detener el timer
            timer.Dispose();
            Escribir("Proceso finalizado");
            return Task.CompletedTask;
        }
        private void DoWork(Object state)
        {
            Escribir("proceso en ejecucion" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }
        private void Escribir(string mensaje)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            // el append sirve para no sustiuir el archivo anterior
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) 
            {
                writer.WriteLine(mensaje);
            }

        }
    }
}
