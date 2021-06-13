namespace Quze.Infrastruture.Types
{
    public class Error
    {
        public const int AuthonticationFailed = 100;
        public static Error WrongId = new Error(101, "wrong id");

        public int Code { get; set; }
        public string Description { get; set; }

        public Error()
        {

        }

        public Error(int code, string description)
        {
            Code = code;
            Description = description;
        }
    }

}
