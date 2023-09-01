using UnityEngine;

public enum MessageType { Float, Integer, String, Boolean, VoidRun }
public enum YesOrNo { yes, no }
public enum MessageBehavior { OneStateChanged, Always }
public enum ParameterMode { OnlyParameter,ParameterPlusId,OnlyId,IdPlusParameter}

public static class Messager
{
    public static void RunVoid(Transform who, string methodName, string msgType, string parameterValue)
    {
        switch (msgType)
        {
            case "Float":
                float parsedFloat = 0;
                who.SendMessage(methodName, float.TryParse(parameterValue, out parsedFloat) ? parsedFloat : 0);
                break;
            case "Integer":
                int parsedInt = 0;
                who.SendMessage(methodName, int.TryParse(parameterValue, out parsedInt) ? parsedInt : 0);
                break;
            case "String":
                who.SendMessage(methodName, parameterValue);
                break;
            case "Boolean":
                bool parsedBoolean = false;
                who.SendMessage(methodName, bool.TryParse(parameterValue, out parsedBoolean) ? parsedBoolean : false);
                break;
            case "VoidRun":
                who.SendMessage(methodName);
                break;
        }
    }

    public static void RunVoid(Transform who, string methodName, string msgType, int parameterValue)
    {
        switch (msgType)
        {
            case "Float":
                who.SendMessage(methodName, parameterValue);
                break;
            case "Integer":
                who.SendMessage(methodName, parameterValue);
                break;
            case "String":
                who.SendMessage(methodName, parameterValue.ToString());
                break;
            case "Boolean":
                bool parsedBoolean = parameterValue == 0? false : true;
                who.SendMessage(methodName,parsedBoolean);
                break;
            case "VoidRun":
                who.SendMessage(methodName);
                break;
        }
    }
}