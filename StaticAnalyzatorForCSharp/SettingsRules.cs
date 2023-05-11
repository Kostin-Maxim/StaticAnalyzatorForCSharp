﻿using System.Collections.Generic;
using System.Configuration;

namespace StaticAnalyzatorForCSharp
{
    internal class SettingsRules
    {
        public static readonly Dictionary<string, string> translateErrorsForRussian = new Dictionary<string, string>()
        {
            {"ifWarningMessage", "Проверка выходных данных конструкции if-else" },
            {"isThrowWarningMessage", "Проверка выделения памяти для исключения" },
            {"isUpperSymbolInMethodMessage", "Проверка наименование метода" },
            {"isLowerSymbolInVariableMessage", "Проверка наименования переменной" },
            {"logicNotEqualsFormat", "Проверка форматирования конструкции if"},
            {"ifStateEquals", "Проверка однозначного условия" }
        };

        public enum NamesErrors
        {
            ifWarningMessage,
            isThrowWarningMessage,
            isUpperSymbolInMethodMessage,
            isLowerSymbolInVariableMessage,
            logicNotEqualsFormat,
            ifStateEquals
        }

        private static readonly Dictionary<NamesErrors, bool> rules = new Dictionary<NamesErrors, bool>()
        {
            {NamesErrors.ifWarningMessage, GetValueSetting(NamesErrors.ifWarningMessage) },
            {NamesErrors.isThrowWarningMessage, GetValueSetting(NamesErrors.isThrowWarningMessage) },
            {NamesErrors.isUpperSymbolInMethodMessage, GetValueSetting(NamesErrors.isUpperSymbolInMethodMessage) },
            {NamesErrors.isLowerSymbolInVariableMessage, GetValueSetting(NamesErrors.isLowerSymbolInVariableMessage) },
            {NamesErrors.logicNotEqualsFormat, GetValueSetting(NamesErrors.logicNotEqualsFormat) },
            {NamesErrors.ifStateEquals, GetValueSetting(NamesErrors.ifStateEquals) }
        };

        public static void SetDictionary(NamesErrors key, bool value)
        {
            rules[key] = value;
        }

        public static bool GetDictionary(NamesErrors key)
        {
            return rules[key];
        }

        private static bool GetValueSetting(NamesErrors nameError)
        {
            foreach (var rule in Properties.Settings.Default.PropertyValues)
            {
                var currentRule = (SettingsPropertyValue)rule;
                if (nameError.ToString() == currentRule.Name)
                {
                    return (bool)currentRule.PropertyValue;
                }
            }
            return false;
        }
    }
}
