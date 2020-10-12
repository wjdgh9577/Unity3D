using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringInfo : TableData.IData<string>
{
    public string typeID;

    public string Korean;
    public string English;

    public Dictionary<Language, string> message;

    public string Key()
    {
        return typeID;
    }

    public void Setup()
    {
        message = new Dictionary<Language, string>();

        Add(Korean, English);
    }


    private void Add(string ko, string en)
    {
        message.Add(Language.Korean, ko);
        message.Add(Language.English, en);
    }
}
