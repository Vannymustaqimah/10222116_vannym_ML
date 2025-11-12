using Microsoft;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;
using System.Linq;

public class HoaxInput
{

    [LoadColumn(0)]
    public string? Label { get; set; }

    [LoadColumn(1)]
    public string? Text { get; set; }
}

public class HoaxPrediction
{
    [ColumnName("PredictedLabel")]
    public string? PredictedLabel { get; set; } 

    [ColumnName("Score")]
    public float[]? Score { get; set; }
}

public class Program
{
    private static readonly string DATA_FILEPATH = Path.Combine(Environment.CurrentDirectory, "berita_HOAX_indonesia.csv");
    private static readonly string MODEL_FILEPATH = Path.Combine(Environment.CurrentDirectory, "hoax_detector_model.zip");
    
    private static readonly MLContext mlContext = new MLContext(seed: 0);

    public static void Main(string[] args)
    {
        Console.WriteLine($"Memuat dataset dari: {DATA_FILEPATH}");
        IDataView fullData = mlContext.Data.LoadFromTextFile<HoaxInput>(
            path: DATA_FILEPATH,
            separatorChar: ';',  
            hasHeader: true,     
            allowQuoting: true   
        );

        Console.WriteLine("Memfilter data... Menghapus baris yang labelnya kosong.");

        var dataEnumerable = mlContext.Data.CreateEnumerable<HoaxInput>(fullData, reuseRowObject: false);

        var filteredEnumerable = dataEnumerable.Where(row => !string.IsNullOrEmpty(row.Label));

        IDataView filteredData = mlContext.Data.LoadFromEnumerable(filteredEnumerable);
        
        var pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Label", outputColumnName: "LabelKey")
            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: "Text"))
            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "Features"))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        var splitData = mlContext.Data.TrainTestSplit(filteredData, testFraction: 0.2); 

        Console.WriteLine("========== Mulai Pelatihan Model ==========");
        ITransformer model = pipeline.Fit(splitData.TrainSet);
        Console.WriteLine("========== Pelatihan Selesai ==========\n");

        EvaluateModel(model, splitData.TestSet);

        mlContext.Model.Save(model, filteredData.Schema, MODEL_FILEPATH);
        Console.WriteLine($"\nModel berhasil disimpan di: {MODEL_FILEPATH}");
        Console.WriteLine("File .zip ini yang akan di-upload ke GitHub.\n");

        TestSinglePrediction(model);
    }

    private static void EvaluateModel(ITransformer model, IDataView testData)
    {
        var predictions = model.Transform(testData);
        var metrics = mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "LabelKey");

        Console.WriteLine("========== Evaluasi Model (Akurasi) ==========");
        Console.WriteLine($"* Akurasi (Micro): {metrics.MicroAccuracy:P2}");
        Console.WriteLine($"* Akurasi (Macro): {metrics.MacroAccuracy:P2}");
        Console.WriteLine($"* Log Loss: {metrics.LogLoss:F2}");
        Console.WriteLine("==================================================");
    }

    private static void TestSinglePrediction(ITransformer model)
    {
        var predictionEngine = mlContext.Model.CreatePredictionEngine<HoaxInput, HoaxPrediction>(model);

        var sampleData = new HoaxInput
        {
            Text = "Ditemukan planet baru di Tasikmalaya yang terbuat dari emas murni kata peneliti ITB."
        };

        var prediction = predictionEngine.Predict(sampleData);

        Console.WriteLine("\n========== Uji Coba Prediksi Tunggal ==========");
        Console.WriteLine($"Teks Berita: {sampleData.Text}");
        Console.WriteLine($"Hasil Prediksi: {prediction.PredictedLabel}");
        Console.WriteLine("=============================================");
    }
}