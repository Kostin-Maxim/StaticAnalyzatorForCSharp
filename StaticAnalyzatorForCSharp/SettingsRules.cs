using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StaticAnalyzatorForCSharp
{
    internal class SettingsRules
    {
        public enum NamesErrors
        {
            ifWarningMessage,
            isThrowWarningMessage,
            isUpperSymbolInMethodMessage,
            isLowerSymbolInVariableMessage
        }

        private static Dictionary<NamesErrors, bool> rules = new Dictionary<NamesErrors, bool>()
        {
            {NamesErrors.ifWarningMessage, true },
            {NamesErrors.isThrowWarningMessage, true },
            {NamesErrors.isUpperSymbolInMethodMessage, true },
            {NamesErrors.isLowerSymbolInVariableMessage, true }
        };

        public static void SetDictionary(NamesErrors key, bool value)
        {
            rules[key] = value;
        }

        public static bool GetDictionary(NamesErrors key) 
        { 
            return rules[key];
        }
    }
}
