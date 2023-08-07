using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;
using Microsoft.SemanticKernel.AI.TextCompletion;

namespace Prototype
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.development.json", true).Build();

                var kernel = new KernelBuilder().WithOpenAITextCompletionService(configuration["modelId"], configuration["apiKey"], configuration["orgId"]).Build();

                var function_definition = "Explain the Physics law '{{$input}}' with examples, like explaining to a 5 year old kid.";

                var arguments = "Laws of Thermodynamics";

                var function_reference = kernel.CreateSemanticFunction(function_definition);

                function_reference.UseCompletionSettings(new CompleteRequestSettings { MaxTokens = 1024 });

                var function_response = await function_reference.InvokeAsync(arguments);

                Console.WriteLine(function_response.Result);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            finally
            {
                Console.WriteLine("\n\nPress any key to exit...");

                Console.ReadKey();
            }
        }
    }
}