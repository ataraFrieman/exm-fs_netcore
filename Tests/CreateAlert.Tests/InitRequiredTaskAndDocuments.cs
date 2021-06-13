using Quze.Models.Entities;


namespace CreateAlerts.Test
{
    class InitRequiredTaskAndDocuments
    {
        public RequiredTask GetRequiredTask1()
        {
            var getAlertRule = new InitAletRule();
            return new RequiredTask()
            {
                Code = "184",
                Description = "טופס 17",
                IsRequired = true,
                ServiceTypeID = 1,

                AlertRules = getAlertRule.GetAlertRulesTaskList()
            };
        }

        public RequiredDocument GetRequiredDocuments1()
        {
            var getAlertRule = new InitAletRule();
            return new RequiredDocument()
            {
                Code = "184",
                Description = "צום שעתיים לפני",
                IsRequired = true,
                ServiceTypeID = 1,

                AlertRules = getAlertRule.GetAlertRulesDocumentList()
            };
        }
    }
}
