namespace StaticAnalyzatorForCSharp
{
    internal class NamesMessage
    {
        private const string ifWarningMessage =
            "if и else приводят к одному результату! Файл: {0}, строка: {1}";
        private const string isThrowWarningMessage =
            "Cоздаётся экземпляр класса, унаследованного от 'System.Exception', но при этом никак не используется! Файл: {0}, строка: {1}";
        private const string isUpperSymbolInMethodMessage =
            "Метод: '{0}' объявлена с маленькой буквы. Файл: {1}, строка: {2}";
        private const string isLowerSymbolInVariableMessage =
            "Переменная: '{0}' объявлена с заглавной буквы. Файл: {1}, строка: {2}";
        private const string ifStateEqualsMessage =
            "В условие левая и правая части идентичны. Файл: {0}, строка: {1}";
        private const string correctNameVariableInForMessage =
            "Подозрительный цикл. Переменная цикла не увеличивается. Файл: {0}, строка: {1}";

        public static string IfWarningMessage => ifWarningMessage;
        public static string IsThrowWarningMessage => isThrowWarningMessage;
        public static string IsUpperSymbolInMethodMessage => isUpperSymbolInMethodMessage;
        public static string IsLowerSymbolInVariableMessage => isLowerSymbolInVariableMessage;
        public static string IfStateEqualsMessage => ifStateEqualsMessage;
        public static string СorrectNameVariableInForMessage => correctNameVariableInForMessage;
    }
}
