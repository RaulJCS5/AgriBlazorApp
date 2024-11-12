namespace AgriBlazorApp
{
    public class ChuckNorrisApiClient
    {
        private readonly HttpClient _httpClient;

        public ChuckNorrisApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(ChuckNorrisJoke? Joke, string? ErrorMessage)> GetRandomJokeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Wait for 2 seconds before fetching the joke
                await Task.Delay(2000, cancellationToken);

                var joke = await _httpClient.GetFromJsonAsync<ChuckNorrisJoke>("/jokes/random", cancellationToken);
                return (joke, null);
            }
            catch (OperationCanceledException)
            {
                return (null, "The operation was canceled.");
            }
            catch (HttpRequestException httpEx)
            {
                return (null, $"Request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                return (null, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    public record ChuckNorrisJoke(string IconUrl, string Id, string Url, string Value);
}
