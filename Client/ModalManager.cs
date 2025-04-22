namespace ShopPlatform.Client
{
    public class ModalManager
    {
        private TaskCompletionSource<bool> _confirmationTcs;

        public event Action<string> OnShow;
        public event Action<string, string> OnConfirmRequest;

        public void Show(string modalName)
        {
            OnShow?.Invoke(modalName);
        }

        // Onay isteyen modal
        public Task<bool> ConfirmationAsync(string title, string message)
        {
            _confirmationTcs = new TaskCompletionSource<bool>();
            OnConfirmRequest?.Invoke(title, message);
            return _confirmationTcs.Task;
        }

        public void Confirm() => _confirmationTcs?.TrySetResult(true);
        public void Cancel() => _confirmationTcs?.TrySetResult(false);
    }
}
