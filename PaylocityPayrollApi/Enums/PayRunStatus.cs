namespace PaylocityPayrollApi.Enums
{
    public enum PayRunStatus : byte
    {
        Created = 0,
        Calculating = 1,
        Failed = 2,
        Ready = 3,
        Posted = 4,
        Voided = 5
    }
}
