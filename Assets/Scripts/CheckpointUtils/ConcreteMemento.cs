namespace CheckpointUtils
{
    public class ConcreteMemento : IMemento
    {
        private CheckpointInfo _checkpointInfo;

        public ConcreteMemento(CheckpointInfo checkpointInfo)
        {
            _checkpointInfo = checkpointInfo;
        }
        
        public CheckpointInfo GetCheckpointInfo()
        {
            return _checkpointInfo;
        }
    }
}