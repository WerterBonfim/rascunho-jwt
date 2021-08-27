using System;
using System.Diagnostics;
using System.Text.Json;
using LitJWT;
using LitJWT.Algorithms;
using Werter.Rascunhos.Compartilhado;

namespace Werter.Rascunhos.ComLitJwt
{
    internal class Program
    {
        public static void Main(string[] args) =>
            new ExemploComLitJwt().TestarLib();
    }

    public class ExemploComLitJwt
    {
        private readonly JwtEncoder _codificador;

        public ExemploComLitJwt()
        {
            // Gera uma chave aleatoria com um tamanho recomendado
            var chave = HS256Algorithm.GenerateRandomRecommendedKey();
            

            // Cria o codificador, JwtEncoder (thread-safe) armazena de forma recomendada 
            // um singleton estatico
            _codificador = new JwtEncoder(new HS256Algorithm(chave));
        }

        public void TestarLib()
        {
            var token = GerarToken();
            
            Console.WriteLine("\n\n");
            
            DecodificarToken(token);
        }

        private string GerarToken()
        {
            Console.WriteLine("Gerando um JWT com LitJWT");

            var cronometro = Stopwatch.StartNew();


            // codif
            var payload = new SessaoUsuario
            {
                Id = Guid.NewGuid(),
                Nome = "Werter",
                Email = "werter@wertersa.com.br"
            };

            var token = _codificador.Encode(payload, TimeSpan.FromMinutes(2),
                (x, writer) => writer.Write(Utf8Json.JsonSerializer.SerializeUnsafe(x)));

            cronometro.Stop();

            Console.WriteLine("token gerado:");
            Console.WriteLine(token);
            Console.WriteLine($"Tempo decorrido: {cronometro.Elapsed}");

            return token;
        }

        private void DecodificarToken(string token)
        {
            var cronometro = Stopwatch.StartNew();
            
            var decodificador = new JwtDecoder(_codificador.SignAlgorithm);

            var resultado = decodificador.TryDecode(token,
                x => Utf8Json.JsonSerializer
                    .Deserialize<SessaoUsuario>(x.ToArray()), out var playload);
            
            cronometro.Stop();
            
            if (resultado == DecodeResult.Success)
            {
                Console.WriteLine("Token decodificado:");
                Console.WriteLine((playload.Id, playload.Nome, playload.Email));
            }
            
            Console.WriteLine($"Tempo decorrido: {cronometro.Elapsed}");
        }
    }
}