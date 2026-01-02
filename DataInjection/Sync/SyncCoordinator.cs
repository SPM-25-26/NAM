namespace DataInjection.Sync
{
    public interface ISyncCoordinator
    {
        Task WaitForDailySyncCompletionAsync(CancellationToken cancellationToken = default);
        void NotifyDailySyncCompleted();
    }

    public class SyncCoordinator : ISyncCoordinator
    {
        private readonly SemaphoreSlim _dailySyncCompletedSignal = new(0, 1);
        private bool _firstSyncCompleted = false;

        public async Task WaitForDailySyncCompletionAsync(CancellationToken cancellationToken = default)
        {
            if (_firstSyncCompleted)
            {
                return;
            }

            await _dailySyncCompletedSignal.WaitAsync(cancellationToken);
            _dailySyncCompletedSignal.Release();
        }

        public void NotifyDailySyncCompleted()
        {
            if (!_firstSyncCompleted)
            {
                _firstSyncCompleted = true;
                _dailySyncCompletedSignal.Release();
            }
        }
    }
}