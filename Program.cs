/*using System;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Establecer la dirección IP y el puerto en el que escuchará el servidor
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // Escuchará en localhost
            int port = 8888; // Puerto de ejemplo

            // Crear un objeto TcpListener
            TcpListener listener = new TcpListener(ipAddress, port);

            try
            {
                // Comenzar a escuchar peticiones
                listener.Start();
                Console.WriteLine("Servidor iniciado. Esperando conexiones...");

                while (true)
                {
                    // Aceptar la conexión del cliente
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Cliente conectado.");

                    // Obtener el flujo de datos de la conexión
                    NetworkStream stream = client.GetStream();

                    // Crear un lector de datos
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string requestData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Petición recibida: " + requestData);

                    // Obtener la descripción de la computadora local utilizando WMI
                    string computerDescription = GetComputerDescription();

                    Console.WriteLine(">>> " + computerDescription);

                    // Enviar la descripción de la computadora al cliente
                    //byte[] responseBuffer = Encoding.ASCII.GetBytes(computerDescription);
                    byte[] responseBuffer = Encoding.ASCII.GetBytes(computerDescription);
                    Console.WriteLine(">>> " + responseBuffer);


                    stream.Write(responseBuffer, 0, responseBuffer.Length);

                    // Cerrar la conexión con el cliente
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Detener el servidor
                listener.Stop();
            }
        }

        // Método para obtener la descripción de la computadora local utilizando WMI
        static string GetComputerDescription()
        {
            string description = "Descripción de la computadora local:\n";
            try
            {
                // Crear una instancia de la clase ManagementObjectSearcher
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");

                // Iterar sobre los resultados y obtener la descripción
                //foreach (ManagementObject queryObj in searcher.Get())
               // {
                //    description += $"Nombre: {queryObj["Name"]}\n";
                //    description += $"Fabricante: {queryObj["Manufacturer"]}\n";
               //     description += $"Modelo: {queryObj["Model"]}\n";
                //    description += $"Tipo: {queryObj["SystemType"]}\n";
                //}

                ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
                foreach (ManagementObject queryObj in osClass.GetInstances())
                {
                    foreach (PropertyData prop in queryObj.Properties)
                    {
                        //add these to your arraylist or dictionary 
                        if (String.Equals(prop.Name, "Description"))
                        {
                            Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
                            //description = $"Description: {queryObj["Description"]}\n";
                            description = prop.Value.ToString();
                            break;
                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                description = "Error al obtener la descripción de la computadora: " + e.Message;
            }
            return description;
        }
    }
}*/

using System;
using System.Management;
using System.Net;
using System.Text;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Establecer la URL y el puerto en el que escuchará el servidor
            string url = "http://localhost:8888/";

            // Crear un objeto HttpListener
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);

            try
            {
                // Comenzar a escuchar peticiones
                listener.Start();
                Console.WriteLine("Servidor HTTP iniciado. Esperando conexiones...");

                while (true)
                {
                    // Aceptar la conexión del cliente
                    HttpListenerContext context = listener.GetContext();
                    Console.WriteLine("Cliente conectado.");

                    // Obtener el objeto HttpListenerRequest
                    HttpListenerRequest request = context.Request;

                    // Obtener el objeto HttpListenerResponse
                    HttpListenerResponse response = context.Response;


                    // Configurar los encabezados CORS
                    response.AddHeader("Access-Control-Allow-Origin", "*");
                    response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                    response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                    // Manejar las solicitudes OPTIONS (pre-flight request)
                    if (request.HttpMethod == "OPTIONS")
                    {
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Close();
                        continue;
                    }

                    // Obtener la descripción de la computadora local utilizando WMI
                    string computerDescription = GetComputerDescription();

                    // Convertir la respuesta a bytes
                    byte[] buffer = Encoding.UTF8.GetBytes(computerDescription);
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "text/plain";

                    // Obtener el flujo de datos de la respuesta y enviar la respuesta
                    using (System.IO.Stream output = response.OutputStream)
                    {
                        output.Write(buffer, 0, buffer.Length);
                    }

                    Console.WriteLine("Respuesta enviada al cliente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Detener el servidor
                listener.Stop();
            }
        }

        // Método para obtener la descripción de la computadora local utilizando WMI
        static string GetComputerDescription()
        {
            string description = "Descripción de la computadora local:\n";
            try
            {
                // Crear una instancia de la clase ManagementObjectSearcher
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");

                // Iterar sobre los resultados y obtener la descripción
               

                ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
                foreach (ManagementObject queryObj in osClass.GetInstances())
                {
                    foreach (PropertyData prop in queryObj.Properties)
                    {
                        //add these to your arraylist or dictionary 
                        if (String.Equals(prop.Name, "Description"))
                        {
                            Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
                            //description = $"Description: {queryObj["Description"]}\n";
                            description = prop.Value.ToString();
                            break;
                        }
                    }
                }
            }
            catch (ManagementException e)
            {
                description = "Error al obtener la descripción de la computadora: " + e.Message;
            }
            return description;
        }
    }
}

