namespace GameResources.Control.Economy.ResourceProgress.Scripts
{
    public class IslandResourceProgress
    {
        private readonly int _currentStage;
        private readonly int _totalStages;
        private StageResources[] _stages;

        public bool IsCompleted => _currentStage == _totalStages;
        
        public IslandResourceProgress(int numberOfStages, StageResources[] stages) 
        {
            _currentStage = 0;
            _totalStages = numberOfStages;
            _stages = stages;
        }
    }
}
