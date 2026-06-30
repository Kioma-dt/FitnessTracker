namespace FitnessTracker.Shared.Exceptions.InternalServerError
{
    public class EnviormnetVariableNotFoundException 
        : ApiException
    {
      public EnviormnetVariableNotFoundException(string details)
        : base(500, "Enviorment variable not set on server", details)
      { }
    }
}
