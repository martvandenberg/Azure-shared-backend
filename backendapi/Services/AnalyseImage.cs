using Azure.AI.Vision.Common;
using Azure;
using Azure.Identity;
using Azure.AI.Vision.ImageAnalysis;
using System.Text;
using backendapi.Controllers;

namespace backendapi.Services
{
    public class AnalyseImage
    {
        private readonly ILogger<AnalyseImage> _logger;
        private readonly IConfiguration _configuration;

        public AnalyseImage(IConfiguration configuration, ILogger<AnalyseImage> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        


        public void AnalyseImageWithAi(string urlSasString)
        {
            _logger.LogInformation($"LOG INFO. urls {urlSasString}");
            StringBuilder analysisResult = new StringBuilder();
            analysisResult.AppendLine("Starting image analysing tool Ai");
            Console.WriteLine($"{urlSasString}");

            var visionEndpoint = _configuration["ComputerVision:Endpoint"];
            var visionKey = _configuration["ComputerVision:ApiKey"];

            var serviceOptions = new VisionServiceOptions(
                visionEndpoint,
                new AzureKeyCredential(visionKey));


            using var imageSource =  VisionSource.FromUrl(urlSasString);
            //using var imageSource2 = VisionSource.FromUrl("https://aka.ms/azai/vision/image-analysis-sample.jpg");



            var analysisOptions = new ImageAnalysisOptions()
            {
                Features = ImageAnalysisFeature.Caption | ImageAnalysisFeature.Text,

                Language = "en",

                GenderNeutralCaption = true
            };

            using var analyzer = new ImageAnalyzer(serviceOptions, imageSource, analysisOptions);
            var result = analyzer.Analyze();

            if (result.Reason == ImageAnalysisResultReason.Analyzed)
            {
                if (result.Caption != null)
                {
                    analysisResult.AppendLine($"Caption: \"{result.Caption.Content}\", Confidence {result.Caption.Confidence:0.0000}");
                }

                if (result.Text != null)
                {
                    analysisResult.AppendLine("Text:");
                    foreach (var line in result.Text.Lines)
                    {
                        analysisResult.AppendLine($"   Line: '{line.Content}'");
                    }
                }
            }
            else
            {
                var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
                analysisResult.AppendLine("Analysis failed.");
                analysisResult.AppendLine($"   Error reason : {errorDetails.Reason}");
                analysisResult.AppendLine($"   Error code : {errorDetails.ErrorCode}");
                analysisResult.AppendLine($"   Error message: {errorDetails.Message}");
                
            }
            _logger.LogInformation($"LOG INFO. {analysisResult.ToString()}");
            Console.WriteLine(analysisResult.ToString());
        }
    }
}
