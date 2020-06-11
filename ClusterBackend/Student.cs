namespace ClusterBackend
{
    public class Student
    {
        public int Id { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Jmbag { get; }
        public string Email { get; }
        public int Ects { get; }
        public bool IsEnrolled { get; }


        public Student(int id, string firstname, string lastname, string jmbag, string email, int ects, bool isEnrolled)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Jmbag = jmbag;
            Email = email;
            Ects = ects;
            IsEnrolled = isEnrolled;
        }
    }
}