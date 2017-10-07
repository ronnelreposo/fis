using System;

namespace FIS.Records
{
  internal struct FacultyRec
  {
    internal PersonalRec PersonalRec { get; private set; }
    internal int ID { get; private set; }
    internal DateTime DateHired { get; private set; }
    internal string Status { get; private set; }

    FacultyRec(int iD, PersonalRec personalRec, DateTime dateHired, string status)
    {
      ID = iD;
      PersonalRec = personalRec;
      DateHired = dateHired;
      Status = status;
    }
    internal static FacultyRec Create(int ID, PersonalRec PersonalRec, DateTime DateHired, string Status)
    => new FacultyRec(iD: ID, personalRec: PersonalRec, dateHired: DateHired, status: Status);
  }
}