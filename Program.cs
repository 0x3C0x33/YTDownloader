using System.Collections;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YTDownloader {
    internal class Program {
        private static YoutubeClient youtube = new YoutubeClient();
        private static String videoUrl = "",
                titulo = "",
                ruta = "",
                rutaFija = "",
                genero = "";

        public static void Main(string[] args) {
            Console.WriteLine("=== Descargador de música de YouTube === ");
            Console.WriteLine("           Hecho por 0x3C0x33            ");
            Console.WriteLine("    usando: YouTubeExplode by Tyrrrz     ");

            Console.WriteLine("Elija modo de descarga 1:(canción); 2:(playlist)");
            
            switch (Convert.ToByte(Console.ReadLine())) {
                case 1: cancion(); break;
                case 2: lista(); break;
            }
        }

        //TODO: Lista por terminar
        private static void lista() {
            int contador = 1;
            do {
                Console.WriteLine($"\n --- Lista: {contador} ---");
                Console.WriteLine("Para salir: \\ o ç");
                Console.Write("URL: ");
                videoUrl = pregunta();

                Console.Write("Ruta: ");
                String respuesta = pregunta();
                ruta = "" == respuesta ? ruta : respuesta;

                Console.WriteLine("Nuevo Genero? (1:si, 2:NO)");
                if (Convert.ToByte(Console.ReadLine()) == 1) {
                    Console.Write("Nombre del nuevo genero: ");
                    string nuevoGenero = pregunta();
                } else {
                    String[] generos = Directory.GetDirectories(ruta);
                    Console.Write("Generos: ");
                    for (global::System.Int32 i = 0; i < generos.Length; i++) {
                        Console.Write(i + 1 + ":" + Path.GetFileName(generos[i]) + " || ");
                    }
                    Console.Write("\nGenero: ");
                    respuesta = pregunta();
                    genero = "" != respuesta ? Path.GetFileName(generos[int.Parse(respuesta) - 1]) + "/" : respuesta;
                }

                ruta = rutaFija + genero;
                getTituloAsync();
                Console.WriteLine($"Descargando '{titulo}' en '{genero}' ruta: {ruta}");
                descargar();
                contador++;
            } while (true);
            throw new NotImplementedException();
        }

        private static void cancion() {
            int contador = 1;
            do {
                Console.WriteLine($"\n --- Musica: {contador} ---");
                Console.WriteLine("Para salir: \\ o ç");
                Console.Write("URL: ");
                videoUrl = pregunta();

                Console.Write("Ruta: ");
                String respuesta = pregunta();
                rutaFija = "" == respuesta ? rutaFija : respuesta;
                String[] generos = new String[1];
                try {
                    generos = Directory.GetDirectories(rutaFija);

                    Console.Write("Generos: ");
                    for (global::System.Int32 i = 0; i < generos.Length; i++) {
                        Console.Write(i + 1 + ":" + Path.GetFileName(generos[i]) + " || ");
                    }
                } catch (Exception) {

                }
                Console.Write("\nGénero (Nombre para crear género): ");
                respuesta = pregunta();
                try {
                    genero = "" != respuesta ? Path.GetFileName(generos[int.Parse(respuesta) - 1]) + "/" : respuesta;
                } catch (FormatException) {
                    genero = respuesta;
                    Directory.CreateDirectory(rutaFija + "/" + genero);
                }
                ruta = rutaFija + "/" + genero;
                getTituloAsync();
                Console.WriteLine($"Descargando '{titulo}' en '{genero}' ruta: {ruta}");
                descargar();
                contador++;
            } while (true);
        }

        private static string pregunta() {
            String respuesta = "";
            try {
                respuesta = Console.ReadLine();
                if (respuesta == @"\" || respuesta == "ç") {
                    Console.WriteLine("Saliendo...");
                    Environment.Exit(0);
                }
                return respuesta;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async void descargar() {
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

        private static async void getTituloAsync() {
            var vid = await youtube.Videos.GetAsync(videoUrl);
            titulo = vid.Title;
        }
    }
}
