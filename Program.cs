using System.Collections;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YTDownloader {
    internal class Program {
        private static YoutubeClient youtube = new YoutubeClient();
        private static String videoUrl = "",
                titulo = "",
                ruta = "//192.168.1.104/adrian/Multimedia/Musica/",
                rutaFija = "//192.168.1.104/adrian/Multimedia/Musica/",
                genero = "";

        static async Task Main(string[] args) {
            Console.WriteLine("=== Descargador de música de YouTube === ");
            Console.WriteLine("           Hecho por 0x3C0x33            ");
            Console.WriteLine("    usando: YouTubeExplode by Tyrrrz     ");

            Console.WriteLine("Elija modo de descarga 1:(canción); 2:(playlist)");
            switch (Convert.ToByte(Console.ReadKey())) {
                case 1: cancion(); break;
                case 2: lista(); break;
            }
        }

        private static async Task lista() {
            int contador = 1;
            do {
                Console.WriteLine($"\n --- Lista: {contador} ---");
                Console.WriteLine("Para salir: \\ o ç");
                Console.Write("URL: ");
                videoUrl = await pregunta();

                Console.Write("Ruta: ");
                String respuesta = await pregunta();
                ruta = "" == respuesta ? ruta : respuesta;

                Console.WriteLine("Nuevo Genero? (1:si, 2:NO)");
                if (Convert.ToByte(Console.ReadKey()) == 1) {
                    Console.Write("Nombre del nuevo genero: ");
                    string nuevoGenero = await pregunta();
                } else {
                    String[] generos = Directory.GetDirectories(ruta);
                    Console.Write("Generos: ");
                    for (global::System.Int32 i = 0; i < generos.Length; i++) {
                        Console.Write(i + 1 + ":" + Path.GetFileName(generos[i]) + " || ");
                    }
                    Console.Write("\nGenero: ");
                    respuesta = await pregunta();
                    genero = "" != respuesta ? Path.GetFileName(generos[int.Parse(respuesta) - 1]) + "/" : respuesta;
                }

                ruta = rutaFija + genero;
                titulo = await getTituloAsync(videoUrl);
                Console.WriteLine($"Descargando '{titulo}' en '{genero}' ruta: {ruta}");
                await descargar(videoUrl, titulo, ruta);
                contador++;
            } while (true);
            throw new NotImplementedException();
        }

        private static async Task cancion() {
            int contador = 1;
            do {
                Console.WriteLine($"\n --- Musica: {contador} ---");
                Console.WriteLine("Para salir: \\ o ç");
                Console.Write("URL: ");
                videoUrl = await pregunta();

                Console.Write("Ruta: ");
                String respuesta = await pregunta();
                ruta = "" == respuesta ? ruta : respuesta;

                String[] generos = Directory.GetDirectories(ruta);
                Console.Write("Generos: ");
                for (global::System.Int32 i = 0; i < generos.Length; i++) {
                    Console.Write(i + 1 + ":" + Path.GetFileName(generos[i]) + " || ");
                }
                Console.Write("\nGenero: ");
                respuesta = await pregunta();
                genero = "" != respuesta ? Path.GetFileName(generos[int.Parse(respuesta) - 1]) + "/" : respuesta;

                ruta = rutaFija + genero;
                titulo = await getTituloAsync(videoUrl);
                Console.WriteLine($"Descargando '{titulo}' en '{genero}' ruta: {ruta}");
                await descargar(videoUrl, titulo, ruta);
                contador++;
            } while (true);
        }

        private static async Task<string> pregunta() {
            String respuesta = "";
            try {
                respuesta = Console.ReadLine();
                if (respuesta == "\\" || respuesta == "ç") {
                    Console.WriteLine("Saliendo...");
                    Environment.Exit(0);
                } else if (respuesta == "") {
                    return "";
                }
                return respuesta;
            } catch (Exception) {
                throw;
            }
        }

        private static async Task descargar(string videoUrl, string titulo, string ruta) {
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
            try {
                await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{ruta}{titulo}.mp3");
            } catch (System.IO.IOException) {
                Console.WriteLine("Error al escribir el nombre del archivo, renombrado como: musica.mp3");
                Console.WriteLine("Por favor cambie el nombre del archivo cuanto antes...");
                await youtube.Videos.Streams.DownloadAsync(streamInfo, $"{ruta}musica.mp3");
            }
        }

        private static async Task<string> getTituloAsync(string videoUrl) {
            var video = await youtube.Videos.GetAsync(videoUrl);
            return video.Title;
        }
    }
}
