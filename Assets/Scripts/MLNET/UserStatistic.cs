using Microsoft.ML.Data;

namespace MLNetDll
{
    // <Snippet2>
    public class UserStatistic
    {
        [LoadColumn(0)]
        public float numberLevel;

        [LoadColumn(1)]
        public float countAllInstruction;

        [LoadColumn(2)]
        public float countIfInstruction;

        [LoadColumn(3)]
        public float countLoopInstruction;

        [LoadColumn(4)]
        public float timeComplitingLastLevel;

        [LoadColumn(5)]
        public float timeForCompleteCurrentLevel;
    }

    public class UserStatisticPrediction
    {
        [ColumnName("Score")]
        public float timeForCompleteCurrentLevel;
    }
    // </Snippet2>
}
