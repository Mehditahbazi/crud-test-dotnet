namespace Mc2.CrudTest.AcceptanceTests.Hooks
{
    [Binding]
    public class Hooks
    {
        [BeforeScenario]
        public void BeforeScenario()
        {

            //_factory = new WebApplicationFactory<Program>();
            //var client = _factory.CreateClient();
            //_objectContainer.RegisterInstanceAs(client); // Register HttpClient
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //_factory?.Dispose(); // Dispose of the WebApplicationFactory
        }
    }
}