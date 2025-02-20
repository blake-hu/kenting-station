namespace KentingStation.Exception;

public class KsReregistrationException(string registeredObject)
    : System.Exception($"{registeredObject} is already registered and cannot be registered again.");