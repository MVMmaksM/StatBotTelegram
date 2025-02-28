namespace StatBotTelegram.Helpers;

public static class SplitterMessage
{
    private const int MAX_LENGTH_MESSAGE = 4096;
    
    public static List<string> SplitMessage(string message)
    {
        var countMessage = (int)Math.Ceiling((double)message.Length / MAX_LENGTH_MESSAGE);
        var splitMessages = new List<string>();

        if (countMessage == 1)
        {
            splitMessages.Add(message);
            return splitMessages;
        }
        
        for (int i = 0, startIndex = 0; i < countMessage; i++, startIndex += MAX_LENGTH_MESSAGE)
        {
            if (i == countMessage - 1)
            {
                splitMessages.Add(message.Substring(startIndex));
            }
            else
            {
                splitMessages.Add(message.Substring(startIndex, MAX_LENGTH_MESSAGE));
            }
        }
        
        return splitMessages;
    }
}