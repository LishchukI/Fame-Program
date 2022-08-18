using Microsoft.ML;
using System;
using System.IO;
using UnityEngine;

namespace MLNetDll
{
    class MLNET
    {
        static readonly string _trainDataPath = Application.dataPath + "/Resources/CompletedLevelsStatistic.csv";
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

        public static float StartPrediction(int numberLevel, int countAllInstruction, int countIfInstruction, int countLoopInstruction, int timeForCompleteCurrentLevel)
        {
            MLContext mlContext = new MLContext(seed: 0);

            var model = Train(mlContext, _trainDataPath);

            var userStatisticSample = new UserStatistic()
            {
                numberLevel = numberLevel,
                countAllInstruction = countAllInstruction,
                countIfInstruction = countIfInstruction,
                countLoopInstruction = countLoopInstruction,
                timeComplitingLastLevel = timeForCompleteCurrentLevel,
                timeForCompleteCurrentLevel = 0
            };
            float predittionTime = TestSinglePrediction(mlContext, model, userStatisticSample);
            return predittionTime;
        }

        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<UserStatistic>(dataPath, hasHeader: true, separatorChar: ',');
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "timeForCompleteCurrentLevel")
                    .Append(mlContext.Transforms.Concatenate("Features", "numberLevel", "countAllInstruction", "countIfInstruction", "countLoopInstruction", "timeComplitingLastLevel"))
                    .Append(mlContext.Regression.Trainers.FastTree());


            Console.WriteLine("=============== Create and Train the Model ===============");

            var model = pipeline.Fit(dataView);

            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();

            return model;
        }


        private static float TestSinglePrediction(MLContext mlContext, ITransformer model, UserStatistic userStatisticSample)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<UserStatistic, UserStatisticPrediction>(model);

            var prediction = predictionFunction.Predict(userStatisticSample);

            return (prediction.timeForCompleteCurrentLevel);
            Debug.Log(prediction.timeForCompleteCurrentLevel);
            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted fare: {prediction.timeForCompleteCurrentLevel:0.####}");
            Console.WriteLine($"**********************************************************************");
        }
    }
}
