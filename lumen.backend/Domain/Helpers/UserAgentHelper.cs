namespace Domain.Helpers;

public static class UserAgentHelper
{
    public static string DetectarDispositivo(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent)) return "Desconhecido";
        
        if (Regex.IsMatch(userAgent, @"Mobile|Android|iPhone|iPod", RegexOptions.IgnoreCase))
            return "Mobile";
        
        if (Regex.IsMatch(userAgent, @"Tablet|iPad", RegexOptions.IgnoreCase))
            return "Tablet";
        
        return "Desktop";
    }

    public static string DetectarNavegador(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent)) 
            return "Desconhecido";
        
        if (userAgent.Contains("Edg")) 
            return "Edge";
        
        if (userAgent.Contains("Chrome")) 
            return "Chrome";
        
        if (userAgent.Contains("Firefox")) 
            return "Firefox";
        
        if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome")) 
            return "Safari";
        
        return "Outros";
    }

    public static string DetectarSo(string userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent)) 
            return "Desconhecido";
        
        if (userAgent.Contains("Windows")) 
            return "Windows";
        
        if (userAgent.Contains("Mac OS")) 
            return "MacOS";
        
        if (userAgent.Contains("Android")) 
            return "Android";
        
        if (userAgent.Contains("iOS") || userAgent.Contains("iPhone") || userAgent.Contains("iPad")) 
            return "iOS";
        
        if (userAgent.Contains("Linux")) 
            return "Linux";
        
        return "Outros";
    }
}