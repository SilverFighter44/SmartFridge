using UnityEngine;

public static class UITools
{
    public static char ValidateCharInclusie(string validCharacters, char addedChar)
    {
        return validCharacters.Contains(addedChar) ? addedChar : '\0';
    }

    public static char ValidateCharExclusive(string invalidCharacters, char addedChar)
    {
        //"\\/:*?\"<>|"
        return invalidCharacters.Contains(addedChar) ? '\0' : addedChar;
    }
}
